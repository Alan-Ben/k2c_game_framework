---
name: create_server_protocol
description: 在两个服务器之间补充新的服务器间通信协议落地约定（To[目标]_R 请求 + To[目标]_RB 响应）。前置必须先使用 protocol skill 完成 `.alpro` 的创建/编辑、包号检查与协议生成；随后再按本 skill 处理 Writer、Handler 与注册。
---

# 服务器间通信协议补充 Skill

> 前置：服务器间协议的 `.alpro` 创建、编辑、包号检查统一先使用 `protocol` skill。

命名规则：`To[目标服务器缩写]_R/RB_[包号]_[序号]_[功能描述]`（服务器缩写见 CLAUDE.md 3.2）

## 步骤

### 1. 确认包号
```bash
ls ServerProtocol/ProtocolScripts/S_[目标]Request/
```

### 2. 按服务器间协议约定放置 .alpro 文件
目录：`ServerProtocol/ProtocolScripts/S_[目标缩写]Request/p[包号]_[功能]Op/`

**请求协议**（`To[目标]_R_[包号]_[序号]_[功能].alpro`）：
```alpro
JavaPackage To[目标]_R.p[包号]_[功能]Op;

ALProtocol To[目标]_R_[包号]_[序号]_[功能描述][中文说明] <[包号], [序号]>
{
    // 请求字段
}
```

**响应协议**（`To[目标]_RB_[包号]_[序号]_[功能].alpro`）：
```alpro
JavaPackage To[目标]_RB.p[包号]_[功能]Op;
import Common.CommonObj.alpro;

ALProtocol To[目标]_RB_[包号]_[序号]_[功能描述][中文说明] <[包号], [序号]>
{
    // 返回字段
}
```

### 3. 生成协议类
```powershell
cd ServerProtocol/ProtocolScripts && ./ALProtocolMaker.exe -a
```
生成规则、字段语法和 import 维护统一以 `protocol` skill 为准。

### 4. 创建 Writer 类（发送方）
路径：`Common/src/NPServerProtocolWriter/To[目标]/Request/To[目标]_R_Writer_[包号]_[功能]Op.java`

```java
package NPServerProtocolWriter.To[目标].Request;

/** [功能描述] 请求 Writer */
public class To[目标]_R_Writer_[包号]_[功能]Op
{
    public static To[目标]_R_[包号]_[序号]_[功能] make_[包号]_[序号]_[功能](/* 参数 */)
    {
        To[目标]_R_[包号]_[序号]_[功能] proto = new To[目标]_R_[包号]_[序号]_[功能]();
        // 设置字段
        return proto;
    }
}
```

### 5. 创建接收方 Handler
目录：`[目标服务器]/src/[目标服务器]Package/RequestDispather/p[包号]_[功能]Op/`

```java
package [目标服务器包].RequestDispather.p[包号]_[功能]Op;

/** [功能中文描述] 请求处理器 */
public class [目标服务器前缀]GeneralRequest_[包号]_[序号]_[功能]
    extends NPRequestDealer<To[目标]_R_[包号]_[序号]_[功能]>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                                To[目标]_R_[包号]_[序号]_[功能] _msg)
    {
        // 业务逻辑处理
        _commiter.commitSucRes([响应Writer].make_[包号]_[序号]_[功能]());
    }
}
```

### 6. 注册 Handler
在目标服务器的 RequestDispatcher 注册文件中添加：
```java
_dispather.regHandler(new [目标服务器前缀]GeneralRequest_[包号]_[序号]_[功能]());
```

### 7. 发送方调用
```java
getServer().sendRequestToBSServer(
    EServerType.[目标类型].ordinal(),
    EN[目标服务器]Type.[目标子类型].ordinal(),
    To[目标]_R_Writer_[包号]_[功能]Op.make_[包号]_[序号]_[功能](/* 参数 */),
    new _IWCGCallbackDealer() {
        @Override
        public _IALProtocolStructure createProtocolObj() {
            return new To[目标]_RB_[包号]_[序号]_[功能]();
        }
        @Override
        public void dealSuc(_IALProtocolStructure _retProto) { /* 处理成功 */ }
        @Override
        public void dealFail(int _errCode) { /* 处理失败 */ }
    }
);
```

## 注意事项
- `To[目标]` 表示**目标服务器**，不是来源