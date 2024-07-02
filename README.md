## 项目架构



- dotnetVerson 8.0.101
- wpf + webapi
- sqlite
- communityToolkit.Mvvm
- log4Net
- sqlSugarCore
- fastReport







### 接口

1. 获取标签模板列表（已实现）
2. 删除标签模板（已实现）
3. 添加标签模板（已实现）
4. 添加打印机（已实现）
5. 删除打印机（已实现）
6. 获取打印机列表（已实现）
7. 打印简单数据标签（标签数据key-value形式）（已实现）
8. 打印复杂数据标签（暂未考虑复杂形式）（待实现接口）





### 技术难点问题

1. 日志框架log4Net使用wpf基础日志和asp.net core如何共用，目前之启用了基础日志；使用asp.net core日志时wpf订阅不到该日志。
2. 遇到swagger的坑一个控制器内重复请求方法类型是swagger无法解析需要自定义





**该项目仅供参考使用学习，禁止用于商业活动。**