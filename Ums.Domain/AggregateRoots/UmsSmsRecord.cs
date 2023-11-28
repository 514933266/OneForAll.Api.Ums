using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Domain.Enums;

namespace Ums.Domain.AggregateRoots
{
    /// <summary>
    /// 短信发送记录
    /// </summary>
    public class UmsSmsRecord
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PlatformName { get; set; }

        /// <summary>
        /// 所属模块名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleCode { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        [Required]
        [StringLength(10)]
        public string SignName { get; set; } = "";

        /// <summary>
        /// 国家（或地区）码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string NationCode { get; set; } = "";

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 原始消息
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Content { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        [Required]
        [StringLength(200)]
        public UmsSmsSendStatusEnum Status { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ErrMsg { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
