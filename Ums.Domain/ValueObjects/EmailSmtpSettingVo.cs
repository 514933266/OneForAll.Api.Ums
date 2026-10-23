namespace Ums.Domain.ValueObjects
{
    /// <summary>
    /// 邮件SMTP配置（对应 appsettings.json 的 Email 节）
    /// </summary>
    public class EmailSmtpSettingVo
    {
        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        public string SmtpPort { get; set; }

        /// <summary>
        /// 发件账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 发件密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 发件人显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public string EnableSsl { get; set; }
    }
}
