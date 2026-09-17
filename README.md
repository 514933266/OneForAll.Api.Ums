# OneForAll.Api.Ums

基于 ASP.NET Core 8.0 构建的通用消息推送服务，支持多种消息渠道（短信、邮件、企业微信、钉钉、微信公众号等），提供消息去重、监控告警及统一管理功能。

## 技术栈

- **.NET 8.0** / ASP.NET Core
- **Entity Framework Core 8.0** - ORM（SQL Server）
- **Autofac 9.0** - 依赖注入
- **AutoMapper 16.0** - 对象映射
- **JWT Bearer** - 身份认证
- **RabbitMQ** - 消息队列（异步消费）
- **Quartz.NET 3.15** - 定时任务调度

## 项目结构

```
OneForAll.Api.Ums/
├── Ums.Host/                # Web 主机层（API 入口、中间件、Quartz 配置）
├── Ums.Application/         # 应用服务层（业务用例、DTO、外部服务集成）
├── Ums.Domain/              # 领域层（实体、枚举、业务逻辑、仓储接口）
├── Ums.Repository/          # 数据访问层（EF Core 仓储实现）
├── Ums.HttpService/         # 外部 HTTP 服务客户端
└── Ums.Public/              # 公共模型
```

## 核心功能

### 多渠道消息推送

支持以下消息类型：

| 渠道 | 说明 |
|------|------|
| 腾讯云短信 | 发送验证码、通知类短信 |
| 邮件通知 | SMTP 协议发送邮件 |
| 企业微信机器人 | 群机器人文本/Markdown 消息 |
| 钉钉机器人 | 群机器人文本/Markdown 消息 |
| 微信公众号模板消息 | 向关注用户发送模板消息 |
| 微信公众号订阅消息 | 小程序订阅消息推送 |
| 站内个人消息 | 系统内用户消息通知 |

### 消息去重机制

基于内容匹配规则和时间窗口，防止重复消息推送：

- **匹配规则**：支持按标题、内容、接收人等维度组合匹配
- **去重策略**：拦截重复消息或合并发送
- **时间窗口**：可配置去重有效期（如 5 分钟内相同内容不重复发送）

### 异步消息消费

通过 RabbitMQ 实现消息异步处理，各渠道独立消费者：

- SystemMessageConsumer - 站内消息
- TxCloudSmsConsumer - 腾讯云短信
- WxgzhMessageConsumer - 微信公众号消息
- WxmpMessageConsumer - 微信小程序消息
- WxqyMessageConsumer - 企业微信消息
- DingTalkMessageConsumer - 钉钉消息
- EmailMessageConsumer - 邮件消息

### 监控与告警

- Quartz 定时任务监控未发送消息队列
- 消息发送记录持久化（成功/失败状态）
- 支持配置告警通知（企业微信/钉钉）

## API 接口

### 消息发送接口

| 方法 | 路径 | 说明 |
|------|------|------|
| POST | `/api/UmsMessages` | 发送通用消息（自动路由到对应渠道） |
| POST | `/api/TxCloudSms` | 发送腾讯云短信 |
| POST | `/api/UmsEmailMessages` | 发送邮件 |
| POST | `/api/DingTalkMessages` | 发送钉钉机器人消息 |
| POST | `/api/WxqyMessage` | 发送企业微信机器人消息 |
| POST | `/api/WxgzhMessage/Template` | 发送微信公众号模板消息 |
| POST | `/api/WxgzhMessage/Subscribe` | 发送微信公众号订阅消息 |
| POST | `/api/WxmpMessage` | 发送微信小程序订阅消息 |
| POST | `/api/UmsPersonalMessages` | 发送站内个人消息 |

### 管理界面

访问 `http://localhost:5085/` 打开内置管理界面（Vue3 + Element Plus 单页应用，静态文件位于 `Ums.Host/wwwroot`），包含：

- 消息日志、短信记录、去重记录查询（时间范围筛选精确到秒）
- 通知配置管理（增删改查）
- 内置定时任务控制（暂停 / 恢复 / 执行一次）

### 管理接口（无需认证）

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/UmsMessageLogs/{pageIndex}/{pageSize}` | 分页查询消息发送日志 |
| GET | `/api/UmsSmsRecords/{pageIndex}/{pageSize}` | 分页查询短信发送记录 |
| GET | `/api/UmsNotificationConfigs/{pageIndex}/{pageSize}` | 分页查询通知配置 |
| POST | `/api/UmsNotificationConfigs` | 创建通知配置 |
| PUT | `/api/UmsNotificationConfigs/{id}` | 更新通知配置 |
| DELETE | `/api/UmsNotificationConfigs/{id}` | 删除通知配置 |
| GET | `/api/UmsDeduplicationRecords/{pageIndex}/{pageSize}` | 分页查询去重记录 |
| POST | `/api/UmsDeduplicationRecords` | 添加去重记录 |

### 系统接口

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/Startups` | 健康检查（供调度中心探测） |
| POST | `/api/Startups/Default/Jobs/{jobName}/Stop` | 暂停内置定时任务 |
| POST | `/api/Startups/Default/Jobs/{jobName}/Resume` | 恢复内置定时任务 |
| POST | `/api/Startups/Default/Jobs/{jobName}/Excute` | 执行一次内置定时任务 |

## 数据库

使用 SQL Server，数据库名 `OneForAll.Ums`，主要表：

| 表名 | 说明 |
|------|------|
| ums_message | 消息定义（类型、内容、接收人、状态等） |
| ums_message_record | 消息发送记录（发送时间、结果、错误信息） |
| ums_sms_record | 短信发送记录（手机号、模板ID、发送状态） |
| ums_notification_config | 通知配置（Webhook、模板ID、渠道参数） |
| ums_deduplication_config | 去重配置（匹配规则、时间窗口、策略） |
| ums_deduplication_record | 去重记录（消息指纹、命中次数、时间戳） |

## 配置说明

`appsettings.json` 关键配置项：

```json
{
  "ConnectionStrings": {
    "Default": "SQL Server 连接字符串"
  },
  "Auth": {
    "JwtKey": "JWT 签名密钥",
    "Issuer": "令牌签发地址",
    "ClientId": "客户端ID",
    "ClientSecret": "客户端密钥"
  },
  "HttpService": {
    "SysBase": "http://localhost:5082",
    "SysLog": "http://localhost:5084",
    "SysUms": "http://localhost:5085",
    "SysJob": "http://localhost:5086",
    "Weixin": "https://api.weixin.qq.com"
  },
  "RabbitMQ": {
    "IsEnabled": false,
    "Host": "localhost",
    "Port": "5672",
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "Consumers": [
      "SystemMessageConsumerHostedService",
      "TxCloudSmsConsumerHostedService",
      "WxgzhMessageConsumerHostedService",
      "WxmpMessageConsumerHostedService",
      "WxqyMessageConsumerHostedService",
      "DingTalkMessageConsumerHostedService",
      "EmailMessageConsumerHostedService"
    ]
  },
  "Sms": {
    "TxCloud": {
      "SecretId": "腾讯云 SecretId",
      "SecretKey": "腾讯云 SecretKey",
      "AppId": "腾讯云 AppId"
    }
  },
  "Email": {
    "SmtpHost": "smtp.example.com",
    "SmtpPort": "587",
    "UserName": "邮箱账号",
    "Password": "邮箱授权码",
    "DisplayName": "系统通知",
    "EnableSsl": "true"
  },
  "Quartz": {
    "IsEnabled": false,
    "AppId": "OneForAll.Ums",
    "GroupName": "消息服务",
    "NodeName": "http://localhost:5085",
    "ScheduleJobs": [
      {
        "TypeName": "MonitorUnsentMessageJob",
        "Corn": "0 0/20 * * * ?",
        "Remark": "监控当天未发送队列消息（超一小时）"
      }
    ]
  }
}
```

| 配置项 | 说明 |
|--------|------|
| `RabbitMQ.IsEnabled` | 是否启用 RabbitMQ 消息队列消费 |
| `RabbitMQ.Consumers` | 启用的消费者列表（按需配置） |
| `Sms.TxCloud` | 腾讯云短信凭证（发送短信必填） |
| `Email` | SMTP 邮件服务器配置（发送邮件必填） |
| `Quartz.IsEnabled` | 是否启用 Quartz 定时任务 |
| `Quartz.ScheduleJobs` | 内置调度任务列表 |

## 外部服务依赖

本服务作为 OneForAll 微服务体系的一部分，依赖以下服务：

| 服务 | 配置键 | 说明 |
|------|--------|------|
| SysBase | `HttpService.SysBase` | 基础系统服务（租户、用户信息） |
| SysLog | `HttpService.SysLog` | 日志服务（操作日志、异常日志） |
| SysUms | `HttpService.SysUms` | 消息服务（自身，用于内部调用） |
| SysJob | `HttpService.SysJob` | 定时任务调度服务（注册任务、上报心跳） |
| Weixin | `HttpService.Weixin` | 微信开放平台 API |

## 启动运行

1. 确保 SQL Server 已就绪，并更新 `appsettings.json` 中的连接字符串
2. 配置 JWT 认证参数（与统一认证中心一致）
3. 按需配置外部服务（腾讯云短信、SMTP 邮件、RabbitMQ、微信开放平台）
4. 如需启用消息队列，配置 RabbitMQ 连接信息并设置 `IsEnabled: true`
5. 如需启用定时监控，配置 Quartz 参数并设置 `IsEnabled: true`

```bash
dotnet restore
dotnet run --project Ums.Host
```

服务默认监听配置文件 `Urls` 中指定的端口（通常为 `http://*:5085`）。

## 消息发送流程

```
客户端请求 → API Controller → Application Service → Domain Manager
                                    ↓
                            消息去重检查（可选）
                                    ↓
                         发布到 RabbitMQ Queue（异步）
                                    ↓
                          Consumer 消费并调用第三方 API
                                    ↓
                         记录发送结果到数据库
```

## 注意事项

- 发送短信需先在腾讯云控制台申请模板和签名
- 微信公众号消息需先完成公众号认证并配置模板
- 企业微信/钉钉机器人需在群聊中添加机器人并获取 Webhook
- 消息去重功能需在数据库中配置 `ums_deduplication_config` 规则后生效