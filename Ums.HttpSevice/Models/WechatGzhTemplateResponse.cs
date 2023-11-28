using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.HttpService.Models
{
    /// <summary>
    /// 微信公众号消息推送
    /// </summary>
    public class WechatGzhTemplateResponse
    {
        /// <summary>
        /// 消息id
        /// </summary>
        [JsonProperty(PropertyName = "msgid")]
        public string MsgId { get; set; }

        // <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "errcode")]
        public string ErrCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "errmsg")]
        public string ErrMsg { get; set; }
    }
}
