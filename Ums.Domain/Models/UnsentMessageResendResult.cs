namespace Ums.Domain.Models
{
    /// <summary>
    /// 未发送消息重发结果
    /// </summary>
    public class UnsentMessageResendResult
    {
        /// <summary>
        /// 待重发记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 重发成功记录数
        /// </summary>
        public int SuccessCount { get; set; }
    }
}
