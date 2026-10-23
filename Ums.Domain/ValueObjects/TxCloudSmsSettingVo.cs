namespace Ums.Domain.ValueObjects
{
    /// <summary>
    /// 腾讯云短信配置（对应 appsettings.json 的 Sms:TxCloud 节）
    /// </summary>
    public class TxCloudSmsSettingVo
    {
        /// <summary>
        /// 密钥ID
        /// </summary>
        public string SecretId { get; set; }

        /// <summary>
        /// 密钥Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
    }
}
