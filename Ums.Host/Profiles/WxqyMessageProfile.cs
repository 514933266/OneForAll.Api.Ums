using AutoMapper;
using Ums.Domain.Entities;
using Ums.Domain.Models;
using Ums.HttpService.Models;

namespace Ums.Host.Profiles
{
    public class WxqyMessageProfile : Profile
    {
        public WxqyMessageProfile()
        {
            CreateMap<WxqyRobotMessageForm, WxqyRobotTextMessageRequest>()
                .ForPath(t => t.Text.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.Text.MentionedList, a => a.MapFrom(e => e.UserIds))
                .ForPath(t => t.Text.MentionedMobileList, a => a.MapFrom(e => e.Mobiles));

            CreateMap<WxqyRobotMessageForm, WxqyRobotMarkdownMessageRequest>()
                .ForPath(t => t.Markdown.Content, a => a.MapFrom(e => e.Content))
                .ForPath(t => t.Markdown.MentionedList, a => a.MapFrom(e => e.UserIds))
                .ForPath(t => t.Markdown.MentionedMobileList, a => a.MapFrom(e => e.Mobiles));
        }
    }
}

