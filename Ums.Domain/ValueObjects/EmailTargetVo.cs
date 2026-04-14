using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Domain.ValueObjects
{
    /// <summary>
    /// 邮件通知目标配置
    /// </summary>
    public class EmailTargetVo
    {
        /// <summary>
        /// 收件人邮箱（多个以,号隔开）
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 抄送邮箱（多个以,号隔开）
        /// </summary>
        public string Cc { get; set; }
    }
}
