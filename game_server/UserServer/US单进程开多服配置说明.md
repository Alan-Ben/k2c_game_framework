# US 单进程开多服 - 配置文件名对照关系

## 核心规则

```
配置文件后缀 = 服务器 typeId
```

## 对照关系表

假设配置：
```properties
UserServer.openCount = 3
UserServer.typeId = 10001:10002:10003
```

**文件名对照**：

| 服务器ID | 配置文件后缀 | 实际文件名 |
|---------|------------|-----------|
| 10001 | **10001** | `WCGBasicServerConf10001.properties` |
|  |  | `UserDBConf10001.properties` |
|  |  | `UserLogDBConf10001.properties` |
| 10002 | **10002** | `WCGBasicServerConf10002.properties` |
|  |  | `UserDBConf10002.properties` |
|  |  | `UserLogDBConf10002.properties` |
| 10003 | **10003** | `WCGBasicServerConf10003.properties` |
|  |  | `UserDBConf10003.properties` |
|  |  | `UserLogDBConf10003.properties` |

## 关键点

1. **后缀直接使用服务器ID**：typeId 是多少，配置文件后缀就是多少
2. **所有配置文件统一后缀**：同一个服务器的所有配置文件使用相同的 typeId 作为后缀
3. **简化对应关系**：不再需要计算位置，直接使用服务器ID

## 通用公式

```
typeId = 10001:10002:10003
         ↓     ↓     ↓
后缀 =   10001:10002:10003
```

## 配置步骤

1. 修改 `UserServerConf.properties`：
   ```properties
   UserServer.openCount = N
   UserServer.typeId = 服务器ID列表（冒号分隔）
   ```

2. 创建配置文件（N 份）：
   - 为每个服务器ID创建对应的配置文件
   - 配置文件名格式：`配置名{服务器ID}.properties`
   - 例如：`UserDBConf10001.properties`、`UserDBConf10002.properties`

3. 修改各配置文件的差异项（端口、数据库等）

4. 启动进程，系统自动根据 typeId 加载对应后缀的配置文件

## 示例

假设要开3个服务器（ID：10001、10002、10003）：

1. **主配置**：
   ```properties
   UserServer.openCount = 3
   UserServer.typeId = 10001:10002:10003
   ```

2. **创建配置文件**：
   - `WCGBasicServerConf10001.properties`（端口 6700）
   - `WCGBasicServerConf10002.properties`（端口 6701）
   - `WCGBasicServerConf10003.properties`（端口 6702）
   - `UserDBConf10001.properties`（数据库 gob_user_10001）
   - `UserDBConf10002.properties`（数据库 gob_user_10002）
   - `UserDBConf10003.properties`（数据库 gob_user_10003）
   - ... 其他配置文件同理
