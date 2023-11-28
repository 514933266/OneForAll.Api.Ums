using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Enums;

namespace Ums.Application.Dtos
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    public class UmsSmsRecordDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// 国家（或地区）码
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 原始消息
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public UmsSmsSendStatusEnum Status { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
