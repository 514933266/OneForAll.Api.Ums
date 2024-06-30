using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.Models
{
    /// <summary>
    /// 短信消息
    /// </summary>
    public class TxCloudSmsForm
    {
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
        [StringLength(10)]
        public string SignName { get; set; }

        /// <summary>
        /// 模板id
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TemplateId { get; set; }

        /// <summary>
        /// 手机号（多个号码以,号隔开）
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 短信内容（多个参数以,号隔开）
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [Required]
        public string Sign { get; set; }
    }
}
