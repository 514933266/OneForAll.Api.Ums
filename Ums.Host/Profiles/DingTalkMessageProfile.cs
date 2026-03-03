using AutoMapper;
using Ums.Domain.Entities;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class DingTalkMessageProfile : Profile
    {
        public DingTalkMessageProfile()
        {
            CreateMap<DingTalkRobotMessageForm, DingTalkRobotTextMessageRequest>()
                .ForPath(t => t.Text.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.At.AtMobiles, a => a.MapFrom(e => e.AtMobiles))
                .ForPath(t => t.At.AtUserIds, a => a.MapFrom(e => e.AtUserIds))
                .ForPath(t => t.At.IsAtAll, a => a.MapFrom(e => e.IsAtAll));

            CreateMap<DingTalkRobotMessageForm, DingTalkRobotMarkdownMessageRequest>()
                .ForPath(t => t.Markdown.Title, a => a.MapFrom(e => e.Title))
                .ForPath(t => t.Markdown.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.At.AtMobiles, a => a.MapFrom(e => e.AtMobiles))
                .ForPath(t => t.At.AtUserIds, a => a.MapFrom(e => e.AtUserIds))
                .ForPath(t => t.At.IsAtAll, a => a.MapFrom(e => e.IsAtAll));
        }
    }
}
