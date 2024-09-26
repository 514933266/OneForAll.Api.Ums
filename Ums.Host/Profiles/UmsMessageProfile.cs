using AutoMapper;
using Ums.Application.Dtos;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Models;

namespace Ums.Host.Profiles
{
    public class UmsMessageProfile : Profile
    {
        public UmsMessageProfile()
        {
            CreateMap<UmsMessage, UmsPersonalMessageDto>();
            CreateMap<UmsMessageForm, UmsMessage>();
            CreateMap<UmsMessageForm, UmsMessageRecord>();
        }
    }
}
