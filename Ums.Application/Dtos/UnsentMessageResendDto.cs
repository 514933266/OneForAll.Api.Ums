namespace Ums.Application.Dtos
{
    /// <summary>
    /// 未发送消息重发结果
    /// </summary>
    public class UnsentMessageResendDto
    {
        /// <summary>
        /// 待重发记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 重发成功记录数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 执行结果说明
        /// </summary>
        public string Message { get; set; }
    }
}
