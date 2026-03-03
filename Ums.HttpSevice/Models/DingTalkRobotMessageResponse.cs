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
    /// 钉钉机器人消息响应
    /// </summary>
    public class DingTalkRobotMessageResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty(PropertyName = "errmsg")]
        public string ErrMsg { get; set; }
    }
}
