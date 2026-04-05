# GitHub Copilot 指令文档

这是一个基于 Java 的分布式游戏服务器系统，具有广泛的代码生成和微服务架构。

## 架构概览

这是一个**微服务游戏服务器架构**，包含以下核心服务器：
- **GatewayServer（网关服务器）**: 客户端连接网关和负载均衡
- **UserServer（用户服务器）**: 核心游戏逻辑和玩家数据管理
- **CommonServer（通用服务器）**: 共享服务和服务器间通信
- **BusServer（总线服务器）**: 服务器间消息总线
- **LoginServer（登录服务器）**: 身份验证和账号管理
- **CrossGameServer/CrossRankServer**: 跨服功能

每个服务器遵循模式：`[ServerName]/src/NP[ServerName]/`，配置文件分别在 `conf/` 和 `customConf/`（环境覆盖）中。

## 关键开发工作流

### 构建系统
```bash
# 完整构建序列（必须按此顺序执行）
cd bat && python build_enum.py && python build_err.py && python build_rpc.py
cd ../DBTool && python genAll.py  
cd ../build && build2.bat
```

**构建顺序很重要**：枚举 → 错误码 → 协议 → 数据库 → Ant 构建。Ant 构建（`build2.xml`）按依赖顺序在 `/Lib/` 目录中创建 JAR 文件：ServerProtocol → Common → GameRes → GameLogicCommon → 各个服务器。

### 代码生成（项目核心）
- **协议**：`ServerProtocol/ProtocolScripts/ALLRPC/` 中的 `.alpro` 文件 → `python build_rpc.py` → Java 类
- **错误码**：`bat/err/[主码]_[类名]_[描述].txt` → `python build_err.py` → 跨语言错误枚举
- **数据库**：`DBTool/source_db/` 中的 Python 表定义 → `python genAll.py` → BO/BM 类
- **后处理**：`ServerProtocol/ProtocolScripts/post_exec.bat` 运行完整生成管道

## 项目特定模式

### 消息处理（UserServer 核心模式）
**5层架构**用于玩家消息处理：

1. **自动注册**：`NPUserMsgBasicDispatcher.autoRegistHandler()` 扫描 `NPUserMsgDealer<T>` 子类
2. **消息项**：`NPUSUserRequestMsgItem`（带客户端序列号）vs `NPUSUserNormalMsgItem`（推送）
3. **处理器**：命名为 `MsgDealer_[方向]_[主协议]_[副协议]_[功能]`，位于 `NPUserMsgDispather/p[XXX]_[功能]/`
4. **组件**：业务逻辑在 `[功能]Component extends _ANPUserComponent`
5. **响应构造器**：手动编写的响应构造器 `US2GCWriter_[协议]_[功能]` 在 `Write/` 目录

### 协议约定
- **网络格式**：`[方向]_[主协议]_[副协议]_[功能]`（例如：`GC2GS_036_001_ReqTreasureHuntOreCapture`）
- **网络协议**：`[主协议(1字节)] + [副协议(1字节)] + [数据(变长)]`
- **包组织**：`p[XXX]_[功能名]Op` 目录分组相关协议

### 组件系统
- **生命周期**：构造函数自动注册 → `_init()` 异步 → `getDependCompList()` → `onInited()` 回调
- **线程安全**：通过 `getUserData().lockUser()` 实现玩家级别锁保证数据一致性
- **持久化**：BM（Business Manager）模式，使用 `[功能]BO` 数据对象和 `[功能]BM.inst` 管理器

### 配置系统
- **双层**：`conf/[ServerName]Conf.properties` 中的基础配置，`customConf/` 中的覆盖
- **模式**：单例 `[ServerName]Conf.getInstance().init()` 加载两层
- **热重载**：许多配置支持运行时更新

## 集成点

### 数据库集成
- **BM 模式**：`[功能]BM.inst.insertOrUpdate[功能]([功能]BO, context)`
- **路由**：`EDBTag` 枚举路由到不同数据库
- **事务**：通过 BM 层基于上下文的事务管理
- **备份**：带时间戳的自动 `.backup` 文件

### 跨服务器通信
- **BusServer**：服务器间通信的中央消息总线
- **协议共享**：相同的 `.alpro` 定义生成客户端和服务器端代码
- **服务发现**：基于配置的服务器寻址

### 热更新系统
- **ActivitiesV01**：专用的游戏内容更新热更新模块
- **部署**：`cp_hotifx.bat` 复制热更新类到部署位置
- **集成**：热更新类可以覆盖基础功能

## 关键文件和目录

- `bat/build_*.py`：代码生成脚本（在 Ant 构建前运行这些）
- `build/build2.xml`：带依赖顺序的 Ant 构建配置
- `ServerProtocol/ProtocolScripts/ALLRPC/`：协议定义（`.alpro` 文件）
- `[Server]/src/NP[Server]/`：主服务器实现
- `UserServer/src/NPUSServer/NPUserMsgDispather/`：消息处理逻辑
- `Common/src/`：共享协议和工具类
- `DBTool/source_db/`：数据库表定义

## 要避免的反模式

- **绝不**编辑生成的文件（下次构建时会被覆盖）
- **绝不**跳过 枚举→错误→协议→数据库→构建 的序列
- **绝不**手动修改 `Common/src/` 中的文件（它们是从协议生成的）
- **不要**在没有对应 `Write/` 目录中 Writers 的情况下创建消息处理器
- **不要**忘记在 `getDependCompList()` 中声明组件依赖

## 快速参考

**添加新玩家功能**：定义 `.alpro` → 运行 `build_rpc.py` → 创建 `MsgDealer_*` → 创建 `Writer_*` → 实现 `*Component` → 构建
**添加数据库表**：在 `DBTool/source_db/[Server]/` 中创建 Python 定义 → 运行 `genAll.py` → 使用生成的 `*BO/*BM` 类
**调试协议问题**：检查 `Common/src/` 中的生成类，验证 `.alpro` 语法，确保 `build_rpc.py` 成功运行
