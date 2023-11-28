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
    /// 微信公众号模板消息
    /// </summary>
    public class WechatGzhTemplateRequest
    {
        /// <summary>
        /// 接收者openid
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "touser")]
        public string ToUser { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板跳转链接（海外账号没有跳转能力）
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// 跳小程序所需数据，不需跳小程序可不用传该数据
        /// </summary>
        [JsonProperty(PropertyName = "miniprogram")]
        public WechatGzhTemplateMsgMiniRequest MiniProgram { get; set; }

        /// <summary>
        /// 模板数据
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        /// <summary>
        /// 防重入id。对于同一个openid + client_msg_id, 只发送一条消息,10分钟有效,超过10分钟不保证效果。若无防重入需求，可不填
        /// </summary>
        [JsonProperty(PropertyName = "client_msg_id")]
        public string ClientMsgId { get; set; }
    }

    /// <summary>
    /// 跳小程序所需数据，不需跳小程序可不用传该数据
    /// </summary>
    public class WechatGzhTemplateMsgMiniRequest
    {
        /// <summary>
        /// 所需跳转到的小程序appid（该小程序appid必须与发模板消息的公众号是绑定关联关系，暂不支持小游戏）
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "appid")]
        public string AppId { get; set; }

        /// <summary>
        /// 所需跳转到小程序的具体页面路径，支持带参数,（示例index?foo=bar），要求该小程序已发布，暂不支持小游戏
        /// </summary>
        [JsonProperty(PropertyName = "pagepath")]
        public string PagepPath { get; set; }
    }
}
