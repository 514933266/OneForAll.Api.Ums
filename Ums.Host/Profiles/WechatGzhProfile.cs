using AutoMapper;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class WechatGzhProfile : Profile
    {
        public WechatGzhProfile()
        {
            CreateMap<WechatGzhTemplateForm, WechatGzhTemplateRequest>();
            CreateMap<WechatGzhTemplateMsgMiniForm, WechatGzhTemplateMsgMiniRequest>();
        }
    }
}
