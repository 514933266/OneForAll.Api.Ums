using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ums.Public.Models
{
    /// <summary>
    /// MongoDB链接配置
    /// </summary>
    public class MongoDbConnectionConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; } = "OneForAll_Ums";
    }
}
