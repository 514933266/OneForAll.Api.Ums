using AutoMapper;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Models;

namespace Ums.Host.Profiles
{
    public class UmsMessageProfile : Profile
    {
        public UmsMessageProfile()
        {
            CreateMap<UmsMessageForm, UmsMessage>();
        }
    }
}
