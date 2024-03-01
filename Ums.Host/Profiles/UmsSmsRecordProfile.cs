using AutoMapper;
using Ums.Application.Dtos;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Models;

namespace Ums.Host.Profiles
{
    public class UmsSmsRecordProfile : Profile
    {
        public UmsSmsRecordProfile()
        {
            CreateMap<UmsSmsRecord, UmsSmsRecordDto>();
        }
    }
}
