using AutoMapper;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class WechatQyRobotProfile : Profile
    {
        public WechatQyRobotProfile()
        {
            CreateMap<WechatQyRobotTextForm, WechatQyRobotTextRequest>()
                .ForPath(t => t.Text.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.Text.MentionedList, a => a.MapFrom(e => e.UserIds))
                .ForPath(t => t.Text.MentionedMobileList, a => a.MapFrom(e => e.Mobiles));

            CreateMap<WechatQyRobotTextForm, WechatQyRobotMarkdownRequest>()
                .ForPath(t => t.Markdown.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.Markdown.MentionedList, a => a.MapFrom(e => e.UserIds))
                .ForPath(t => t.Markdown.MentionedMobileList, a => a.MapFrom(e => e.Mobiles));
        }
    }
}

