# RushExchangeComponent - 限时兑换组件

## 概述

限时兑换系统是一个礼包轮换兑换系统，玩家可以消耗指定道具后等待一定时间获得其他道具。

## 核心特性

- **每日限次**：每日仅可发起兑换一次（次数由配置控制）
- **展示轮换**：每隔固定时间（如2小时）自动刷新展示的礼包
- **不放回随机**：同一轮次内不会重复出现相同礼包，全部展示完后重新开始
- **钻石补充**：支持道具不足时使用钻石补充
- **时间等待**：兑换后需等待指定时间（如20小时）才能领取奖励

## 数据结构

### PlayerRushExchangeBO（数据库表）

| 字段名 | 类型 | 说明 |
|--------|------|------|
| cid | long | 玩家CID |
| group_id | int | 礼包组ID |
| ref_id | int | 当前礼包配置ID |
| active_time_ms | long | 轮换时间基准（兑换开始时间或上次刷新时间） |
| exchange_time_ms | long | 兑换发起时间（0表示未兑换） |
| is_rewarded | bool | 是否已领奖 |
| today_exchange_count | int | 当天兑换次数 |
| next_reset_count_time_ms | long | 下次重置次数时间（每日0点） |

### RushExchange_Info（协议对象）

与数据库字段一致，用于客户端展示。

## 配置系统

### General表配置

| KEY名 | 默认值 | 说明 |
|-------|--------|------|
| rush_exchange_day_can_exchange_times | 1 | 每日可兑换次数 |
| rush_exchange_done_need_wait_sec | 72000 | 兑换完成所需等待时间（秒），默认20小时 |
| rush_exchange_refresh_sec | 7200 | 礼包刷新间隔时间（秒），默认2小时 |

### 配表设计

**rush_exchange_group表**（礼包组）
```
group_id    | 分组ID
condition   | 刷新条件（预留）
```

**rush_exchange表**（礼包配置）
```
id          | 配置ID
group_id    | 分组ID
cost_list   | 消耗列表 CommonItem[]
reward_list | 奖励列表 CommonItem[]
```

**rush_exchange_item表**（道具钻石价值）
```
id          | 配置ID
item        | 物品 CommonItem
gem_count   | 物品钻石价值
```

## 核心功能

### 1. 初始化

```java
// 组件自动初始化
// - 加载数据库数据或创建新数据
// - 随机选择首个礼包
// - 初始化次数重置时间为明天0点
```

### 2. 礼包轮换

**轮换逻辑**：
1. 每隔 `rush_exchange_refresh_sec` 秒自动刷新礼包
2. 从礼包组中随机选择一个未展示过的礼包
3. 如果所有礼包都展示过，清空记录重新开始
4. 仅在没有进行中的兑换时才会轮换

**触发时机**：
- 玩家上线时（组件初始化后）
- 玩家主动请求刷新（`GC2GS_004_102_ReqRushExchangeRefresh`）

### 3. 发起兑换

**请求协议**：`GC2GS_004_101_ReqRushExchange`
```java
bool useGemSupplement; // 是否使用钻石补充
```

**执行流程**：
1. 检查今日兑换次数是否达到上限
2. 检查是否有进行中的兑换
3. 获取当前礼包配置
4. 检查道具配置
5. 计算实际消耗列表（根据是否使用钻石补充）
6. 扣除道具（或道具+钻石）
7. 设置 exchange_time_ms 为当前时间
8. 增加 today_exchange_count
9. 保存数据库
10. 推送信息变更协议

**钻石补充机制**：
```
对于每个消耗道具：
1. 检查玩家拥有数量
2. 如果数量充足：
   - 直接加入消耗列表
3. 如果数量不足：
   - 从 rush_exchange_item 表查询道具钻石价值
   - 如果配表中没有该道具，返回错误（无法补充）
   - 如果玩家有部分道具，先扣除已有部分
   - 计算不足部分的钻石价值：钻石数 = 不足数量 × 单个道具钻石价值
   - 将钻石加入消耗列表

最终消耗列表 = 玩家已有道具 + 补充钻石
```

**钻石补充示例**：
```
需要道具：物品A×100
玩家拥有：物品A×60
配表价值：物品A = 10钻石/个

实际扣除：
- 物品A×60（玩家已有）
- 钻石×400（补充不足的40个，40×10=400）
```

### 4. 领取奖励

**请求协议**：`GC2GS_004_103_ReqRushExchangeReward`

**执行流程**：
1. 检查是否有兑换记录（exchange_time_ms != 0）
2. 检查是否已领奖
3. 检查是否到达可领奖时间（当前时间 >= exchange_time_ms + 等待时间）
4. 发放奖励
5. 标记 is_rewarded = true
6. 如果当天次数未达上限，刷新下一个礼包并重置兑换状态
7. 推送信息变更协议

### 5. 次数重置

**重置时机**：每日0点服务器时间

**重置逻辑**：
1. 检查 current_time >= next_reset_count_time_ms
2. 重置 today_exchange_count = 0
3. 更新 next_reset_count_time_ms = 明天0点
4. 推送信息变更协议

## 协议通信

### 初始化协议

**请求**：`GC2GS_002_085_ReqRushExchangeInit`
- 参数：无
- 响应：`GS2GC_002_085_RetRushExchangeInit`
  - RushExchange_Info - 完整状态信息

### 操作协议

| 协议号 | 请求 | 响应 | 说明 |
|--------|------|------|------|
| 004_101 | GC2GS_004_101_ReqRushExchange | GS2GC_004_101_RetRushExchange | 发起兑换 |
| 004_102 | GC2GS_004_102_ReqRushExchangeRefresh | GS2GC_004_102_RetRushExchangeRefresh | 手动刷新礼包 |
| 004_103 | GC2GS_004_103_ReqRushExchangeReward | GS2GC_004_103_RetRushExchangeReward | 领取奖励 |

### 推送协议

**协议号**：`GS2GC_004_075_OnRushExchangeChg`
- RushExchange_Info - 变更后的信息

**推送时机**：
- 发起兑换后
- 手动刷新礼包后
- 领取奖励后
- 自动刷新礼包后
- 次数重置后

## 错误码

系统使用专用错误码类 `RushExchangeErr` 和通用错误码：

### 专用错误码（RushExchangeErr）

| 错误码 | 数值 | 说明 |
|--------|------|------|
| EXCHANGE_COUNT_NOT_ENOUGH | 600001 | 今日兑换次数已达上限 |
| EXCHANGE_IN_PROGRESS | 600002 | 已有进行中的兑换 |
| EXCHANGE_TIME_NOT_REACH | 600003 | 未到达可领奖时间 |
| EXCHANGE_NOT_FOUND | 600004 | 没有兑换记录 |
| EXCHANGE_ALREADY_REWARDED | 600005 | 已经领取过奖励 |

### 通用错误码

| 错误码类 | 错误码 | 说明 |
|---------|--------|------|
| CommErr | REF_NOT_FOUND | 配置不存在 |
| CommErr | SYS_ERR | 系统错误 |
| CommErr | ITEM_NOT_ENOUGH | 物品不足（道具无法用钻石补充） |
| CommErr | CONSUME_FAIL | 消耗失败 |

## 游戏事件

| 事件枚举 | 说明 |
|---------|------|
| ENPGameEvent.RUSH_EXCHANGE | 发起兑换 |
| ENPGameEvent.RUSH_EXCHANGE_REWARD | 领取奖励 |

## 客户端交互流程

### 正常兑换流程

```
1. 进入界面
   → GC2GS_002_085_ReqRushExchangeInit
   ← GS2GC_002_085_RetRushExchangeInit（返回当前状态）

2. 玩家选择兑换
   → GC2GS_004_101_ReqRushExchange（useGemSupplement=false）
   ← GS2GC_004_075_OnRushExchangeChg（推送变更，exchange_time_ms已设置）

3. 等待20小时后，玩家领取奖励
   → GC2GS_004_103_ReqRushExchangeReward
   ← GS2GC_004_075_OnRushExchangeChg（推送变更，is_rewarded=true，可能已刷新下一个礼包）
```

### 钻石补充流程

```
1. 玩家道具不足，选择钻石补充
   → GC2GS_004_101_ReqRushExchange（useGemSupplement=true）

2. 服务器计算需要的钻石数量
   - 对每个道具：不足数量 * 道具钻石价值
   - 扣除总钻石

3. ← GS2GC_004_075_OnRushExchangeChg（推送变更）
```

### 手动刷新礼包流程

```
1. 客户端检测到达刷新时间（或玩家点击刷新按钮）
   → GC2GS_004_102_ReqRushExchangeRefresh

2. 服务器检查条件：
   - 没有进行中的兑换
   - 已到达刷新时间

3. ← GS2GC_004_075_OnRushExchangeChg（推送变更，ref_id已更新）
```

## 注意事项

### 开发注意

1. **线程安全**：所有数据操作通过玩家锁保证线程安全
2. **懒加载**：数据库记录首次需要时才插入（tryCreateInDB）
3. **增量更新**：使用 ALMySqlUpdateValue 精确更新字段，避免全表更新
4. **配置缓存**：配置对象直接从 RefXXX.getMgr().get() 获取，无需缓存

### 配置注意

1. **礼包组ID**：默认为1，可扩展支持多个礼包组
2. **等待时间**：默认20小时（72000秒），可在general表配置
3. **刷新间隔**：默认2小时（7200秒），可在general表配置
4. **每日次数**：默认1次，通过general表的`rush_exchange_day_can_exchange_times`字段配置

### 客户端注意

1. **时间显示**：
   - 下次刷新时间 = active_time_ms + rush_exchange_refresh_sec * 1000
   - 可领奖时间 = exchange_time_ms + rush_exchange_done_need_wait_sec * 1000
   - 次数重置时间 = next_reset_count_time_ms

2. **状态判断**：
   - exchange_time_ms == 0：无兑换记录
   - exchange_time_ms != 0 && !is_rewarded：兑换中
   - is_rewarded == true：已领奖（可能还有次数）

3. **按钮状态**：
   - 兑换按钮：today_exchange_count < 最大次数 且 exchange_time_ms == 0
   - 领取按钮：exchange_time_ms != 0 且 !is_rewarded 且 当前时间 >= 可领奖时间
   - 刷新按钮：exchange_time_ms == 0 且 当前时间 >= 下次刷新时间

## 扩展功能（预留）

1. **多礼包组**：支持根据玩家等级或战力选择不同的礼包组
2. **解锁条件**：支持配置礼包的解锁条件（玩家等级、功能解锁等）
3. **刷新条件**：在 rush_exchange_group 表中配置刷新条件

## 文件清单

### 组件类
- `RushExchangeComponent.java` - 组件主类

### 消息处理器
- `MsgDealer_GC2GS_002_085_ReqRushExchangeInit.java` - 初始化
- `MsgDealer_GC2GS_004_101_ReqRushExchange.java` - 发起兑换
- `MsgDealer_GC2GS_004_102_ReqRushExchangeRefresh.java` - 手动刷新
- `MsgDealer_GC2GS_004_103_ReqRushExchangeReward.java` - 领取奖励

### Writer类
- `US2GCWriter_002_InitOp.make_085_RetRushExchangeInit()` - 初始化响应
- `US2GCWriter_004_PlayerOp.make_075_OnRushExchangeChg()` - 信息变更推送

### 数据库
- `player_rush_exchange.py` - BO类定义
- `PlayerRushExchangeBO.java` - 自动生成的BO类

### 协议
- `RushExchangeObj.alpro` - 数据结构定义
- `GC2GS_002_085_ReqRushExchangeInit.alpro` - 初始化请求
- `GS2GC_002_085_RetRushExchangeInit.alpro` - 初始化响应
- `GC2GS_004_101_ReqRushExchange.alpro` - 兑换请求
- `GS2GC_004_101_RetRushExchange.alpro` - 兑换响应
- `GC2GS_004_102_ReqRushExchangeRefresh.alpro` - 刷新请求
- `GS2GC_004_102_RetRushExchangeRefresh.alpro` - 刷新响应
- `GC2GS_004_103_ReqRushExchangeReward.alpro` - 领奖请求
- `GS2GC_004_103_RetRushExchangeReward.alpro` - 领奖响应
- `GS2GC_004_075_OnRushExchangeChg.alpro` - 信息变更推送

## 测试建议

### 单元测试

1. **兑换流程测试**
   - 正常兑换（道具足够）
   - 钻石补充兑换（道具不足）
   - 次数限制测试
   - 重复兑换拦截

2. **领奖流程测试**
   - 正常领奖
   - 时间未到领奖
   - 重复领奖拦截
   - 领奖后自动刷新

3. **刷新流程测试**
   - 自动刷新
   - 手动刷新
   - 不放回随机验证
   - 全部展示完后重新开始

4. **次数重置测试**
   - 跨天次数重置
   - 次数重置时间计算

### 集成测试

1. 完整兑换流程（发起→等待→领取→刷新）
2. 钻石补充流程
3. 次数限制和重置
4. 客户端推送协议验证

### 压力测试

1. 并发兑换请求
2. 频繁刷新请求
3. 大量玩家同时在线

## 版本历史

- v1.0.0 (2026-01-21)
  - 初始版本
  - 支持基础兑换、刷新、领奖功能
  - 支持钻石补充
  - 支持每日次数限制
  - 支持礼包不放回随机
