﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 微信小程序订阅模板消息
    /// </summary>
    public class WxmpSubscribeForm
    {
        /// <summary>
        /// 请求凭证
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// 接收者openid
        /// </summary>
        [Required]
        public string ToUser { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Required]
        public string TemplateId { get; set; }

        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转
        /// </summary>
        public string Page { get; set; }

        /// <summary>
        /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
        /// </summary>
        public string MiniProgramState { get; set; } = "formal";

        /// <summary>
        /// 模板数据
        /// </summary>
        [Required]
        public object Data { get; set; }

        /// <summary>
        /// 进入小程序查看”的语言类型，支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，默认为zh_CN
        /// </summary>
        public string Lang { get; set; } = "zh_CN";
    }
}
