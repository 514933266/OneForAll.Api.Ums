using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Autofac;
using Autofac.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneForAll.File;
using OneForAll.EFCore;
using OneForAll.Core.Extension;
using Ums.Host.Models;
using Ums.Host.Filters;
using Ums.Public.Models;
using Ums.HttpService.Models;
using OneForAll.Core.Upload;
using MongoDB.Driver;

namespace Ums.Host
{
    public class Startup
    {
        private const string CORS = "Cors";
        private const string AUTH = "Auth";

        private const string HTTP_SERVICE = "Ums.HttpService";
        private const string HTTP_SERVICE_KEY = "HttpService";

        private const string BASE_HOST = "Ums.Host";
        private const string BASE_DOMAIN = "Ums.Domain";
        private const string BASE_APPLICATION = "Ums.Application";
        private const string BASE_REPOSITORY = "Ums.Repository";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors

            var corsConfig = new CorsConfig();
            Configuration.GetSection(CORS).Bind(corsConfig);
            services.AddCors(option => option.AddPolicy(CORS, policy => policy
                .WithOrigins(corsConfig.Origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            ));

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"OA服务接口文档 {version}",
                        Description = $"OneForAll OA Web API {version}"
                    });
                });

                var xmlHostFile = BASE_HOST.Append(".xml");
                var xmlAppFile = BASE_APPLICATION.Append(".xml");
                var xmlDomainFile = BASE_DOMAIN.Append(".xml");
                var xmlHostPath = Path.Combine(AppContext.BaseDirectory, xmlHostFile);
                var xmlAppPath = Path.Combine(AppContext.BaseDirectory, xmlAppFile);
                var xmlDomainPath = Path.Combine(AppContext.BaseDirectory, xmlDomainFile);
                c.IncludeXmlComments(xmlHostPath, true);
                c.IncludeXmlComments(xmlAppPath);
                c.IncludeXmlComments(xmlDomainPath);
            });

            #endregion

            #region Mvc

            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ApiModelStateFilter>();
                options.Filters.Add<ExceptionFilter>();

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            #endregion

            #region Http数据服务

            var serviceConfig = new HttpServiceConfig();
            Configuration.GetSection(HTTP_SERVICE_KEY).Bind(serviceConfig);
            var props = OneForAll.Core.Utility.ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(e.GetValue(serviceConfig).ToString());
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });

            #endregion

            #region IdentityServer4

            var authConfig = new AuthConfig();
            Configuration.GetSection(AUTH).Bind(authConfig);
            services.AddAuthentication(authConfig.Type)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = authConfig.Url;
                options.RequireHttpsMetadata = false;
            });

            #endregion

            #region AutoMapper

            services.AddAutoMapper(config =>
            {
                config.AllowNullDestinationValues = false;
            }, Assembly.Load(BASE_HOST));

            #endregion

            #region EFCore

            services.AddDbContext<OneForAllContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddSingleton<IUploader, Uploader>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpServiceConfig>();

            #endregion

            #region RabbitMQ

            var rabbitmqConfig = new RabbitMqConnectionConfig();
            Configuration.GetSection("RabbitMQ").Bind(rabbitmqConfig);
            var factory = new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = rabbitmqConfig.HostName,
                Port = rabbitmqConfig.Port,
                UserName = rabbitmqConfig.UserName,
                Password = rabbitmqConfig.Password,
                VirtualHost = rabbitmqConfig.VirtualHost
            };
            services.AddSingleton(factory);
            services.AddHostedService<MessageConsumerHostedService>();
            #endregion

            #region MongoDb

            var mongoDbClient = new MongoClient(Configuration["MongoDb:ConnectionString"]);
            var mongoDb = mongoDbClient.GetDatabase(Configuration["MongoDb:DatabaseName"]);
            services.AddSingleton(mongoDb);

            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Http数据服务
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // 基础
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IEFCoreRepository<>));

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(OneForAllContext)).Named<DbContext>("OneForAllContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("OneForAllContext"))
               .AsImplementedInterfaces();

            var authConfig = new AuthConfig();
            Configuration.GetSection(AUTH).Bind(authConfig);
            builder.Register(s => authConfig).SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                    });
                });
            }

            DirectoryHelper.Create(Path.Combine(Directory.GetCurrentDirectory(), @"upload"));
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"upload")),
                RequestPath = new PathString("/resources"),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            });

            app.UseRouting();

            app.UseCors(CORS);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ApiLogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
