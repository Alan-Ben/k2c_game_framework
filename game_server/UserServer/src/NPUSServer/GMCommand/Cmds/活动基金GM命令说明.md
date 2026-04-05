# 活动基金GM命令使用说明

## 概述
为活动基金组件添加了一系列GM命令，用于测试和调试活动基金相关功能。

## 命令列表

### 1. 查看命令

#### listAllFunds - 查看所有活动基金信息
```
activityFund listAllFunds
```
显示当前玩家所有活动基金的详细信息，包括：
- 基金ID
- 活动实例ID
- 任务分数
- 公式分数
- 总分
- 免费档已领取阶段
- 付费档已领取阶段

#### listTasks - 查看指定基金的任务列表
```
activityFund listTasks [基金ID]
```
显示指定基金下所有任务的详细信息，包括：
- 任务ID
- 当前计数
- 完成次数

**示例：**
```
activityFund listTasks 1001
```

### 2. 分数操作命令

#### addTaskScore - 增加任务分数
```
activityFund addTaskScore [基金ID] [分数]
```
直接增加指定基金的任务分数，并自动更新当前阶段。

**示例：**
```
activityFund addTaskScore 1001 100
```
为基金1001增加100点任务分数。

### 3. 领取进度操作命令

#### setFreeDrawStep - 设置免费档已领取阶段
```
activityFund setFreeDrawStep [基金ID] [阶段号]
```
设置指定基金的免费档已领取阶段。

**示例：**
```
activityFund setFreeDrawStep 1001 5
```
将基金1001的免费档已领取阶段设置为5。

#### setPayDrawStep - 设置付费档已领取阶段
```
activityFund setPayDrawStep [基金ID] [阶段号]
```
设置指定基金的付费档已领取阶段。

**示例：**
```
activityFund setPayDrawStep 1001 5
```
将基金1001的付费档已领取阶段设置为5。

#### resetFreeDrawStep - 重置免费档已领取进度
```
activityFund resetFreeDrawStep [基金ID]
```
将指定基金的免费档已领取进度重置为0。

**示例：**
```
activityFund resetFreeDrawStep 1001
```

#### resetPayDrawStep - 重置付费档已领取进度
```
activityFund resetPayDrawStep [基金ID]
```
将指定基金的付费档已领取进度重置为0。

**示例：**
```
activityFund resetPayDrawStep 1001
```

### 4. 任务操作命令

#### setTaskCount - 设置任务计数
```
activityFund setTaskCount [基金ID] [任务ID] [计数值]
```
直接设置指定任务的计数值（覆盖模式）。如果计数达到完成条件，会自动完成任务并增加分数。

**示例：**
```
activityFund setTaskCount 1001 2001 100
```
将基金1001下任务2001的计数设置为100。

#### addTaskCount - 增加任务计数
```
activityFund addTaskCount [基金ID] [任务ID] [计数值]
```
增加指定任务的计数值（累加模式）。如果计数达到完成条件，会自动完成任务并增加分数。

**示例：**
```
activityFund addTaskCount 1001 2001 50
```
为基金1001下任务2001的计数增加50。

#### resetTaskCount - 重置任务计数
```
activityFund resetTaskCount [基金ID] [任务ID]
```
将指定任务的计数重置为0。

**示例：**
```
activityFund resetTaskCount 1001 2001
```

#### setTaskRefreshTime - 设置任务刷新时间
```
activityFund setTaskRefreshTime [基金ID] [延迟秒数]
```
设置任务刷新时间。当达到设置的时间后，会自动清空所有任务数据。设置为0表示取消刷新。

**注意：**
- 这是GM临时控制，不会保存到数据库
- 仅在内存中生效，服务器重启后失效
- 当时间到达后，会立即清空所有任务数据并重置刷新标记

**示例：**
```
# 设置10秒后刷新任务
activityFund setTaskRefreshTime 1001 10

# 取消刷新
activityFund setTaskRefreshTime 1001 0
```

### 5. 奖励领取命令

#### drawAllRewards - 一键领取所有可领取奖励
```
activityFund drawAllRewards [基金ID]
```
领取指定基金所有可领取的阶段奖励（包括免费档和付费档）。

**示例：**
```
activityFund drawAllRewards 1001
```

## 典型测试场景

### 场景1：测试阶段升级
```bash
# 1. 查看当前基金状态
activityFund listAllFunds

# 2. 增加任务分数，触发阶段升级
activityFund addTaskScore 1001 500

# 3. 再次查看状态，确认阶段已更新
activityFund listAllFunds
```

### 场景2：测试任务完成
```bash
# 1. 查看任务列表
activityFund listTasks 1001

# 2. 增加任务计数
activityFund addTaskCount 1001 2001 100

# 3. 查看任务状态和分数变化
activityFund listTasks 1001
activityFund listAllFunds
```

### 场景3：测试奖励领取
```bash
# 1. 增加分数到一定阶段
activityFund addTaskScore 1001 1000

# 2. 一键领取所有可领取奖励
activityFund drawAllRewards 1001

# 3. 查看领取进度
activityFund listAllFunds
```

### 场景4：重置测试环境
```bash
# 1. 重置免费档领取进度
activityFund resetFreeDrawStep 1001

# 2. 重置付费档领取进度
activityFund resetPayDrawStep 1001

# 3. 重置所有任务计数
activityFund resetTaskCount 1001 2001
activityFund resetTaskCount 1001 2002
# ... 其他任务
```

### 场景5：测试任务刷新机制
```bash
# 1. 查看当前任务状态
activityFund listTasks 1001

# 2. 增加一些任务计数
activityFund addTaskCount 1001 2001 50

# 3. 设置10秒后刷新任务
activityFund setTaskRefreshTime 1001 10

# 4. 等待10秒后，查看任务状态（应该被清空）
activityFund listTasks 1001

# 5. 取消刷新（如果需要）
activityFund setTaskRefreshTime 1001 0
```

## 注意事项

1. **基金ID获取**：使用 `listAllFunds` 命令可以查看所有基金ID
2. **任务ID获取**：使用 `listTasks [基金ID]` 命令可以查看指定基金下的所有任务ID
3. **分数计算**：总分 = 任务分数 + 公式分数
4. **自动完成**：设置任务计数时，如果达到完成条件会自动完成任务并增加分数
5. **协议推送**：所有数据修改都会自动推送协议到客户端
6. **数据持久化**：所有修改都会自动保存到数据库

## 文件说明

- **GM命令类**：`UserServer/src/NPUSServer/GMCommand/Cmds/CmdActivityFund.java`
- **组件类**：`UserServer/src/NPUSServer/NPUSUserMgr/UserComp/ActivityFund/ActivityFundComponent.java`
- **基金信息类**：`UserServer/src/NPUSServer/NPUSUserMgr/UserComp/ActivityFund/ActivityFundInfo.java`
- **任务信息类**：`UserServer/src/NPUSServer/NPUSUserMgr/UserComp/ActivityFund/ActivityFundTaskInfo.java`
