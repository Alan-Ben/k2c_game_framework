---
name: offline_dealer
description: 统一处理 OfflineDealer 离线事件（玩家在线与离线都必须生效），覆盖 _AOfflineDataDealer 的新增、改造与排查。用于离线奖励发放、出征/战斗结算、联盟申请与审批、订单回调、禁言状态变更等场景；当需求包含“玩家不在线也要处理”“OfflineRewardFunc.addPlayerOfflineReward”“_AOfflineDataDealer”时触发。
---

# offline_dealer Skill

## 目标
- 统一离线事件处理，保证玩家在线/离线结果一致。
- 约束 `_AOfflineDataDealer` 实现，避免漏处理、重复处理、漏注册。

## 核心规则
1. 离线事件统一入口：`OfflineRewardFunc.addPlayerOfflineReward(...)`。
2. `syncToClient()` 决策：
   - `true`：落库并可客户端展示/领取。
   - `false`：仅预处理，不入客户端领取链路。
3. `_preDeal(...)` 只执行一次：由基类 `preDeal` 控制 `hasPreDeal/markHasPreDeal`。
4. 调用 `OfflineRewardFunc.addPlayerOfflineReward(...)` 的上层不要再包 `getLoaderMgr().safeCall(...)`，避免重复调度。
5. 发现“在线 `addReward(...)` + 离线手动写 `PlayerOfflineRewardBO`”分支时，默认收敛为 `OfflineRewardFunc.addPlayerOfflineReward(...)`。
6. 上游已有 `ByteBuffer`（如 `_rpc.req().get_buffer_xxx()`）时，优先使用 `OfflineRewardFunc` 的 `ByteBuffer` 重载，避免对象中转。

## 标准流程（新增或改造）
1. 先判定业务类型：仅状态推进用 `syncToClient=false`；需要玩家领取用 `syncToClient=true`。
2. 先改 `.alpro`（禁止改生成代码）：
   - 枚举：`ServerProtocol/ProtocolScripts/Enum/OfflineRewardEnum.alpro`
   - 数据结构：`OfflineRewardObj` 或 `ServerObj` 对应 `.alpro`
   - 客户端可见枚举按约定加 `C_` 前缀
   - 改动后执行协议生成（涉及协议定义必须先走 `protocol` skill）
3. 新建 Dealer：`OfflineDealer_XXX` 继承 `_AOfflineDataDealer`，实现 `getEnum/isValid/syncToClient/_preDeal`。
4. 注册 Dealer：`OfflineDataDealerMgr` 构造中 `regDealer(new OfflineDealer_XXX())`。
5. 触发侧统一走 `OfflineRewardFunc.addPlayerOfflineReward(...)`，不要直接调组件 `addReward(...)`。
6. 需要领取扩展数据时重写 `takeReward(...)`，通过 `OfflineRewardTakeResult.setExtData(...)` 返回。

## 实现模板
```java
public class OfflineDealer_XXX extends _AOfflineDataDealer {
    @Override
    public EOfflineRewardEnum getEnum() {
        return EOfflineRewardEnum.XXX;
    }

    @Override
    public boolean isValid() {
        return true;
    }

    @Override
    public boolean syncToClient() {
        return false;
    }

    @Override
    protected void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context) {
        if (null == _info.getOfflineData()) {
            return;
        }
        try {
            // 1) readPackage 反序列化
            // 2) 调用对应组件完成补偿/状态推进
        } catch (Exception ex) {
            USLog.error(_info.getUserData().getUSServer(), "OfflineDealer_XXX._preDeal - exception, cid={}", _info.getUserData().getCid(), ex);
        }
    }
}
```

## 自检清单
- [ ] 枚举与离线数据结构已在 `.alpro` 定义并重新生成协议。
- [ ] `OfflineDataDealerMgr` 已注册新 Dealer。
- [ ] `_preDeal` 空数据保护 + `readPackage` + 异常日志齐全。
- [ ] `syncToClient` 与需求一致（是否需要客户端领取）。
- [ ] 触发侧统一使用 `OfflineRewardFunc.addPlayerOfflineReward(...)`，未绕开离线框架。
- [ ] 若改动 Java 代码，完成构建验证。

## 常见坑
- 忘记注册 Dealer，导致离线记录存在但无法处理。
- 需要客户端领取却未使用 `C_` 枚举，导致展示链路不一致。
- 在 `_preDeal` 做了非幂等副作用且无保护，重复登录时可能重复生效。
- 需要扩展领取结果却未重写 `takeReward`，客户端拿不到展示扩展数据。

## 文档自查结论
- 入口、决策点、实现流程、回归检查已闭环，内容可直接执行。
- 已删除重复场景罗列，仅保留规则与动作，降低歧义和维护成本。
- 与仓库规范一致：协议先改 `.alpro`、统一入口写离线、改动后需构建验证。
