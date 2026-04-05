# 用户服务器 (UserServer)

## 概述

UserServer 是整个游戏系统的核心，负责处理所有玩家相关的游戏逻辑。它采用先进的组件化架构和自动化消息处理机制，提供高性能、可扩展的游戏功能支持。

## 架构设计

### 总体架构
```
[客户端消息] → [消息分发器] → [消息处理器] → [业务组件] → [数据持久化]
                    ↓              ↓            ↓
              [自动注册]    [协议解析]    [组件管理器]
                    ↓              ↓            ↓
              [反射扫描]    [类型安全]    [生命周期]
```

### 核心目录结构
```
UserServer/src/NPUSServer/
├── NPUserMsgDispather/          # 消息处理系统
│   ├── p002_InitOp/            # 初始化操作
│   ├── p004_PlayerOp/          # 玩家操作
│   ├── p024_DungeonOp/         # 副本操作
│   ├── p036_TreasureHuntOp/    # 太空寻宝
│   └── Write/                  # 响应构造器
├── NPUSUserMgr/                # 用户管理
│   ├── UserComp/               # 用户组件
│   └── NPUSUserData.java       # 用户数据核心类
└── NPUserServer.java           # 服务器主类
```

## 五层消息处理架构

### 第一层：消息分发层
**核心类**：`NPUserMsgBasicDispatcher`

```java
// 自动注册机制
public void autoRegistHandler(String packageName) {
    // 通过反射扫描所有继承 NPUserMsgDealer 的类
    // 零配置自动注册，约定优于配置
}
```

**功能特性**：
- 通过反射自动扫描和注册消息处理器
- 支持热更新和动态加载
- 类型安全的协议绑定

### 第二层：消息封装层
**核心类**：`_ANPUSUserBasicMsgItem` 家族

```java
// 请求-响应模式（带客户端序列号）
public class NPUSUserRequestMsgItem extends _ANPUSUserBasicMsgItem {
    private long clientSerial;  // 客户端序列号，用于异步响应
}

// 单向消息或推送模式
public class NPUSUserNormalMsgItem extends _ANPUSUserBasicMsgItem {
    // 用于服务器主动推送消息到客户端
}
```

**责任**：
- 封装客户端消息和上下文信息
- 提供统一的响应接口 `_IWCGBasicRequestCommiter`
- 区分请求-响应和推送两种模式

### 第三层：消息处理层
**核心基类**：`NPUserMsgDealer<T>`

```java
public abstract class NPUserMsgDealer<GC2GS_036_001_ReqTreasureHuntOreCapture> 
    extends NPUserMsgDealer<GC2GS_036_001_ReqTreasureHuntOreCapture> {
    
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, 
                               GC2GS_036_001_ReqTreasureHuntOreCapture _msg) {
        // 泛型确保协议类型安全
        // 继承 _IAutoRegistMsgHandler 支持自动注册
    }
}
```

**命名规范**：`MsgDealer_[方向]_[主协议号]_[副协议号]_[功能描述]`

**示例**：
- `MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture`
- `MsgDealer_GC2GS_004_002_ReqPlayerLogin`

### 第四层：业务逻辑层
**组件系统调用**：

```java
// 获取玩家数据
NPUSUserData userData = _commiter.getUserData();

// 创建操作上下文
NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT);

// 调用业务组件
ResultOne<CaptureResult> result = userData.getTreasureHuntComponent()
    .capture(isAKey, isAdvance, areaId, context);
```

**关键特性**：
- `NPPlayerContext` 上下文贯穿整个处理流程
- 支持事件跟踪和审计日志
- 统一的错误处理和结果封装

### 第五层：响应构造层
**Writer工具类**（手动编写）：

```java
public class US2GCWriter_036_TreasureHuntOp {
    public static GS2GC_036_001_RetTreasureHuntOreCapture 
        make_001_RetTreasureHuntOreCapture(long exp, List<CaptureResult> results) {
        
        GS2GC_036_001_RetTreasureHuntOreCapture ret = 
            new GS2GC_036_001_RetTreasureHuntOreCapture();
        ret.setResult(CommErr.getSucc());
        ret.setExp(exp);
        ret.setCaptureResults(results);
        return ret;
    }
}
```

**命名规范**：`US2GCWriter_[协议包号]_[功能描述]`

## 组件系统架构

### 组件基类
```java
public abstract class _ANPUserComponent {
    private NPUSUserData _m_udUserData;
    private ENPPlayerCompType _m_eUserComponent;
    private boolean _m_bInited;
    
    // 构造函数中自动注册到组件管理器
    public _ANPUserComponent(NPUSUserData userData, ENPPlayerCompType compType) {
        userData.getComponentMgr().regComponent(this);
    }
}
```

### 组件生命周期
```
构造函数 → 自动注册 → 依赖检查 → 异步初始化 → 完成标记 → 全局回调 → 定时更新 → 资源清理
    ↓         ↓         ↓         ↓         ↓         ↓         ↓         ↓
 new()   regComponent() getDep()   _init()  setInited() onInited()  tick1Sec() dispose()
```

### 核心组件类型

#### 基础组件
- **CurrencyComponent**：货币系统（金币、钻石、经验等）
- **BagItemComponent**：背包物品管理
- **EquipComponent**：装备系统
- **PlayerShowComponent**：玩家展示信息

#### 游戏功能组件
- **HeroComponent**：英雄系统
- **QuestComponent**：任务系统
- **GuildComponent**：公会系统
- **ArenaComponent**：竞技场系统

#### 活动组件
- **DailyCheckComponent**：每日签到
- **TreasureHuntComponent**：太空寻宝
- **DinnerComponent**：宴会系统
- **TowerComponent**：爬塔系统

### 组件管理器
```java
public class NPUserComponentMgr {
    // 组件注册和生命周期管理
    public void regComponent(_ANPUserComponent component);
    
    // 依赖检查和初始化协调
    public boolean checkDependencies();
    
    // 全局事件分发
    public void dispatchGlobalEvent(GameEvent event);
}
```

## 用户数据管理

### NPUSUserData 核心类
这是用户服务器的核心数据类，聚合了所有玩家相关的组件和数据：

```java
public class NPUSUserData implements _IALProtocolReceiver, _IWCGMsgLocker {
    // 基础组件
    private CurrencyComponent _m_compCurrencyComponent;
    private BagItemComponent _m_compBagItemComponent;
    private EquipComponent _m_compEquipComponent;
    
    // 游戏功能组件
    private HeroComponent _m_compHeroComponent;
    private GuildComponent _m_compGuildComponent;
    private ArenaComponent _m_compArenaComponent;
    
    // 活动组件
    private DailyCheckComponent _m_compDailyCheck;
    private TreasureHuntComponent _m_compTreasureHuntComponent;
    
    // 组件访问方法
    public CurrencyComponent getCurrencyComponent() { return _m_compCurrencyComponent; }
    public BagItemComponent getBagItemComponent() { return _m_compBagItemComponent; }
    // ... 其他组件 getter 方法
}
```

### 线程安全机制
```java
// 玩家级别锁保护数据一致性
public void lockUser() {
    _m_mutexUser.lock();
}

public void unlockUser() {
    _m_mutexUser.unlock();
}
```

## 数据持久化

### BM (Business Manager) 模式
```java
// 数据对象
public class TreasureHuntMainBO extends _ABusinessObject {
    private long playerId;
    private int level;
    private long lastCaptureTime;
    // ... 数据字段
}

// 业务管理器
public class TreasureHuntMainBM {
    public static final TreasureHuntMainBM inst = new TreasureHuntMainBM();
    
    public boolean insertOrUpdateTreasureHuntMain(TreasureHuntMainBO bo, NPPlayerContext context) {
        // 数据库操作实现
    }
}
```

## 开发指南

### 添加新的玩家功能组件

#### 1. 创建组件类
```java
public class NewFeatureComponent extends _ANPUserComponent {
    public NewFeatureComponent(NPUSUserData userData) {
        super(userData, ENPPlayerCompType.NEW_FEATURE_COMP);
    }
    
    @Override
    protected void _init() {
        // 异步初始化逻辑
    }
    
    @Override
    public ENPPlayerCompType[] getDependCompList() {
        return new ENPPlayerCompType[]{ENPPlayerCompType.CURRENCY_COMP};
    }
}
```

#### 2. 注册组件类型
在 `ENPPlayerCompType` 枚举中添加：
```java
NEW_FEATURE_COMP,
```

#### 3. 在 NPUSUserData 中添加组件
```java
private NewFeatureComponent _m_compNewFeatureComponent;

public NewFeatureComponent getNewFeatureComponent() {
    return _m_compNewFeatureComponent;
}
```

### 添加新的消息处理器

#### 1. 定义协议（.alpro文件）
```alpro
ALProtocol GC2GS_050_001_ReqNewFeature[新功能请求] {
    int featureParam[功能参数];
}

ALProtocol GS2GC_050_001_RetNewFeature[新功能响应] {
    Common.CommonObj.RESULT result;
    int resultData[结果数据];
}
```

#### 2. 创建消息处理器
```java
public class MsgDealer_GC2GS_050_001_ReqNewFeature 
    extends NPUserMsgDealer<GC2GS_050_001_ReqNewFeature> {
    
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, 
                               GC2GS_050_001_ReqNewFeature _msg) {
        
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData) return;
        
        // 调用组件处理业务逻辑
        ResultOne<Integer> result = userData.getNewFeatureComponent()
            .processFeature(_msg.getFeatureParam());
            
        if (!result.isSucc()) {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        // 构造成功响应
        _commiter.commitSucRes(US2GCWriter_050_NewFeature
            .make_001_RetNewFeature(result.getData()));
    }
}
```

#### 3. 创建响应构造器
```java
public class US2GCWriter_050_NewFeature {
    public static GS2GC_050_001_RetNewFeature make_001_RetNewFeature(int resultData) {
        GS2GC_050_001_RetNewFeature ret = new GS2GC_050_001_RetNewFeature();
        ret.setResult(CommErr.getSucc());
        ret.setResultData(resultData);
        return ret;
    }
}
```

## 性能优化

### 组件初始化优化
- 使用 `ALProcess` 支持复杂的分步异步加载
- 合理设置组件依赖关系，避免循环依赖
- 实现 `_ITickableComponent` 接口支持定时更新

### 内存管理
- 及时释放不再使用的资源
- 在 `dispose()` 方法中清理监听器等资源
- 使用对象池减少 GC 压力

### 数据访问优化
- 合理使用缓存减少数据库访问
- 批量操作优化数据库性能
- 异步处理非关键路径操作

---

**相关文档**：
- [组件系统详解](10_component_system.md)
- [消息处理系统](11_message_system.md)
- [数据持久化系统](03_data_persistence.md)
