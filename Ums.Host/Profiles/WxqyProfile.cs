using AutoMapper;
using Ums.Domain.AggregateRoots;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class WxqyProfile : Profile
    {
        public WxqyProfile()
        {
            CreateMap<WxqyRobotTextForm, WxqyRobotTextRequest>()
                .ForPath(t => t.Text.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.Text.MentionedList, a => a.MapFrom(e => e.UserIds))
                .ForPath(t => t.Text.MentionedMobileList, a => a.MapFrom(e => e.Mobiles));

            CreateMap<WxqyRobotTextForm, WxqyRobotMarkdownRequest>()
                .ForPath(t => t.Markdown.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.Markdown.MentionedList, a => a.MapFrom(e => e.UserIds))
                .ForPath(t => t.Markdown.MentionedMobileList, a => a.MapFrom(e => e.Mobiles));
        }
    }
}

