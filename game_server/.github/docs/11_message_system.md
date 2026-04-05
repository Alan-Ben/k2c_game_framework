# 消息处理系统

## 概述

消息处理系统是 UserServer 的核心机制，负责处理来自客户端的所有游戏协议。系统采用五层架构设计，结合自动注册、类型安全和异步处理等特性，提供高性能、可扩展的消息处理能力。

## 系统架构

### 五层架构概览
```
第1层: 消息分发层    [NPUserMsgBasicDispatcher]
        ↓                    自动注册和路由
第2层: 消息封装层    [_ANPUSUserBasicMsgItem]
        ↓                    请求上下文封装
第3层: 消息处理层    [NPUserMsgDealer<T>]
        ↓                    业务逻辑处理
第4层: 业务逻辑层    [Component System]
        ↓                    具体功能实现
第5层: 响应构造层    [US2GCWriter_XXX]
        ↓                    响应数据构造
      [客户端响应]           网络发送
```

### 消息流向图
```
[客户端消息] → [网关转发] → [消息队列] → [分发器路由] → [处理器执行]
                                              ↓
[客户端接收] ← [网络发送] ← [响应提交] ← [Writer构造] ← [组件处理]
```

## 第一层：消息分发层

### NPUserMsgBasicDispatcher

**核心职责**：
- 自动扫描和注册消息处理器
- 根据协议号路由消息到对应处理器
- 提供类型安全的协议绑定机制

#### 自动注册机制
```java
public class NPUserMsgBasicDispatcher {
    
    // 自动注册处理器
    public void autoRegistHandler(String packageName) {
        try {
            // 1. 扫描指定包下的所有类
            Set<Class<?>> classes = scanPackageClasses(packageName);
            
            // 2. 筛选继承 NPUserMsgDealer 的类
            for (Class<?> clazz : classes) {
                if (isValidMsgDealer(clazz)) {
                    // 3. 通过反射获取泛型参数（协议类型）
                    Type protocolType = getProtocolType(clazz);
                    
                    // 4. 注册到处理器映射表
                    registerHandler(protocolType, clazz);
                }
            }
        } catch (Exception e) {
            // 注册失败处理
            handleRegistrationError(e);
        }
    }
    
    // 消息分发
    public void dispatchMessage(_ANPUSUserBasicMsgItem msgItem, ByteBuffer msgData) {
        // 1. 解析协议号
        int mainProtocol = msgData.get();
        int subProtocol = msgData.get();
        
        // 2. 查找对应的处理器
        NPUserMsgDealer<?> dealer = findHandler(mainProtocol, subProtocol);
        
        if (dealer != null) {
            // 3. 反序列化协议数据
            Object protocolObj = deserializeProtocol(msgData, dealer.getProtocolType());
            
            // 4. 调用处理器
            dealer.dealMessage(msgItem, protocolObj);
        } else {
            // 未找到处理器
            handleUnknownProtocol(mainProtocol, subProtocol);
        }
    }
}
```

#### 处理器注册表
```java
// 协议号 → 处理器映射
private Map<String, NPUserMsgDealer<?>> handlerMap = new ConcurrentHashMap<>();

// 协议类型 → 处理器映射
private Map<Class<?>, NPUserMsgDealer<?>> typeHandlerMap = new ConcurrentHashMap<>();
```

## 第二层：消息封装层

### _ANPUSUserBasicMsgItem 家族

#### 基类设计
```java
public abstract class _ANPUSUserBasicMsgItem implements _IWCGBasicRequestCommiter {
    private NPUSUserData userData;              // 用户数据引用
    private _AWCGBSReceiverListener listener;   // 网络监听器
    private long timestamp;                     // 消息时间戳
    
    // 统一响应接口
    public abstract void commitSucRes(_IALProtocolStructure response);
    public abstract void commitFailRes(int errorCode);
    public abstract void commitSucResByBuffer(ByteBuffer buffer);
}
```

#### 请求-响应模式
```java
public class NPUSUserRequestMsgItem extends _ANPUSUserBasicMsgItem {
    private long clientSerial;  // 客户端序列号
    
    @Override
    public void commitSucRes(_IALProtocolStructure response) {
        // 构造带序列号的响应
        ByteBuffer responseBuffer = buildResponseWithSerial(response, clientSerial);
        sendResponse(responseBuffer);
    }
    
    @Override
    public void commitFailRes(int errorCode) {
        // 发送错误响应到客户端
        sendBackRequestFailToGC(clientSerial, errorCode);
    }
}
```

#### 推送模式
```java
public class NPUSUserNormalMsgItem extends _ANPUSUserBasicMsgItem {
    
    @Override
    public void commitSucRes(_IALProtocolStructure response) {
        // 直接推送消息到客户端
        ByteBuffer responseBuffer = serializeProtocol(response);
        sendPushMessage(responseBuffer);
    }
    
    @Override
    public void commitFailRes(int errorCode) {
        // 发送通用错误协议
        GS2GC_051_001_OnCommError errorMsg = new GS2GC_051_001_OnCommError();
        errorMsg.setErrorCode(errorCode);
        commitSucRes(errorMsg);
    }
}
```

## 第三层：消息处理层

### NPUserMsgDealer<T> 抽象基类

```java
public abstract class NPUserMsgDealer<T extends _IALProtocolStructure> 
    implements _IAutoRegistMsgHandler {
    
    // 抽象方法，子类必须实现
    protected abstract void _dealMessage(_ANPUSUserBasicMsgItem commiter, T msg);
    
    // 框架调用入口
    public final void dealMessage(_ANPUSUserBasicMsgItem commiter, Object msg) {
        try {
            // 类型安全转换
            @SuppressWarnings("unchecked")
            T typedMsg = (T) msg;
            
            // 调用具体实现
            _dealMessage(commiter, typedMsg);
            
        } catch (ClassCastException e) {
            // 类型转换错误
            commiter.commitFailRes(CommErr.PROTOCOL_TYPE_ERROR.getCode());
        } catch (Exception e) {
            // 处理异常
            handleProcessingError(commiter, e);
        }
    }
    
    // 获取协议类型（通过泛型反射）
    public Class<T> getProtocolType() {
        return ReflectionUtil.getGenericParameter(this.getClass());
    }
}
```

### 处理器命名规范

**格式**：`MsgDealer_[方向]_[主协议号]_[副协议号]_[功能描述]`

**示例**：
- `MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture` - 太空寻宝捕捉矿石
- `MsgDealer_GC2GS_004_001_ReqPlayerLogin` - 玩家登录
- `MsgDealer_GC2GS_024_005_ReqDungeonEnter` - 进入副本

### 典型处理器实现

```java
public class MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture 
    extends NPUserMsgDealer<GC2GS_036_001_ReqTreasureHuntOreCapture> {
    
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, 
                               GC2GS_036_001_ReqTreasureHuntOreCapture _msg) {
        
        // 1. 获取用户数据
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData) {
            _commiter.commitFailRes(CommErr.USER_NOT_FOUND.getCode());
            return;
        }
        
        // 2. 参数验证
        if (_msg.getAreaId() <= 0) {
            _commiter.commitFailRes(TreasureHuntErr.INVALID_AREA.getCode());
            return;
        }
        
        // 3. 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_CAPTURE);
        context.setEventParam("areaId", _msg.getAreaId());
        context.setEventParam("isAKey", _msg.getIsAKey());
        
        // 4. 调用业务组件
        TreasureHuntComponent treasureHuntComp = userData.getTreasureHuntComponent();
        ResultOne<TreasureHuntCaptureResult> result = treasureHuntComp.capture(
            _msg.getIsAKey(), _msg.getIsAdvance(), _msg.getAreaId(), context);
        
        // 5. 处理结果
        if (!result.isSucc()) {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        // 6. 构造并提交成功响应
        GS2GC_036_001_RetTreasureHuntOreCapture response = 
            US2GCWriter_036_TreasureHuntOp.make_001_RetTreasureHuntOreCapture(
                result.getData().getExp(), result.getData().getCaptureResults());
        
        _commiter.commitSucRes(response);
        
        // 7. 记录操作日志
        logPlayerOperation(userData, context, result.getData());
    }
}
```

## 第四层：业务逻辑层

### 组件系统集成

#### 上下文传递
```java
// NPPlayerContext 贯穿整个处理流程
NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_CAPTURE);

// 设置事件参数
context.setEventParam("areaId", areaId);
context.setEventParam("captureType", isAdvance ? "advance" : "normal");

// 设置操作来源
context.setSourceType(ENPOperationSource.CLIENT_REQUEST);
context.setSourceData("protocolName", "GC2GS_036_001_ReqTreasureHuntOreCapture");
```

#### 组件调用模式
```java
// 获取相关组件
TreasureHuntComponent treasureHuntComp = userData.getTreasureHuntComponent();
CurrencyComponent currencyComp = userData.getCurrencyComponent();
BagItemComponent bagItemComp = userData.getBagItemComponent();

// 业务逻辑处理
ResultOne<CaptureResult> captureResult = treasureHuntComp.capture(params, context);

if (captureResult.isSucc() && captureResult.getData().hasReward()) {
    // 发放奖励
    ResultOne<Boolean> rewardResult = currencyComp.addCurrency(
        ENPPlayerValueType.GOLD, captureResult.getData().getGoldReward(), context);
        
    if (!rewardResult.isSucc()) {
        // 回滚操作
        treasureHuntComp.rollbackCapture(params, context);
        return ResultOne.createFail(rewardResult.getCode());
    }
}
```

### 错误处理机制

#### 错误分类
```java
public enum ErrorCategory {
    PARAMETER_ERROR,    // 参数错误
    PERMISSION_ERROR,   // 权限错误
    RESOURCE_ERROR,     // 资源不足
    STATE_ERROR,        // 状态错误
    SYSTEM_ERROR        // 系统错误
}
```

#### 统一错误响应
```java
private void handleError(_ANPUSUserBasicMsgItem commiter, int errorCode, String errorMsg) {
    // 记录错误日志
    USLog.error("Message processing error: code={}, msg={}", errorCode, errorMsg);
    
    // 根据消息类型选择错误响应方式
    if (commiter instanceof NPUSUserRequestMsgItem) {
        // 请求式消息：返回错误码
        commiter.commitFailRes(errorCode);
    } else {
        // 普通消息：发送通用错误协议
        GS2GC_051_001_OnCommError errorProtocol = new GS2GC_051_001_OnCommError();
        errorProtocol.setErrorCode(errorCode);
        errorProtocol.setErrorMsg(errorMsg);
        commiter.commitSucRes(errorProtocol);
    }
}
```

## 第五层：响应构造层

### US2GCWriter 工具类

#### 命名规范
**格式**：`US2GCWriter_[协议包号]_[功能描述]`

**示例**：
- `US2GCWriter_036_TreasureHuntOp` - 太空寻宝操作
- `US2GCWriter_004_PlayerOp` - 玩家操作
- `US2GCWriter_024_DungeonOp` - 副本操作

#### 典型Writer实现
```java
public class US2GCWriter_036_TreasureHuntOp {
    
    // 捕捉矿石响应
    public static GS2GC_036_001_RetTreasureHuntOreCapture 
        make_001_RetTreasureHuntOreCapture(long exp, List<TreasureHunt_CaptureResult> captureResults) {
        
        GS2GC_036_001_RetTreasureHuntOreCapture ret = new GS2GC_036_001_RetTreasureHuntOreCapture();
        ret.setResult(CommErr.getSucc());
        ret.setExp(exp);
        ret.setCaptureResults(captureResults);
        return ret;
    }
    
    // 区域信息推送
    public static GS2GC_036_002_PushTreasureHuntAreaInfo 
        make_002_PushTreasureHuntAreaInfo(List<TreasureHunt_AreaInfo> areaInfos) {
        
        GS2GC_036_002_PushTreasureHuntAreaInfo push = new GS2GC_036_002_PushTreasureHuntAreaInfo();
        push.setAreaInfos(areaInfos);
        push.setUpdateTime(System.currentTimeMillis());
        return push;
    }
    
    // 错误响应构造
    public static GS2GC_036_001_RetTreasureHuntOreCapture 
        makeErrorResponse(int errorCode) {
        
        GS2GC_036_001_RetTreasureHuntOreCapture ret = new GS2GC_036_001_RetTreasureHuntOreCapture();
        ret.setResult(createErrorResult(errorCode));
        return ret;
    }
}
```

### 响应数据构造最佳实践

#### 1. 数据完整性检查
```java
public static GS2GC_036_001_RetTreasureHuntOreCapture 
    make_001_RetTreasureHuntOreCapture(long exp, List<TreasureHunt_CaptureResult> captureResults) {
    
    // 参数验证
    if (exp < 0) {
        throw new IllegalArgumentException("exp cannot be negative");
    }
    if (captureResults == null) {
        captureResults = new ArrayList<>();
    }
    
    GS2GC_036_001_RetTreasureHuntOreCapture ret = new GS2GC_036_001_RetTreasureHuntOreCapture();
    ret.setResult(CommErr.getSucc());
    ret.setExp(exp);
    ret.setCaptureResults(captureResults);
    
    return ret;
}
```

#### 2. 数据转换和格式化
```java
private static List<NPCommon_ItemInfo> convertToItemInfoList(List<RewardItem> rewards) {
    List<NPCommon_ItemInfo> itemInfos = new ArrayList<>();
    
    for (RewardItem reward : rewards) {
        NPCommon_ItemInfo itemInfo = new NPCommon_ItemInfo();
        itemInfo.setItemId(reward.getItemId());
        itemInfo.setCount(reward.getCount());
        itemInfo.setExpireTime(reward.getExpireTime());
        itemInfos.add(itemInfo);
    }
    
    return itemInfos;
}
```

## 性能优化

### 1. 消息处理池化
```java
// 使用对象池减少对象创建开销
private static final ObjectPool<NPPlayerContext> contextPool = 
    new ConcurrentObjectPool<>(NPPlayerContext::new);

public NPPlayerContext createContext(ENPGameEvent event) {
    NPPlayerContext context = contextPool.borrowObject();
    context.reset();
    context.setEvent(event);
    return context;
}

public void releaseContext(NPPlayerContext context) {
    contextPool.returnObject(context);
}
```

### 2. 异步处理
```java
// 对于耗时操作，使用异步处理
@Override
protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, 
                           GC2GS_036_001_ReqTreasureHuntOreCapture _msg) {
    
    // 快速响应客户端
    _commiter.commitSucRes(makeQuickResponse());
    
    // 异步执行耗时逻辑
    CompletableFuture.runAsync(() -> {
        processHeavyLogic(_msg);
    }).exceptionally(throwable -> {
        handleAsyncError(throwable);
        return null;
    });
}
```

### 3. 缓存优化
```java
// 缓存频繁访问的数据
private static final LoadingCache<String, ProtocolHandler> handlerCache = 
    Caffeine.newBuilder()
        .maximumSize(1000)
        .expireAfterWrite(Duration.ofMinutes(30))
        .build(key -> loadHandler(key));
```

## 开发指南

### 添加新的消息处理器

#### 1. 定义协议（.alpro文件）
```alpro
ALProtocol GC2GS_050_001_ReqNewFeature[新功能请求] {
    int param1[参数1];
    string param2[参数2];
}

ALProtocol GS2GC_050_001_RetNewFeature[新功能响应] {
    Common.CommonObj.RESULT result;
    NewFeature_ResultData resultData[结果数据];
}
```

#### 2. 生成协议类
```bash
cd bat
python build_rpc.py
```

#### 3. 创建消息处理器
```java
public class MsgDealer_GC2GS_050_001_ReqNewFeature 
    extends NPUserMsgDealer<GC2GS_050_001_ReqNewFeature> {
    // 实现处理逻辑
}
```

#### 4. 创建响应构造器
```java
public class US2GCWriter_050_NewFeature {
    public static GS2GC_050_001_RetNewFeature make_001_RetNewFeature(...) {
        // 实现响应构造
    }
}
```

### 调试和测试

#### 1. 消息处理跟踪
```java
// 启用详细日志
@Override
protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, T _msg) {
    long startTime = System.currentTimeMillis();
    
    try {
        USLog.debug("Processing message: {}, user: {}", 
            _msg.getClass().getSimpleName(), _commiter.getUserData().getCid());
        
        // 处理逻辑
        doProcessMessage(_commiter, _msg);
        
    } finally {
        long endTime = System.currentTimeMillis();
        USLog.debug("Message processed in {}ms", endTime - startTime);
    }
}
```

#### 2. 单元测试
```java
@Test
public void testTreasureHuntCapture() {
    // 创建测试用户数据
    NPUSUserData testUserData = createTestUserData();
    
    // 创建测试消息
    GC2GS_036_001_ReqTreasureHuntOreCapture testMsg = 
        new GC2GS_036_001_ReqTreasureHuntOreCapture();
    testMsg.setAreaId(1001);
    testMsg.setIsAKey(false);
    testMsg.setIsAdvance(true);
    
    // 创建处理器
    MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture dealer = 
        new MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture();
    
    // 执行测试
    MockCommiter commiter = new MockCommiter(testUserData);
    dealer._dealMessage(commiter, testMsg);
    
    // 验证结果
    assertTrue(commiter.isSuccess());
    assertNotNull(commiter.getResponse());
}
```

---

**相关文档**：
- [用户服务器详解](04_user_server.md)
- [组件系统详解](10_component_system.md)
- [协议生成系统](12_protocol_generation.md)
