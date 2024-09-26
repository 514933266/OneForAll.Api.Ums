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
    /// send发送订阅通知
    /// </summary>
    public class WxgzhSubscribeRequest
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
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public string Page { get; set; }

        /// <summary>
        /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
        /// </summary>
        [JsonProperty(PropertyName = "miniprogram")]
        public WxgzhSubscribeMiniprogramRequest MiniProgram { get; set; }

        /// <summary>
        /// 模板数据
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }

    /// <summary>
    /// 跳小程序所需数据，不需跳小程序可不用传该数据
    /// </summary>
    public class WxgzhSubscribeMiniprogramRequest
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
