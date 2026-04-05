# AGENTS.md

本文件为 Claude Code (claude.ai/code) 在此代码库中工作时提供指导。

## 目录

- [1. 强制规范](#1-强制规范)
- [2. 常用命令](#2-常用命令)
- [3. 项目概览](#3-项目概览)
- [3.4 上下文检查](#34-上下文检查)
- [4. 通用基础](#4-通用基础)
- [4.4 .alpro协议定义语法](#44-alpro协议定义语法)
- [5. 玩家系统开发](#5-玩家系统开发)
- [6. 服务器间通信](#6-服务器间通信)
- [7. API常见陷阱](#7-api常见陷阱)
- [8. 部署运维](#8-部署运维)

---

## 1. 强制规范

### 1.0 Skill 优先原则（最高优先级）

**在执行任何开发任务前，必须先检查可用的 Skill 列表。有匹配的 Skill 时，禁止跳过 Skill 直接手动实现。**

**违规示例**：收到"创建协议"或"编辑协议"请求，不得先用 Explore 调研再手动写文件；涉及 `.alpro` 的创建/编辑必须先调用 `protocol`，再按需调用落地 skill（如 `create_player_protocol`）。

### 1.1 工作流程要求

1. **添加新功能前，必须先研究现有代码模式**
2. **逻辑开发必须进行上下文检查，确保实现符合项目框架与服务边界约束**
3. **数据库相关更改必须生成和更新BO类**
4. **协议更改必须重新生成协议类**
5. 先写逻辑代码后加import，避免无用import被IDE移除
6. 所有服务器都不是同进程，只能通过协议通信
7. 如改动到了代码，在最后验收时需要进行构建验证。
8. 加载数据成功情况不需要输出日志，使用 `CommLog.info` 记录正常日志

### 1.2 代码风格规范

#### 命名规范
- **方法命名**：使用驼峰命名法，英文命名
- **函数参数命名**：统一以下划线开头（示例：`_cid`、`_context`）
- **函数命名**：`protected` 与 `private` 函数名统一以下划线开头；除前缀外其余部分保持英文驼峰命名
- **类名前缀**：`abstract` 类以 `_A` 开头，模板类以 `_T` 开头，同时满足两者时以 `_AT` 开头
- **枚举命名**：定义具体枚举值时统一使用全大写命名，必要时使用下划线分隔（示例：`WAIT_RALLY`）
- **枚举默认位规则**：所有业务枚举首项固定为 `NONE`，实际业务枚举值从第二项开始定义

#### 代码格式
- **缩进规范**：统一使用 4 个空格缩进，不使用 2 个空格缩进
- **文件编码**：统一使用 UTF-8 编码（新增/修改文件保持一致）
- **终端写入规则（强制）**：凡是通过终端脚本写入/替换中文（PowerShell、批处理等），执行前必须切换终端为 UTF-8（`chcp 65001`，并设置 Input/OutputEncoding 为 UTF-8），否则禁止执行中文写入操作
- **import**：使用 Java import 语句，代码中不要使用完全限定类路径。遵循正在编辑的文件中的现有代码风格
- **包结构**：严格遵循现有包结构约定

#### 错误处理
- **必须使用 Result 和具体错误码类，不能返回字符串错误**
- 详见 [4.3 Result/ResultOne](#43-resultresultone)

#### 数据库操作
- **优先使用增量更新（ALMySqlUpdateValue），避免全表字段更新**

#### 协议与枚举
- 添加新的枚举、协议或定义时，始终先添加到 .alpro 文件中，不要直接添加到 Java 文件
- 配置/Ref 表（如 activity_rank_rush 表）不是数据库表。它们是从 txt 加载的配置表，对应 Ref* Java 类。不要将它们与 DB 表或 BO 类混淆
- 创建新的协议文件或包号之前，务必检查现有包号以避免冲突
- 使用 ByteBuffer 序列化时，始终创建正确的 struct/协议对象进行序列化。不要将原始数据直接写入 ByteBuffer

### 1.3 注释规范

- **所有注释必须使用中文**
- 在类、成员变量、函数以及关键逻辑判断处都必须有中文注释；在保证覆盖的前提下保持注释简洁
- 注释保持简洁明了。一行就够的时候不要写冗长的多行注释
- 重点说明**业务逻辑**而不仅仅是代码实现

**类级别注释**（使用 `/** */` 格式）：
```java
/**
 * 类名 - 简要描述
 *
 * 主要功能：
 * 1. 功能点一
 *
 * 线程安全：说明线程安全机制
 */
```

**成员变量注释**：`// 变量作用描述`，放在变量上一行

**方法注释**（使用 `/** */` 格式）：简单逻辑一句话概括，复杂逻辑说明步骤，含 `@param`/`@return`

### 1.4 日志规范

- **格式**：`类名.方法名 - 操作分类: 具体描述, 关键参数`
- **必须使用**：`USLog.error(serverObj, "message: {}", variable)` 格式，使用 `{}` 占位符
- 参数格式：`key=value`，多个参数用逗号分隔

```java
// ✅ 正确
USLog.error(getUSServer(), "Order not found: orderId={}, cid={}", orderId, getUserData().getCid());

// ❌ 错误
USLog.Err("message");  // 方法名不存在
CommLog.error("load table failed: {}", tableInfo.getTableName());  // 缺少类名.方法名前缀
```

### 1.5 设计原则

- 当值可以从现有数据派生时，优先使用简单的动态计算而不是持久化新字段。在添加新的持久化字段/存储之前务必先询问
- 实现验证+修改模式时，始终将验证与修改分离（先验证后修改）。不要在同一个循环中同时验证和修改数据
- 对于批量操作（如升级、扣除资源），优先预检查总成本并一次性扣除，而不是逐次迭代扣除
- 游戏服务器 GM 命令：保持实现简单和精简。日志/事件处理放在消息处理器中，不要放在组件中
- 频繁访问的配表对象应该缓存，避免重复查表
- 小数据量(<1000个)可使用List而非Map，减少内存开销
- 新功能不改变现有方法签名，确保向后兼容

---

## 2. 常用命令

```bash
# 主要构建（完成代码更改后必须运行）
cd build && build2.bat
# 注意：在 bash 环境下 build2.bat 可能挂起无输出，改用直接调用 ant：
cd build && apache-ant-1.9.4/bin/ant.bat build -buildfile build2.xml

# 错误码生成
cd bat && python build_err.py

# BO类生成
cd DBTool && python genAll.py

# 协议生成
cd ServerProtocol/ProtocolScripts && .\ALProtocolMaker.exe -a

# 终端切换 UTF-8（在执行任何中文写入命令前先执行）
chcp 65001
[Console]::InputEncoding = [System.Text.Encoding]::UTF8
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
```

---

## 3. 项目概览

### 3.1 技术栈
- **语言**：Java（服务端）+ C#（客户端）
- **架构**：微服务分布式架构
- **数据库**：MySQL + BM模式数据访问
- **构建**：Ant + Python脚本自动化
- **协议**：自定义二进制协议 + 自动代码生成

### 3.2 服务器架构

| 缩写 | 名称 | 说明 |
|------|------|------|
| `US` | UserServer | 核心游戏逻辑、玩家数据管理 |
| `GS` | GatewayServer | 客户端连接网关、负载均衡和请求路由 |
| `LS` | LoginServer | 用户认证和账号管理 |
| `CS` | CommonServer | 共享服务和服务器间通信协调 |
| `SS` | ScheduleServer | 活动调度 |
| `HS` | HttpServer | HTTP API 服务 |
| `CGS` | CrossGameServer | 跨服活动和功能 |
| `CRS` | CrossRankServer | 跨服排行榜系统 |

### 3.3 配置系统
- 基于 Properties 的配置在 `/conf/` 目录中，`/customConf/` 中的环境特定覆盖

### 3.4 上下文检查
- 处理玩家数据逻辑时，必须先判断是否涉及联盟（Guild）上下文。
- 如果涉及联盟数据处理，必须通过 RPC 将请求发送到联盟所在的 US，由目标 US 执行后续业务逻辑，禁止在当前 US 直接处理跨服联盟数据。

---

## 4. 通用基础

### 4.1 数据持久化

BM（Business Manager）模式用于数据库访问，通过 `EDBTag` 枚举系统进行数据库路由。

> 新建BO类请使用 `create_bo` skill

**BO对象更新模式**：

```java
// 单字段直接保存
_m_bo.saveExp(getBM(), newExp);

// 多字段批量保存（先set标记，再统一提交）
_m_bo.setExp(getBM(), newExp);
_m_bo.setStationLevel(getBM(), newLevel);
_m_bo.saveAllMarked(getBM());

// 增量更新（推荐，不依赖BO对象）
ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
updateValue.addValueObj("exp", newExp);
updateValue.addValueObj("station_level", newLevel);
getBM().getBM(XxxBO.class).update("id", _m_dbId, updateValue);
```

### 4.2 数据库标签与线程安全

```python
dbTag = "main"   # 主数据库
dbTag = "log"    # 日志数据库
```

```java
getUserData().lockUser();
try {
    // 业务操作
} finally {
    getUserData().unlockUser();
}
```

上下文：`NPPlayerContext.createNew(ENPGameEvent.XXX)` 用于事件跟踪和审计

### 4.3 Result/ResultOne

- **无数据返回**：使用 `Result`；**有数据返回**：使用 `ResultOne<T>`
- 错误码本身就是Result对象，**直接返回，无需 `Result.failed()` 包装**：

```java
// ✅ 正确
return CommErr.REF_NOT_FOUND;
return Result.SUCC;
return ResultOne.succ(record);

// ❌ 错误
return Result.failed(CommErr.REF_NOT_FOUND);  // 多余的包装
```

### 4.4 .alpro协议定义语法

文件路径：`ServerProtocol/ProtocolScripts/`，修改后需运行协议生成命令。

#### 文件基本结构

```
import Common.NPCommonObj.alpro;       // 导入其他alpro文件（可选）

JavaPackage Common.HeroObj;            // Java包路径（必须）
csharpspace Common.HeroObj;            // C#命名空间（服务端专用文件可省略）
```

#### 结构体定义（ALProtocol）

```
// 纯数据结构体（无包号）
ALProtocol 结构体名[中文注释]
{
    字段类型 字段名[中文注释];
}

// 网络协议（带包号和序号）
ALProtocol GC2GS_004_002_ReqGmCommand <4, 2>
{
    string command;
}
```

**支持的字段类型**：

| 类型 | 说明 |
|------|------|
| `int` / `long` / `short` | 整数 |
| `bool` | 布尔值 |
| `float` | 浮点数 |
| `string` / `String` | 字符串（等价） |
| `bytebuffer` | 原始字节缓冲区 |
| `类型[]` | 数组（如 `long[]`、`Hero_SkinInfo[]`） |
| `同文件结构体名` | 嵌套结构体（如 `Hero_HaloInfo haloInfo`） |
| `跨文件全路径` | 跨文件引用（如 `Common.NPCommonObj.NPCommon_ItemInfo item`） |
| `枚举全路径` | 枚举字段（如 `Enum.LevyEnum.ELevy_Type levyType`） |

**结构体示例**：

```
ALProtocol Levy_Silver[银币征收信息]
{
    long lastSettleMs[上次结算时间（毫秒）];
    long speed[每秒产出数量];
    Enum.LevyEnum.ELevy_Type levyType[征收类型];
    Common.NPCommonObj.NPCommon_ItemInfo[] itemList[物品列表];
    Hero_SkinInfo[] skinList[皮肤列表];
}
```

#### 枚举定义（ALEnum）

```
ALEnum 枚举名[中文注释]
{
    NONE;                    // 首项固定为NONE（业务枚举）
    SILVER[银币 -> 金币];     // 全大写，下划线分隔，[中文注释]
    WAIT_RALLY[等待集结];
}
```

**规则**：枚举值全大写、首项通常为 `NONE`、一个文件可定义多个枚举。

#### 注释格式

- 字段/枚举值内联注释：`字段名[中文描述]`（主要方式）
- 行注释：`//` 或 `///`
- 块注释：`/** */` 或 `/* */`

---

## 5. 玩家系统开发

添加玩家功能的完整流程：协议定义 → 协议生成 → MsgDealer → Writer → Component

> 相关 Skills：`protocol`、`create_player_protocol`、`create_player_msgdealer`、`create_protocol_writer`、`create_player_component`

### 5.1 消息处理框架

**五层架构**：
1. **消息分发层**：`NPUserMsgBasicDispatcher` — 反射自动扫描注册处理器
2. **消息封装层**：`_ANPUSUserBasicMsgItem` — 实现 `_IWCGBasicRequestCommiter` 接口
3. **消息处理层**：`NPUserMsgDealer<T>` 抽象基类，命名：`MsgDealer_[方向]_[包号]_[序号]_[功能]`
4. **业务逻辑层**：通过 `getUserData().getXXXComponent()` 访问业务组件，`NPPlayerContext` 贯穿全流程
5. **响应构造层**：Writer工具类，命名：`US2GCWriter_[包号]_[功能]`

**处理流程**：客户端消息 → MsgItem封装 → Dispatcher路由 → MsgDealer → Component → Writer → 客户端

### 5.2 组件系统

玩家功能通过组件架构实现，核心基于 `_ANPUserComponent` 基类。

**生命周期**：创建 → 依赖检查 → `_init()` 异步初始化（ALProcess）→ `setInited()` → `onInited()` → `tick1Sec()` → `dispose()`

**NPUSUserData结构**：组件注册分三个位置——成员变量声明(~300行)、构造函数初始化(~400行)、getter方法(~800行)

**关键注意**：
- 无依赖组件的 `getDependCompList()` 返回 `null`，不返回空数组
- 必须实现 `dispose()` 清理资源
- 在 `ENPPlayerCompType` 枚举中添加新类型（位于 `NPCommonEnum.java`）

### 5.3 组件数据对象模式

组件内部的数据对象遵循：缓存主键ID（不持有BO对象）、延迟创建、增量更新。

`create_player_component` skill 只覆盖组件类本身，不覆盖此部分。

```java
public class PlayerXxxRecord {
    private XxxComponent _m_comp;    // 持有组件引用，便于访问 BM 和 UserData
    private RefXxx _m_ref;           // 缓存配表对象，避免重复查表
    private long _m_dbId;            // 数据库ID，0表示未插入
    private int _m_buyCount;         // 缓存业务数据

    // 纯配置构造（新建记录）
    public PlayerXxxRecord(XxxComponent _comp, RefXxx _ref) {
        _m_comp = _comp;
        _m_ref = _ref;
        _m_dbId = 0;
    }

    // 从BO构造（数据加载）
    public PlayerXxxRecord(XxxComponent _comp, RefXxx _ref, PlayerXxxBO _bo) {
        this(_comp, _ref);
        _m_dbId = _bo.getId();
        _m_buyCount = _bo.getBuyCount();
    }

    public BM getBM() { return _m_comp.getUSServer().getBM(); }

    // 懒加载插入：返回true=已存在，false=新插入
    private boolean makeSureBoInsert() {
        if (_m_dbId != 0) return true;
        PlayerXxxBO bo = new PlayerXxxBO();
        bo.setCid(getBM(), _m_comp.getUserData().getCid());
        bo.setRefId(getBM(), _m_ref.Id());
        bo.setBuyCount(getBM(), _m_buyCount);
        bo.insert(getBM());
        _m_dbId = bo.getId();
        return false;
    }

    // 业务方法：懒插入 + 增量更新
    public void recordPurchase(int _count) {
        _m_buyCount += _count;
        if (makeSureBoInsert()) {
            ALMySqlUpdateValue uv = new ALMySqlUpdateValue();
            uv.addValueObj("buy_count", _m_buyCount);
            getBM().getBM(PlayerXxxBO.class).update("id", _m_dbId, uv);
        }
    }
}
```

### 5.4 协议包号

**已用包号（客户端）**：
- `p002_InitOp`: 初始化操作
- `p004_PlayerOp`: 玩家操作
- `p024_DungeonOp`: 副本操作
- `p033_SimpleActivityOp`: 简单活动
- `p036_TreasureHuntOp`: 太空寻宝

请求和响应使用相同的包号和序号，放在同一目录下。

### 5.5 错误处理机制

- **请求式消息**：`commitFailRes(errorCode)` → `sendBackRequestFailToGC(序列号, 错误码)`
- **普通消息**：`commitFailRes(errorCode)` → `make_051_OnCommError(错误码)` 通用错误协议
- **响应方式**：`commitSucRes(_IALProtocolStructure)` / `commitSucResByBuffer(ByteBuffer)` / `commitFailRes(int)`

### 5.6 联盟消息处理

> 使用 `deal_player_guild_msg` skill 获取完整模板

**强制规则**：
1. 统一通过 `NPUserServer.dealGuildMsg(...)` 或 `dealGuildMsgByRedirectCommiter(...)` 发送，禁止绕过
2. 必须先校验 `guildId > 0`，无效时本地失败返回，禁止继续转发
3. 仅透传结果用 `dealGuildMsg`；需二次处理用 `dealGuildMsgByRedirectCommiter`
4. 本地有状态变更时必须提供 `_failCallback` 回滚

---

## 6. 服务器间通信

- **服务器隔离**：服务器之间不能直接调用方法，必须通过协议通信
- **通信模式**：基于协议的请求-响应模式，采用异步回调机制
- **任意方向**：任意两个服务器之间都可以互相通信

**通信流程**：发送方构造协议 → `sendRequestToBSServer()` → 接收方 Dispatcher → Handler → `commitSucRes()` → 发送方 `dealSuc()/dealFail()`

> 新建服务器间协议请使用 `create_server_protocol` skill（含命名规范、文件位置、Writer、Handler注册）

---

## 7. API常见陷阱

### 7.1 USLog日志系统

```java
// ✅ 正确：传入server对象，使用{}占位符，小写方法名
USLog.error(getUSServer(), "Order not found: orderId={}, cid={}", orderId, cid);

// ❌ 错误
USLog.Err("message");    // 方法名不存在
USLog.Debug("message");  // 方法名不存在
```

日志级别：`USLog.error()`, `USLog.fatal()`, `USLog.sys()`

### 7.2 RefGeneral配置访问

```java
// ✅ 正确：直接访问字段，Ref()访问单例
long waitTime = RefGeneral.Ref().rush_exchange_done_need_wait_sec;

// ❌ 错误：不存在 GeneralMgr 类
String val = GeneralMgr.getInstance().getString("key");
```

### 7.3 ALProcess异步处理

```java
// ✅ 正确导入
import ALBasicServer.ALProcess.ALProcess;       // 不是 ALAsyncCommon.ALProcess
import NPCommon.Util.CallBack._ICallBackBool;   // 不是 NPCommon.DB.BM.CallBack._ICallBackBool
```

`_ICallBackBool` 回调使用 `onRunOver(boolean)`（不是 `dealAction(boolean)`）

### 7.4 数据库查询回调

```java
// ✅ 正确
new _ASelectCallback<List<XxxBO>>() {
    @Override public void dealSuc(List<XxxBO> _boList) { }
    @Override public void dealFail() { }
}
// 不是 _ASelectCallback<XxxBO>；不是 onSelectResult/onSelectFail
```

---

## 8. 部署运维

- 构建：`/build/build2.xml` — Ant 构建配置
- 脚本：`/bat/` — 代码生成和构建脚本
- 模板：`/build/template/` — 部署模板（含启动脚本、配置文件）
- 调试：GM 命令系统用于游戏内测试；`/HsClient/` 中的开发客户端

---

## 9. 编码恢复规则（新增）

- 发现中文注释乱码或 `BOM`（如 `﻿package`）时，必须先完成编码修复，再继续业务逻辑修改。
- 统一使用 `UTF-8 (no BOM)` 保存源码；禁止在乱码状态下继续叠加逻辑改动。
