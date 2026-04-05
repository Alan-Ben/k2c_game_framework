---
name: game_log_persistence
description: 将玩家重要操作和关键数据变更固化到日志数据库（CommLogDB/MJEventLog），用于运营分析、问题排查和客诉追溯。用户说"加操作日志"、"记录玩家行为"、"日志固化"、"写入日志库"、"客诉排查日志"时触发。
---

# 游戏日志固化 Skill

## 适用场景
- 需要记录玩家关键操作（领取奖励、任务推进、状态切换、资源变更）
- 需要沉淀可追溯日志用于后台分析、风控复盘、客诉处理

## 强制规则
1. 日志必须写入日志数据库，不允许只打控制台日志替代
2. 先完成业务校验与状态变更，再记录结果日志；不要在同一循环里混杂校验和修改
3. 优先复用已有日志表与日志方法，避免重复建表
4. 新增日志表/BO 时必须先走 `create_bo` 流程，不手写生成产物
5. 日志字段命名与类型遵循现有 BO/配表规范，注释使用中文
6. 能从上下文拿到事件来源时，必须透传 `NPPlayerContext`，通过 `context.getContextId()` 记录事件
7. **默认策略**：如果需求没有明确说明“运营后台关联的日志数据/运营统计口径”，一律使用 `CommLogDB`

## 日志体系选择
### 1) CommLogDB（推荐默认）
- 适合：常规业务流水、步骤状态变化、调试追踪
- 写法：`CommLogDB.log(bm, logBo, context)`
- 优点：自动补 `eventId/guid/dateTime/timestamp`
- 选择原则：需求未明确标注“运营后台关联日志”时，必须选 CommLogDB

### 2) MJEventLog
- 适合：运营统计口径明确、已约定表结构的梦加事件日志
- 写法：在 `MJEventLog.java` 增加 `logXxx(...)` 静态方法并落库
- 注意：优先遵循 `UserServer/src/MJLog/CLAUDE.md` 的字段与方法约定
- 使用前提：需求中明确说明该日志用于运营后台关联/运营看板统计

## 标准步骤
### 1. 明确记录目标
- 操作名称（谁在什么场景做了什么）
- 关键主键（`cid`、业务 id、dbid）
- 变更前后值（`before/final`）
- 事件来源（`contextId`）

### 2. 确认是否已有日志表/BO
- 先搜索现有 `*LogBO`、`MJEventLog.logXxx`、组件内 `_log(...)`
- 如可复用，仅补字段和调用点
- 如不可复用，先新增定义再生成 BO

### 3. 在业务类中实现 `_log(...)`（参考 PlayerQuestInfo）
```java
/**
 * 记录任务日志
 */
protected void _log(ENpLogType _logType, NPPlayerContext _context)
{
    BM bmObj = getComp().getUserData().getUSServer().getBM();

    LogQuestBO logBo = new LogQuestBO();
    logBo.setCid(bmObj, getComp().getUserData().getCid());
    logBo.setLogType(bmObj, _logType.ordinal());
    logBo.setQuestId(bmObj, getQuestId());
    logBo.setQuestDbid(bmObj, getDbid());
    logBo.setType(bmObj, getRef().quest_type.ordinal());
    logBo.setStep(bmObj, getQuestStep());
    logBo.setExpiredTs(bmObj, getStepExpireTimeTag());
    logBo.setStatus(bmObj, getStatus().ordinal());

    CommLogDB.log(bmObj, logBo, _context);
}
```

### 4. 放置调用点（只在关键节点）
- 成功节点：如创建成功、升级成功、状态推进成功
- 失败节点：仅记录对排查有价值的失败（配表缺失、扣费失败、条件不满足）
- 避免高频无价值刷日志

### 5. 新增日志表时的落地
1. 在 `DBTool/source_db/...` 新建日志表定义
2. 执行 BO 生成命令
3. 在业务代码中填充字段并调用 `CommLogDB.log(...)`
4. 确认 `dbTag` 指向日志库（如 `log` / `us_log`，以现网约定为准）

## 字段设计清单
- 必备：`cid`、业务主键、操作类型、核心状态值
- 推荐：`before_xxx`、`final_xxx`、`op_source`
- 上下文：`eventId`、`guid`（由 `CommLogDB.log` 自动写入）
- 时间：`dateTime`、`timestamp`（由 `CommLogDB.log` 自动写入）

## 常见误区
- 只写 `USLog`/`CommLog` 不落库，导致无法追溯
- 在多处重复写同一条日志，造成统计膨胀
- 不传 `_context`，导致事件来源断链
- 新增日志字段但未生成 BO，运行期字段缺失

## 验收清单
- 日志写入路径明确（CommLogDB 或 MJEventLog）
- 未明确运营后台关联需求时，已确认使用 CommLogDB
- 关键成功/失败节点已覆盖，且无明显重复
- 字段包含业务主键和核心变更值
- 代码编译通过（涉及代码改动时执行构建验证）
- 抽样确认日志库有落数且字段值正确

