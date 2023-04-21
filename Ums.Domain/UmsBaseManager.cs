using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ums.Domain.Models;
using Ums.Public.Models;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain
{
    /// <summary>
    /// 基类
    /// </summary>
    public class UmsBaseManager : BaseManager
    {
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public UmsBaseManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_ID);

                if (userId != null)
                {
                    return new Guid(userId.Value);
                }
                return Guid.Empty;
            }
        }

        protected string UserName
        {
            get
            {
                var username = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USERNAME);

                if (username != null)
                {
                    return username.Value;
                }
                return null;
            }
        }

        protected Guid TenantId
        {
            get
            {
                var tenantId = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);

                if (tenantId != null)
                {
                    return new Guid(tenantId.Value);
                }
                return Guid.Empty;
            }
        }

        protected LoginUser LoginUser
        {
            get
            {
                var name = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME);

                var role = _httpContextAccessor.HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.ROLE);

                return new LoginUser()
                {
                    Id = UserId,
                    Name = name.Value,
                    TenantId = TenantId
                };
            }
        }
    }
}
