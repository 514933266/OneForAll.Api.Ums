using AutoMapper;
using Ums.Application.Dtos;
using Ums.Domain.Entities;
using Ums.Domain.Models;

namespace Ums.Host.Profiles
{
    public class UmsNotificationConfigProfile : Profile
    {
        public UmsNotificationConfigProfile()
        {
            CreateMap<UmsNotificationConfig, UmsNotificationConfigDto>();
            CreateMap<UmsNotificationConfigForm, UmsNotificationConfig>();
        }
    }
}
