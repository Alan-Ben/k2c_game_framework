---
name: player_offline_data
description: 统一处理“无论玩家是否在线，都能获取或更新玩家最新数据”的场景。基于 PlayerCacheData、UsOfflineTmpDataMgr_PlayerCache、_AUsOfflineTmpDataMgr 的现有模式，沉淀在线直取 + 离线回源 + 回调归一方案。用户提到“离线数据”“offlinedata”“离线也能获取”“在线离线统一取数”时触发。
---

# player_offline_data Skill

## 目标
- 提供一个统一入口，屏蔽在线/离线分支差异。
- 在线时读取实时内存数据，离线时读取最新持久化数据。
- 复用离线临时管理器，避免重复查库和并发重复加载。

## 参考实现
- `PlayerCacheData`
- `UsOfflineTmpDataMgr_PlayerCache`
- `_AUsOfflineTmpDataMgr`
- `_ATUserOfflineTmpDataMgr`
- `PlayerCacheFunc.getData(...)`

## 标准流程
1. 在线优先：
   - `lookupCacheUserData(_cid)` 判断是否在线。
   - 在线分支必须使用 `userData.safeCall(...)` 返回数据。
2. 离线回源：
   - 调用 `getOfflineTmpDataCore().localDoOfflineData(_dataType, _cid, _dataId, _callback)`。
   - 由离线框架完成按 `cid` 分桶、按 `dataId` 去重、回调聚合。
3. 结果归一：
   - 对业务侧统一返回 `(isSuc, data)`，不暴露底层来源。

## 落地步骤
1. 新建离线数据对象：
   - `UserOfflineTmpDataInfo_Xxx implements _IUserOfflineTmpDataInfo`
   - 实现 `getDataId()`，并提供 `initFromBo(...)`
2. 新建单玩家数据管理器：
   - `UserOfflineTmpDataMgr_Xxx extends _ATUserOfflineTmpDataMgr<UserOfflineTmpDataInfo_Xxx>`
   - 只实现 `_loadDataOp(...)`（查库成功 `dealSuc`，失败 `dealFail`）
3. 新建类型管理器：
   - `UsOfflineTmpDataMgr_Xxx extends _AUsOfflineTmpDataMgr<...>`
   - 绑定唯一 `Const_UsOfflineTmpData.C_TmpDataType_Xxx`
4. 注册管理器：
   - 在 `NPUserServer._init()` 调用 `initRegUsOfflineTmpDataMgr_Unsafe(new UsOfflineTmpDataMgr_Xxx(...))`
5. 提供统一访问函数：
   - 新建 `XxxCacheFunc.getData(...)`，固定“在线直取 + 离线回源”模板

## 模板代码
```java
public static void getData(NPUserServer _server, long _cid, HandlerTwo<Boolean, UserOfflineTmpDataInfo_Xxx> _dealer)
{
    if (null == _server || null == _dealer)
        return;

    NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
    if (null != userData)
    {
        userData.safeCall(() -> _dealer.handle(true, userData.getXxxComponent().getXxxData()));
        return;
    }

    _server.getOfflineTmpDataCore().localDoOfflineData(Const_UsOfflineTmpData.C_TmpDataType_Xxx, _cid, _cid,
            new _ICallBackResultT<UserOfflineTmpDataInfo_Xxx>()
            {
                @Override
                public void onRunOver(Result _result, UserOfflineTmpDataInfo_Xxx _offlineDataInfo)
                {
                    _dealer.handle(null != _offlineDataInfo, _offlineDataInfo);
                }
            });
}
```

## 关键约束
- `dataType` 必须全局唯一。
- `dataId` 必须稳定可定位（按玩家维度通常使用 `cid`）。
- 禁止业务层绕过 `localDoOfflineData(...)` 直接查库。
- 在线分支必须 `safeCall(...)`，避免跨线程读取玩家对象。
- 离线临时数据是短期缓存，玩家上线后会 `clearCidData(_cid)`，不可作为长期状态源。

## 自检清单
- [ ] `Const_UsOfflineTmpData` 已新增唯一类型常量
- [ ] `UsOfflineTmpDataMgr_Xxx` 已在 `NPUserServer._init()` 注册
- [ ] `_loadDataOp(...)` 成功/失败分支完整
- [ ] 业务方统一通过 `XxxCacheFunc.getData(...)` 访问
- [ ] 在线分支使用 `safeCall(...)`
- [ ] 若改动 Java 代码，完成构建验证

