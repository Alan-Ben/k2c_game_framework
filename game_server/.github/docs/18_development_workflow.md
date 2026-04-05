# 新功能开发流程

## 概述

本文档详细介绍在 GOB 游戏服务器中开发新功能的完整流程，从需求分析到功能上线的每个步骤。遵循此流程可以确保开发质量和系统稳定性。

## 开发流程总览

```
需求分析 → 技术设计 → 协议定义 → 代码生成 → 组件开发 → 消息处理 → 数据持久化 → 测试验证 → 部署上线
    ↓         ↓         ↓         ↓         ↓         ↓         ↓         ↓         ↓
功能规格   架构设计   .alpro    Python    Component  MsgDealer  BO/BM     单元测试   热更新
```

## 阶段一：需求分析和设计

### 1. 功能需求分析

#### 需求文档模板
```markdown
# 功能需求：太空寻宝系统

## 功能概述
- 玩家可以在不同区域进行寻宝活动
- 支持普通寻宝和高级寻宝两种模式
- 提供一键寻宝功能提升用户体验

## 功能规格
- 区域系统：多个寻宝区域，不同稀有度
- 消耗系统：消耗特定道具或货币
- 奖励系统：获得各种奖励物品
- 冷却系统：限制寻宝频率

## 接口需求
- 获取区域信息接口
- 执行寻宝操作接口
- 查询寻宝历史接口
```

### 2. 技术架构设计

#### 组件职责划分
```java
// 主要组件
TreasureHuntComponent {
    - 寻宝逻辑处理
    - 区域管理
    - 冷却时间控制
}

// 依赖组件
CurrencyComponent {
    - 货币消耗和奖励
}

BagItemComponent {
    - 道具消耗和奖励
}

// 数据持久化
TreasureHuntMainBO {
    - 玩家寻宝主数据
}

TreasureHuntRecordBO {
    - 寻宝历史记录
}
```

#### 协议设计规划
```
主协议号：036 (TreasureHuntOp)
子协议号分配：
001 - 矿石捕捉请求/响应
002 - 区域信息推送
003 - 寻宝历史查询
004 - 冷却时间查询
```

## 阶段二：协议定义和代码生成

### 1. 创建协议定义文件

#### 文件位置
```
ServerProtocol/ProtocolScripts/ALLRPC/US/p036_TreasureHuntOp.alpro
```

#### 协议定义内容
```alpro
JavaPackage GC2GS.p036_TreasureHuntOp;
JavaPackage GS2GC.p036_TreasureHuntOp;

// 数据结构定义
ALClass TreasureHunt_AreaInfo[寻宝区域信息] {
    long areaId[区域ID];
    int areaType[区域类型];
    string areaName[区域名称];
    boolean isUnlocked[是否解锁];
    long cooldownEndTime[冷却结束时间];
}

ALClass TreasureHunt_CaptureResult[捕捉结果] {
    int itemId[物品ID];
    long count[数量];
    int quality[品质];
}

ALClass TreasureHunt_Record[寻宝记录] {
    long recordId[记录ID];
    long areaId[区域ID];
    long captureTime[捕捉时间];
    TreasureHunt_CaptureResult[] results[捕捉结果];
}

// 请求协议
ALProtocol GC2GS_036_001_ReqTreasureHuntOreCapture[太空寻宝-捕捉矿石] {
    boolean isAKey[是否一键];
    boolean isAdvance[是否高级];
    long areaId[区域ID];
}

ALProtocol GC2GS_036_002_ReqTreasureHuntAreaInfo[获取区域信息] {
    // 无参数，获取所有区域信息
}

ALProtocol GC2GS_036_003_ReqTreasureHuntHistory[查询寻宝历史] {
    int pageIndex[页码];
    int pageSize[每页大小];
}

// 响应协议
ALProtocol GS2GC_036_001_RetTreasureHuntOreCapture[返回捕捉结果] {
    Common.CommonObj.RESULT result;
    long exp[获得经验];
    TreasureHunt_CaptureResult[] captureResults[捕捉结果列表];
    long nextCooldownTime[下次冷却时间];
}

ALProtocol GS2GC_036_002_RetTreasureHuntAreaInfo[返回区域信息] {
    Common.CommonObj.RESULT result;
    TreasureHunt_AreaInfo[] areaInfos[区域信息列表];
}

ALProtocol GS2GC_036_003_RetTreasureHuntHistory[返回寻宝历史] {
    Common.CommonObj.RESULT result;
    TreasureHunt_Record[] records[历史记录];
    int totalCount[总记录数];
}

// 推送协议
ALProtocol GS2GC_036_004_PushAreaStatusUpdate[推送区域状态更新] {
    TreasureHunt_AreaInfo areaInfo[更新的区域信息];
}
```

### 2. 错误码定义

#### 创建错误码文件
```
bat/err/036_TreasureHuntErr_寻宝错误.txt
```

```txt
java_path= ../Common/src
java_package=NPCommon.ErrMain
csharp_type = main
id	name	desc
1	AREA_NOT_FOUND	区域不存在
2	AREA_NOT_UNLOCKED	区域未解锁
3	IN_COOLDOWN	处于冷却中
4	INSUFFICIENT_RESOURCE	资源不足
5	INVALID_CAPTURE_TYPE	无效的捕捉类型
6	SYSTEM_MAINTENANCE	系统维护中
```

### 3. 执行代码生成

```bash
# 完整代码生成流程
cd bat

# 1. 生成枚举（如果有新枚举）
python build_enum.py

# 2. 生成错误码
python build_err.py

# 3. 生成协议类
python build_rpc.py

# 4. 生成事件（如果需要）
python build_events.py
```

## 阶段三：数据库设计和生成

### 1. 数据表设计

#### 主数据表
```python
# DBTool/source_db/UserServer/player_treasure_hunt.py
tableComment = "玩家太空寻宝数据"
field = [
    Field("cid", "long", True, "玩家ID"),
    Field("current_area_id", "long", False, "当前选中区域ID"),
    Field("total_capture_count", "int", False, "总捕捉次数"),
    Field("last_capture_time", "long", False, "最后捕捉时间"),
    Field("daily_capture_count", "int", False, "今日捕捉次数"),
    Field("unlock_areas", "string", False, "已解锁区域列表JSON"),
    Field("create_time", "long", False, "创建时间"),
    Field("update_time", "long", False, "更新时间"),
]
```

#### 记录表
```python
# DBTool/source_db/UserServer/player_treasure_hunt_record.py
tableComment = "玩家太空寻宝记录"
field = [
    Field("record_id", "long", True, "记录ID"),
    Field("cid", "long", False, "玩家ID"),
    Field("area_id", "long", False, "区域ID"),
    Field("capture_type", "int", False, "捕捉类型"),
    Field("capture_time", "long", False, "捕捉时间"),
    Field("results_json", "string", False, "捕捉结果JSON"),
    Field("cost_items_json", "string", False, "消耗道具JSON"),
]
```

### 2. 生成数据库代码

```bash
cd DBTool
python genAll.py
```

生成的文件：
- `UserServer/src/USDB/TreasureHunt/TreasureHuntMainBO.java`
- `UserServer/src/USDB/TreasureHunt/TreasureHuntMainBM.java`
- `UserServer/src/USDB/TreasureHunt/TreasureHuntRecordBO.java`
- `UserServer/src/USDB/TreasureHunt/TreasureHuntRecordBM.java`

## 阶段四：组件开发

### 1. 创建组件类

```java
// UserServer/src/NPUSServer/NPUSUserMgr/UserComp/TreasureHuntComp/TreasureHuntComponent.java
package NPUSServer.NPUSUserMgr.UserComp.TreasureHuntComp;

public class TreasureHuntComponent extends _ANPUserComponent implements _ITickableComponent {
    
    private TreasureHuntMainBO mainData;
    private Map<Long, TreasureHunt_AreaInfo> areaInfoMap;
    private long lastTickTime;
    
    public TreasureHuntComponent(NPUSUserData userData) {
        super(userData, ENPPlayerCompType.TREASURE_HUNT_COMP);
    }
    
    @Override
    public ENPPlayerCompType[] getDependCompList() {
        return new ENPPlayerCompType[]{
            ENPPlayerCompType.CURRENCY_COMP,
            ENPPlayerCompType.BAG_ITEM_COMP
        };
    }
    
    @Override
    protected void _init() {
        final ALProcess process = ALProcess.CreateProcess("treasure_hunt_init");
        
        // 加载玩家寻宝数据
        process.AddHandle(new _IProcessHandle() {
            @Override
            public void doProcess(_IALProcessAsynCommiter commiter) {
                loadPlayerTreasureHuntData(commiter);
            }
        });
        
        // 初始化区域信息
        process.AddHandle(new _IProcessHandle() {
            @Override
            public void doProcess(_IALProcessAsynCommiter commiter) {
                initAreaInfos();
                commiter.commitSuc();
            }
        });
        
        process.startALProcess(new _IALProcessFinishListener() {
            @Override
            public void onFinish(boolean isSucc) {
                if (isSucc) {
                    setInited();
                }
            }
        });
    }
    
    @Override
    public void tick1Sec() {
        // 每秒更新冷却状态
        updateCooldownStatus();
    }
    
    // 主要业务方法
    public ResultOne<TreasureHuntCaptureResult> capture(boolean isAKey, boolean isAdvance, 
                                                       long areaId, NPPlayerContext context) {
        try {
            // 参数验证
            ResultOne<Boolean> validateResult = validateCaptureRequest(isAKey, isAdvance, areaId);
            if (!validateResult.isSucc()) {
                return ResultOne.createFail(validateResult.getCode());
            }
            
            // 检查冷却时间
            if (isInCooldown(areaId)) {
                return ResultOne.createFail(TreasureHuntErr.IN_COOLDOWN.getCode());
            }
            
            // 检查和消耗资源
            ResultOne<Boolean> costResult = processCost(isAKey, isAdvance, context);
            if (!costResult.isSucc()) {
                return ResultOne.createFail(costResult.getCode());
            }
            
            // 执行寻宝逻辑
            TreasureHuntCaptureResult result = performCapture(isAKey, isAdvance, areaId, context);
            
            // 发放奖励
            giveRewards(result.getRewards(), context);
            
            // 更新数据
            updateCaptureData(areaId, result);
            
            // 保存到数据库
            saveToDB(context);
            
            return ResultOne.createSuc(result);
            
        } catch (Exception e) {
            USLog.error("Treasure hunt capture error", e);
            return ResultOne.createFail(CommErr.SYS_ERR.getCode());
        }
    }
    
    // 其他业务方法...
}
```

### 2. 注册组件到枚举

```java
// NPCommon/Enum/NPCommonEnum.java
public enum ENPPlayerCompType {
    // ... 其他组件
    TREASURE_HUNT_COMP,
}
```

### 3. 添加到用户数据类

```java
// NPUSUserData.java
private TreasureHuntComponent _m_compTreasureHuntComponent;

public TreasureHuntComponent getTreasureHuntComponent() {
    return _m_compTreasureHuntComponent;
}
```

## 阶段五：消息处理器开发

### 1. 创建消息处理器

```java
// UserServer/src/NPUSServer/NPUserMsgDispather/p036_TreasureHuntOp/MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture.java
package NPUSServer.NPUserMsgDispather.p036_TreasureHuntOp;

public class MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture 
    extends NPUserMsgDealer<GC2GS_036_001_ReqTreasureHuntOreCapture> {
    
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, 
                               GC2GS_036_001_ReqTreasureHuntOreCapture _msg) {
        
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData) {
            _commiter.commitFailRes(CommErr.USER_NOT_FOUND.getCode());
            return;
        }
        
        // 参数验证
        if (_msg.getAreaId() <= 0) {
            _commiter.commitFailRes(TreasureHuntErr.AREA_NOT_FOUND.getCode());
            return;
        }
        
        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_CAPTURE);
        context.setEventParam("areaId", _msg.getAreaId());
        context.setEventParam("isAKey", _msg.getIsAKey());
        context.setEventParam("isAdvance", _msg.getIsAdvance());
        
        // 调用组件处理
        TreasureHuntComponent treasureHuntComp = userData.getTreasureHuntComponent();
        ResultOne<TreasureHuntCaptureResult> result = treasureHuntComp.capture(
            _msg.getIsAKey(), _msg.getIsAdvance(), _msg.getAreaId(), context);
        
        if (!result.isSucc()) {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        // 构造成功响应
        _commiter.commitSucRes(US2GCWriter_036_TreasureHuntOp
            .make_001_RetTreasureHuntOreCapture(
                result.getData().getExp(), 
                result.getData().getCaptureResults(),
                result.getData().getNextCooldownTime()));
    }
}
```

### 2. 创建其他消息处理器

类似地创建：
- `MsgDealer_GC2GS_036_002_ReqTreasureHuntAreaInfo`
- `MsgDealer_GC2GS_036_003_ReqTreasureHuntHistory`

### 3. 创建响应构造器

```java
// UserServer/src/NPUSServer/NPUserMsgDispather/Write/US2GCWriter_036_TreasureHuntOp.java
package NPUSServer.NPUserMsgDispather.Write;

public class US2GCWriter_036_TreasureHuntOp {
    
    public static GS2GC_036_001_RetTreasureHuntOreCapture 
        make_001_RetTreasureHuntOreCapture(long exp, List<TreasureHunt_CaptureResult> captureResults, 
                                          long nextCooldownTime) {
        
        GS2GC_036_001_RetTreasureHuntOreCapture ret = new GS2GC_036_001_RetTreasureHuntOreCapture();
        ret.setResult(CommErr.getSucc());
        ret.setExp(exp);
        ret.setCaptureResults(captureResults);
        ret.setNextCooldownTime(nextCooldownTime);
        return ret;
    }
    
    public static GS2GC_036_002_RetTreasureHuntAreaInfo 
        make_002_RetTreasureHuntAreaInfo(List<TreasureHunt_AreaInfo> areaInfos) {
        
        GS2GC_036_002_RetTreasureHuntAreaInfo ret = new GS2GC_036_002_RetTreasureHuntAreaInfo();
        ret.setResult(CommErr.getSucc());
        ret.setAreaInfos(areaInfos);
        return ret;
    }
    
    public static GS2GC_036_004_PushAreaStatusUpdate 
        make_004_PushAreaStatusUpdate(TreasureHunt_AreaInfo areaInfo) {
        
        GS2GC_036_004_PushAreaStatusUpdate push = new GS2GC_036_004_PushAreaStatusUpdate();
        push.setAreaInfo(areaInfo);
        return push;
    }
}
```

## 阶段六：配置和数据准备

### 1. 添加配置表

```java
// GameRes/src/NPGameRes/Refs/TreasureHunt/RefTreasureHuntArea.java
public class RefTreasureHuntArea extends _ARefDataBase {
    public long areaId;           // 区域ID
    public String areaName;       // 区域名称
    public int unlockLevel;       // 解锁等级
    public int normalCost;        // 普通寻宝消耗
    public int advanceCost;       // 高级寻宝消耗
    public int cooldownTime;      // 冷却时间（秒）
    public String rewardGroups;   // 奖励组JSON
}
```

### 2. 准备配置数据

创建Excel配置表并转换为游戏配置文件。

## 阶段七：构建和测试

### 1. 执行构建

```bash
# 完整构建流程
cd bat && python build_enum.py && python build_err.py && python build_rpc.py
cd ../DBTool && python genAll.py  
cd ../build && build2.bat
```

### 2. 单元测试

```java
@Test
public void testTreasureHuntCapture() {
    // 准备测试数据
    NPUSUserData testUser = createTestUser();
    setupTreasureHuntData(testUser);
    
    // 创建组件
    TreasureHuntComponent component = testUser.getTreasureHuntComponent();
    
    // 准备测试参数
    NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TREASURE_HUNT_CAPTURE);
    
    // 执行测试
    ResultOne<TreasureHuntCaptureResult> result = component.capture(false, true, 1001L, context);
    
    // 验证结果
    assertTrue(result.isSucc());
    assertNotNull(result.getData());
    assertTrue(result.getData().getCaptureResults().size() > 0);
}
```

### 3. 集成测试

```java
@Test
public void testTreasureHuntProtocol() {
    // 模拟客户端请求
    GC2GS_036_001_ReqTreasureHuntOreCapture request = 
        new GC2GS_036_001_ReqTreasureHuntOreCapture();
    request.setAreaId(1001L);
    request.setIsAKey(false);
    request.setIsAdvance(true);
    
    // 创建处理器
    MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture dealer = 
        new MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture();
    
    // 模拟请求处理
    MockCommiter commiter = new MockCommiter(testUser);
    dealer._dealMessage(commiter, request);
    
    // 验证响应
    assertTrue(commiter.isSuccess());
    GS2GC_036_001_RetTreasureHuntOreCapture response = 
        (GS2GC_036_001_RetTreasureHuntOreCapture) commiter.getResponse();
    assertNotNull(response);
    assertTrue(response.getResult().isSucc());
}
```

## 阶段八：部署和监控

### 1. 热更新部署

```bash
# 如果是热更新功能，放在ActivitiesV01中开发
cd bat
cp_hotifx.bat
cd ../build
build2.bat
```

### 2. 服务器部署

```bash
# 停止服务器
./stop_server.sh

# 备份现有版本
cp -r server_current server_backup_$(date +%Y%m%d_%H%M%S)

# 部署新版本
cp ../Lib/*.jar server_current/lib/
cp config/* server_current/conf/

# 启动服务器
./start_server.sh
```

### 3. 监控和日志

```java
// 添加业务监控
public ResultOne<TreasureHuntCaptureResult> capture(...) {
    long startTime = System.currentTimeMillis();
    
    try {
        // 业务逻辑
        TreasureHuntCaptureResult result = doCapture(...);
        
        // 成功监控
        MonitorMgr.recordSuccess("treasure_hunt_capture", System.currentTimeMillis() - startTime);
        USLog.info("Treasure hunt capture success: user={}, area={}, rewards={}", 
            getUserData().getCid(), areaId, result.getCaptureResults().size());
        
        return ResultOne.createSuc(result);
        
    } catch (Exception e) {
        // 失败监控
        MonitorMgr.recordError("treasure_hunt_capture", e);
        USLog.error("Treasure hunt capture failed: user={}, area={}", 
            getUserData().getCid(), areaId, e);
        
        return ResultOne.createFail(CommErr.SYS_ERR.getCode());
    }
}
```

## 质量保证检查清单

### 代码质量
- [ ] 所有public方法都有JavaDoc注释
- [ ] 异常处理完整且合理
- [ ] 没有硬编码的魔法数字
- [ ] 遵循项目命名规范
- [ ] 代码review通过

### 功能完整性
- [ ] 所有协议接口都已实现
- [ ] 错误场景处理完整
- [ ] 数据持久化正确
- [ ] 组件依赖关系明确
- [ ] 资源消耗和奖励发放正确

### 性能和安全
- [ ] 无性能瓶颈和内存泄露
- [ ] 输入参数验证完整
- [ ] 并发安全考虑
- [ ] 数据库操作优化
- [ ] 日志级别合理

### 测试覆盖
- [ ] 单元测试覆盖率 > 80%
- [ ] 集成测试通过
- [ ] 压力测试通过
- [ ] 异常场景测试通过

## 常见问题和解决方案

### 1. 协议生成失败
**原因**：.alpro文件语法错误
**解决**：检查协议定义语法，确保字段类型正确

### 2. 组件初始化失败
**原因**：依赖组件未初始化完成
**解决**：检查依赖关系，确保在onInited()中访问依赖组件

### 3. 数据库操作异常
**原因**：BO类字段与数据库表结构不匹配
**解决**：重新生成数据库代码，检查表结构

### 4. 消息处理器未生效
**原因**：处理器未被自动注册
**解决**：检查类名是否符合规范，确保继承了正确的基类

---

**相关文档**：
- [构建和部署系统](02_build_system.md)
- [组件系统详解](10_component_system.md)
- [消息处理系统](11_message_system.md)
- [协议生成系统](12_protocol_generation.md)
