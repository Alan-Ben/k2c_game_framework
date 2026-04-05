---
name: cross_data
description: 构建跨服数据集能力（CrossData）。当需求是“把多个US的数据做集中同步、聚合查询”时触发。基于 UserServer/CrossDataServer 两侧 `_ATCrossDataMgr` 固定基类快速落地；明确 CrossDataServer 仅负责数据集中与同步，实际业务修改必须回到目标US执行。
---

# cross_data Skill（跨服数据集）

## 目标
- 统一实现“多 US 数据 -> CrossDataServer 聚合数据集”的底层能力。
- 为后续跨服基础交互提供查询视图和索引支持。
- 严格区分职责：
  - `CrossDataServer`：只做数据集中、同步、聚合查询。
  - 业务处理服（通常是目标 `US`）：做真实业务校验与状态修改。

## 触发场景
- 用户提到：跨服数据集、CrossData 聚合、多 US 汇总查询、跨服共享读模型。
- 需要复用固定框架快速新增一类可同步数据（新增 `ECrossDataType` + 双侧管理器）。

## 非目标
- 不在 `CrossDataServer` 直接执行业务扣费、发奖、状态变更。
- 不把 `CrossData` 当作业务真值库；它是“跨服共享读模型/缓存层”。

## 关键代码锚点
- US 侧基类：`UserServer/src/NPUSServer/CrossDataBasicPack/_ATCrossDataMgr.java`
- US 侧数据对象基类：`UserServer/src/NPUSServer/CrossDataBasicPack/_ATCrossDataInfo.java`
- US 侧核心注册与重同步：`UserServer/src/NPUSServer/CrossDataBasicPack/CrossDataCore.java`
- US->CDS 协议 Writer：`UserServer/src/NPUSServer/CrossDataBasicPack/Writer/GOM2CD_R_Writer_001_DataOp.java`
- CDS 侧基类：`CrossDataServer/src/CrossDataServer/CrossDataMgr/_ATCrossDataMgr.java`
- CDS 侧类型管理器：`CrossDataServer/src/CrossDataServer/CrossDataMgr/_ACrossDataTypeMgr.java`
- CDS 侧按 US 分桶：`CrossDataServer/src/CrossDataServer/CrossDataMgr/_TCrossDataUSMgr.java`
- CDS 控制器：`CrossDataServer/src/CrossDataServer/CrossDataMgr/CrossDataController.java`
- CDS 请求分发：`CrossDataServer/src/CrossDataServer/GeneralListener/RequestDispather/GOMCDGeneral_001_RequestDispatcher_DataOp.java`
- 示例实现：
  - US：`UserServer/src/NPUSServer/UsMars/MineCore/UsMarsMineCore.java`
  - US DataInfo：`UserServer/src/NPUSServer/UsMars/MineCore/UsMarsMineObj.java`
  - CDS Test：`CrossDataServer/src/CrossDataServer/CrossDataMgr/TestData/`

## 标准落地流程
1. 定义跨服数据协议与类型枚举  
   - 在 `ServerProtocol/ProtocolScripts/Common/CrossData.alpro` 增加数据结构。
   - 在 `ServerProtocol/ProtocolScripts/Enum/CrossDataType.alpro` 增加 `ECrossDataType` 枚举值。
   - 运行协议生成：`cd ServerProtocol/ProtocolScripts && .\\ALProtocolMaker.exe -a`

2. US 侧实现“可同步数据对象”  
   - 新建 `XxxCrossDataInfo extends _ATCrossDataInfo<TProtocol>`。
   - 必须实现：
     - `getDataId()`：跨 US 全局唯一（建议 `UsID.makeUsId(us, localId)` 形式）。
     - `makeCrossDataProtocolObj()`：构造用于同步的协议对象。

3. US 侧实现“数据管理器”  
   - 新建 `XxxCrossDataMgr extends _ATCrossDataMgr<TProtocol, XxxCrossDataInfo>`。
   - 必须实现：
     - `_getInitSyncDataPerPageCount()`
     - `_onAddData_InLock(...)`
     - `_onRmvData_InLock(...)`
   - 初始化期加载本服数据时，按场景使用 `_initAddData` 或 `addData`。
   - 进入跨服分组时调用 `initSyncData(groupId)`；分组变更时调用 `chgGroupId(groupId)`。

4. CDS 侧实现“聚合管理器”  
   - 新建 `XxxCDDataInfo implements _ITCrossDataInfo<TProtocol>`：
     - `getDataId()`
     - `syncData(TProtocol)`
   - 新建 `XxxCDGroupMgr extends _ATCrossDataMgr<TProtocol, XxxCDDataInfo>`：
     - `_getDataId(...)`
     - `_readData(ByteBuffer)`
     - `_createNewDataInfo(...)`
     - `_onAddData_InLock(...)`
     - `_onRmvData_InLock(...)`
     - `_dealCustomOp(...)`（处理查询类自定义操作）
   - 新建 `XxxCDTypeMgr extends _ACrossDataTypeMgr`，在 `_createNewGroupDataMgr(groupId)` 返回 `new XxxCDGroupMgr(groupId)`。
   - 在 CrossDataServer 启动阶段注册：`CrossDataController.regDataMgr(new XxxCDTypeMgr(...));`

5. 查询与业务处理分离  
   - 查询：业务服可通过 `sendCustomOpToCrossData(...)` 请求 CDS 聚合数据。
   - 修改：拿到目标数据归属 `usId` 后，必须把业务消息/RPC 发到目标 `US` 执行。
   - 结论：`CrossData` 只提供“看得见”，不提供“改得动”。

## 代码骨架
```java
// US 侧 DataInfo
public class XxxCrossDataInfo extends _ATCrossDataInfo<XxxProto> {
    @Override
    public long getDataId() {
        return UsID.makeUsId(_usServer, _localId);
    }

    @Override
    public XxxProto makeCrossDataProtocolObj() {
        return new XxxProto(getDataId(), ...);
    }
}
```

```java
// US 侧 Mgr
public class XxxCrossDataMgr extends _ATCrossDataMgr<XxxProto, XxxCrossDataInfo> {
    public XxxCrossDataMgr(NPUserServer _usServer) {
        super(_usServer, ECrossDataType.XXX.ordinal());
    }

    @Override protected int _getInitSyncDataPerPageCount() { return 200; }
    @Override protected void _onAddData_InLock(XxxCrossDataInfo _data) {}
    @Override protected void _onRmvData_InLock(XxxCrossDataInfo _data) {}
}
```

```java
// CDS 侧 Group Mgr
public class XxxCDGroupMgr extends _ATCrossDataMgr<XxxProto, XxxCDDataInfo> {
    public XxxCDGroupMgr(long _groupId) { super(_groupId); }

    @Override protected long _getDataId(XxxProto _dataProtocol) { return _dataProtocol.getDataId(); }
    @Override protected XxxProto _readData(ByteBuffer _data) { ... }
    @Override protected XxxCDDataInfo _createNewDataInfo(XxxProto _dataProtocol) { ... }
    @Override protected void _onAddData_InLock(XxxCDDataInfo _data) { ... }
    @Override protected void _onRmvData_InLock(XxxCDDataInfo _data) { ... }
    @Override public void _dealCustomOp(_IWCGBasicRequestCommiter _commiter, ByteBuffer _protocol) { ... }
}
```

## 强约束与常见坑
- `DataId` 必须全局唯一且稳定，否则跨 US 覆盖/误删。
- `initSyncData` 成功前不要依赖实时同步结果；框架有重试与重同步机制。
- `onCrossDataServerOnline()` 会触发全量重同步，新增类型后要确保幂等。
- `CrossDataServer` 不能承接业务事务；只做“集中、同步、查询”。
- 查询后若要修改业务数据，必须路由到目标 US 处理，禁止在查询服本地硬改。

## 自检清单
- [ ] `Common/CrossData.alpro` 已新增结构并生成协议代码
- [ ] `Enum/CrossDataType.alpro` 已新增类型枚举
- [ ] US 侧 `DataInfo` + `Mgr` 已实现并接入分组同步
- [ ] CDS 侧 `DataInfo` + `GroupMgr` + `TypeMgr` 已实现并注册
- [ ] 自定义查询仅走 CDS，业务修改已路由目标 US
- [ ] 关键日志与失败回调已补齐（尤其是自定义查询与跨服路由失败）
