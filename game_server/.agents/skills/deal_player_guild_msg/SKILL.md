---
name: deal_player_guild_msg
description: 处理 MsgDealer 中 GC2GS 联盟协议的固定转发流程：通过 dealGuildMsg/dealGuildMsgByRedirectCommiter 按目标 guildId 转发到联盟服务器；非 MsgDealer 场景请使用 RPC skill。触发关键词：联盟消息、联盟请求、转发到联盟、联盟操作。
---

# 联盟消息转发 Skill

## 适用场景
- MsgDealer 中处理 `GC2GS_*` 联盟相关协议
- 请求需要在目标联盟所在 US 执行（当前 US 仅做转发与结果回传）
- 目标联盟由业务参数指定（如 `_msg.getGuildId()`）

## 不适用场景
- 非 MsgDealer 的其他操作（定时器、组件、GM、服务器消息等）不使用本技能流程，改用 `rpc` skill 进行跨服交互

## 强制规则
1. 仅 MsgDealer 的 `GC2GS_*` 联盟协议使用 `dealGuildMsg(...)` / `dealGuildMsgByRedirectCommiter(...)` 转发，禁止在 MsgDealer 里直连 `GuildMgr`。
2. 先完成前置校验（`userData` 非空、CD/权限、`guildId > 0` 等）。
3. 目标联盟查询类请求，优先使用“消息里的 guildId”；不要误用 `userData.getGuildComponent().getGuildId()`。
4. 有本地状态变更时必须提供 `_failCallback`，在失败回调中完整回滚。
5. `dealGuildMsg(...)` / `dealGuildMsgByRedirectCommiter(...)` 属于玩家消息转发框架流程，不归类为 RPC 流程。

## 路由关键点（必须理解）
- MsgDealer 侧统一按目标 `guildId` 调用 `dealGuildMsg*`，不在业务代码里分叉“本机/跨服”逻辑。
- 目标 US 是本机时，由玩家消息转发框架在本服执行；目标 US 非本机时由框架跨服转发并回传结果。

## 场景选择
- 仅透传联盟处理结果：`dealGuildMsg`
- 需要在调用侧二次处理联盟返回：`dealGuildMsgByRedirectCommiter`

## 模板A：透传场景（目标 guildId 来自消息）

```java
NPUSUserData userData = _commiter.getUserData();
if (null == userData) return;

long guildId = _msg.getGuildId();
if (guildId <= 0) {
    _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
    return;
}

getUSServer().dealGuildMsg(
        _commiter,
        userData.getCid(),
        guildId,
        _msg,
        null
);
```

## 模板B：需要回滚/二次处理

```java
NPUSUserData userData = _commiter.getUserData();
if (null == userData) return;

long guildId = _msg.getGuildId();
if (guildId <= 0) {
    _commiter.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
    return;
}

// 本地前置校验 + 状态变更（记录回滚信息）

getUSServer().dealGuildMsgByRedirectCommiter(
        new _ATGuildUserMsgRedirectCommiter<GuildOp_RetXxx>(_commiter) {
            @Override
            protected GuildOp_RetXxx _createNewTmpObj() {
                return new GuildOp_RetXxx();
            }

            @Override
            protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _msgItem, GuildOp_RetXxx _retMsg) {
                _msgItem.commitSucRes(_retMsg);
            }
        },
        userData.getCid(),
        guildId,
        _msg,
        null,
        (_errCode, _msgItem) -> {
            // 回滚本地状态
            _msgItem.commitFailRes(_errCode);
        }
);
```

## 联盟侧处理链路校验
1. 入口是否正确转发：目标 `guildId` 是否传给 `dealGuildMsg` 或 `dealGuildMsgByRedirectCommiter`。
2. 分发器是否注册：确认对应 `RequestDealer` 已挂载。
3. RequestDealer 是否正确：按 `guildId` 查 `GuildInfo`，并正确 `commitSucRes/commitFailRes`。
4. 回包协议是否匹配：请求号与 Writer 方法严格对应。

## 典型坑位（实战）
- `GC2GS_032_047_ReqGuildIconShow`：
  - 玩家侧应按 `_msg.getGuildId()` 转发。

## 提交前自检
- [ ] MsgDealer 的 `GC2GS_*` 是否通过 `dealGuildMsg`/`dealGuildMsgByRedirectCommiter`
- [ ] 是否完成 `guildId > 0` 与必要前置校验
- [ ] 目标联盟 ID 是否来自正确来源（查询他人联盟时应来自消息）
- [ ] 联盟侧 RequestDealer 是否已注册且可达
- [ ] 回包协议与请求号是否严格匹配
- [ ] 若有本地变更，失败回调是否完整回滚
