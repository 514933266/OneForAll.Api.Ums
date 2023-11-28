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
    /// 企业微信-发送消息
    /// </summary>
    public class WechatQyRobotRequest
    {
        /// <summary>
        /// 消息类型，此时固定为：text
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "msgtype")]
        public string MsgType { get; set; }
    }

    /// <summary>
    /// 发送文本消息
    /// </summary>
    public class WechatQyRobotTextRequest : WechatQyRobotRequest
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "text")]
        public WechatQyRobotTextContentRequest Text { get; set; } = new WechatQyRobotTextContentRequest();
    }

    /// <summary>
    /// 发送文本消息
    /// </summary>
    public class WechatQyRobotTextContentRequest
    {
        /// <summary>
        /// 文本内容，最长不超过2048个字节，必须是utf8编码
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        /// <summary>
        /// userid的列表，提醒群中的指定成员(@某个成员)，@all表示提醒所有人，如果开发者获取不到userid，可以使用mentioned_mobile_list
        /// </summary>
        [JsonProperty(PropertyName = "mentioned_list")]
        public List<string> MentionedList { get; set; } = new List<string>(); 

        /// <summary>
        /// 手机号列表，提醒手机号对应的群成员(@某个成员)，@all表示提醒所有人
        /// </summary>
        [JsonProperty(PropertyName = "mentioned_mobile_list")]
        public List<string> MentionedMobileList { get; set; } = new List<string>();
    }

    /// <summary>
    /// Markdown请求
    /// </summary>
    public class WechatQyRobotMarkdownRequest : WechatQyRobotRequest
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "markdown")]
        public WechatQyRobotTextContentRequest Markdown { get; set; } = new WechatQyRobotTextContentRequest();
    }
}
