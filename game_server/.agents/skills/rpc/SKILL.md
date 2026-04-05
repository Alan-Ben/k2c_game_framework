---
name: rpc
description: 在服务器之间通过 RPC 完成跨服数据交互与处理。用户说"RPC通讯"、"跨服数据交互"、"跨服查询数据"、"跨服处理逻辑"、"用RPC请求其他服务器"时触发。此 skill 依赖 protocol skill：凡是新增或调整 RPC 请求/返回结构字段，必须先通过 protocol skill 完成协议定义或字段修改。
---

# 跨服 RPC Skill

## 适用场景
- 涉及跨服数据查询或跨服处理逻辑
- 需要 US 与其他 US 或专服（如 DinnerServer）做请求-响应式交互

## 依赖关系
- RPC 请求/返回结构新增或调整时，必须先使用 `protocol` skill
- RPC 关联协议定义位置：`ServerProtocol/ProtocolScripts/ALLRPC`

## 强制规则
1. 涉及本地/跨服判断时，统一走发送 RPC 逻辑；不要在业务层分叉两套处理流程
2. 本服与跨服的区分由发送逻辑内部处理（`RpcSender` 内已处理本服直达）
3. Handler 必须实现 `_IAutoRegistHandler`，依赖 `RpcDispatcher.autoRegistHandler(...)` 自动注册
4. US 侧 Handler 优先继承 `_ATBasicUSRpc_Handler<T>`；非 US 侧继承 `RpcRequestHandler<T>`
5. Handler 统一使用 `_rpc.req()` 取参、`_rpc.retObj()` 填充返回，最终 `commit()` 或 `commitFail()`
6. `ERpcClassName` 只能追加，不能删除或重排
7. 回调先处理 `_errCode > 0`，再处理成功分支；一次请求只提交一次结果

## 路径与命名
- RPC 类：`Common/src/AllRpcData/[服务域]/[模块]/`
- RPC classId：`Common/src/AllRpcData/ERpcClassName.java`
- US 侧 Handler：`UserServer/src/USServer/RPCDispatcher/[模块]/[Rpc类名]_Handler.java`
- 专服 Handler：`[目标服务器]/src/[目标包]/RPCDispatcher/[模块]/[Rpc类名]_Handler.java`

## 落地步骤
1. 先用 `protocol` skill 完成 `ALLRPC` 下协议定义/字段调整
2. 新增或维护 RPC 类（`_ARPCBase<Req, Return>`），并在 `ERpcClassName` 追加枚举
3. 业务侧统一写发送 RPC 调用（如 `rpc2us()`、`rpc2dinner()`），不要额外写“本地直处理分支”
4. 在目标服务器实现 Handler，按规范读取 `req`、填充 `retObj` 并提交
5. 校验自动注册、成功回调、失败回调都能闭环

## 最小验收清单
- 已确认该场景使用 RPC 而非直接跨服调用
- 若改动字段，已先完成 `protocol` skill 前置处理
- `AllRpcData`、`ERpcClassName`、发送端、Handler 四个节点已闭环
- 成功/失败分支都明确，且请求只提交一次结果


