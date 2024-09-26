using AutoMapper;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class WxgzhProfile : Profile
    {
        public WxgzhProfile()
        {
            CreateMap<WxgzhTemplateForm, WxgzhTemplateRequest>();
            CreateMap<WechatGzhTemplateMsgMiniForm, WechatGzhTemplateMsgMiniRequest>();
            CreateMap<WxgzhSubscribeForm, WxgzhSubscribeRequest>();
            CreateMap<WxgzhSubscribeMiniProgramForm, WxgzhSubscribeMiniprogramRequest>();
        }
    }
}
