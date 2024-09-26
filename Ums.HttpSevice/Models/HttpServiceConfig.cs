using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ums.HttpService.Models
{
    /// <summary>
    /// 数据资源服务配置
    /// </summary>
    public class HttpServiceConfig
    {
        /// <summary>
        /// 权限验证接口
        /// </summary>
        public string SysPermissionCheck { get; set; } = "SysPermissionCheck";

        /// <summary>
        /// Api日志
        /// </summary>
        public string SysApiLog { get; set; } = "SysApiLog";

        /// <summary>
        /// 异常日志
        /// </summary>
        public string SysExceptionLog { get; set; } = "SysExceptionLog";

        /// <summary>
        /// 全局异常日志
        /// </summary>
        public string SysGlobalExceptionLog { get; set; } = "SysGlobalExceptionLog";

        /// <summary>
        /// 操作日志
        /// </summary>
        public string SysOperationLog { get; set; } = "SysOperationLog";

        /// <summary>
        /// 企业微信机器人
        /// </summary>
        public string WxQyRobot { get; set; } = "WxQyRobot";

        /// <summary>
        /// 微信公众号模板消息
        /// </summary>
        public string WxgzhTemplate { get; set; } = "WxgzhTemplate";

        /// <summary>
        /// 微信公众号长期订阅消息
        /// </summary>
        public string WxgzhSubscribe { get; set; } = "WxgzhSubscribe";

        /// <summary>
        /// 微信公众号模板消息
        /// </summary>
        public string WxmpSubscribe { get; set; } = "WxmpSubscribe";
        
        /// <summary>
        /// 定时任务调度中心
        /// </summary>
        public string ScheduleJob { get; set; } = "ScheduleJob";
    }
}
