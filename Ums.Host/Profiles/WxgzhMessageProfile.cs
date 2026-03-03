using AutoMapper;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class WxgzhMessageProfile : Profile
    {
        public WxgzhMessageProfile()
        {
            CreateMap<WxgzhTemplateMessageForm, WxgzhTemplateMessageRequest>();
            CreateMap<WechatGzhTemplateMsgMiniForm, WechatGzhTemplateMsgMiniRequest>();
            CreateMap<WxgzhSubscribeMessageForm, WxgzhSubscribeMessageRequest>();
            CreateMap<WxgzhSubscribeMiniProgramForm, WxgzhSubscribeMiniprogramRequest>();
        }
    }
}
