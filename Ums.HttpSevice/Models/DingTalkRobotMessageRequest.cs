using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.HttpService.Models
{
    /// <summary>
    /// 钉钉机器人-发送消息
    /// </summary>
    public class DingTalkRobotMessageRequest
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "msgtype")]
        public string MsgType { get; set; }
    }

    /// <summary>
    /// 钉钉机器人-发送文本消息
    /// </summary>
    public class DingTalkRobotTextMessageRequest : DingTalkRobotMessageRequest
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "text")]
        public DingTalkRobotTextContentRequest Text { get; set; } = new DingTalkRobotTextContentRequest();

        /// <summary>
        /// @信息
        /// </summary>
        [JsonProperty(PropertyName = "at")]
        public DingTalkRobotAtRequest At { get; set; } = new DingTalkRobotAtRequest();
    }

    /// <summary>
    /// 钉钉机器人-文本内容
    /// </summary>
    public class DingTalkRobotTextContentRequest
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

    /// <summary>
    /// 钉钉机器人-发送Markdown消息
    /// </summary>
    public class DingTalkRobotMarkdownMessageRequest : DingTalkRobotMessageRequest
    {
        /// <summary>
        /// Markdown内容
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "markdown")]
        public DingTalkRobotMarkdownContentRequest Markdown { get; set; } = new DingTalkRobotMarkdownContentRequest();

        /// <summary>
        /// @信息
        /// </summary>
        [JsonProperty(PropertyName = "at")]
        public DingTalkRobotAtRequest At { get; set; } = new DingTalkRobotAtRequest();
    }

    /// <summary>
    /// 钉钉机器人-Markdown内容
    /// </summary>
    public class DingTalkRobotMarkdownContentRequest
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Markdown格式的消息内容
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "text")]
        public string Content { get; set; }
    }

    /// <summary>
    /// 钉钉机器人-@信息
    /// </summary>
    public class DingTalkRobotAtRequest
    {
        /// <summary>
        /// 被@人的手机号
        /// </summary>
        [JsonProperty(PropertyName = "atMobiles")]
        public List<string> AtMobiles { get; set; } = new List<string>();

        /// <summary>
        /// 被@人的用户id
        /// </summary>
        [JsonProperty(PropertyName = "atUserIds")]
        public List<string> AtUserIds { get; set; } = new List<string>();

        /// <summary>
        /// 是否@所有人
        /// </summary>
        [JsonProperty(PropertyName = "isAtAll")]
        public bool IsAtAll { get; set; }
    }
}
