---
name: create_player_protocol
description: 为客户端与服务器交互补充 GC2GS/GS2GC 协议落地约定。前置必须先使用 protocol skill 完成 `.alpro` 的创建/编辑、包号检查与协议生成；随后再按本 skill 处理客户端协议目录约定与请求/响应配对。
---

# 客户端-服务器协议补充 Skill

> 前置：协议的创建、编辑、字段调整、包号检查统一先使用 `protocol` skill。

## 步骤

### 1. 确认包号
```bash
ls ServerProtocol/ProtocolScripts/GC2GS/
```
以 `protocol` skill 的检查结果为准，确认客户端目录中未占用的三位包号。

### 2. 按客户端约定放置 .alpro 协议文件
目录：`ServerProtocol/ProtocolScripts/GC2GS/p[包号]_[功能]Op/`，请求和响应放**同一目录**。

**请求协议**（`GC2GS_[包号]_[序号]_[功能].alpro`）：
```alpro
JavaPackage GC2GS.p[包号]_[功能]Op;
csharpspace GC2GS.p[包号]_[功能]Op;

ALProtocol GC2GS_[包号]_[序号]_[功能描述][中文说明] <[包号十进制], [序号十进制]>
{
    // 请求字段
}
```

**响应协议**（`GS2GC_[包号]_[序号]_[功能].alpro`）：
```alpro
import Common.CommonObj.alpro;

JavaPackage GS2GC.p[包号]_[功能]Op;
csharpspace GS2GC.p[包号]_[功能]Op;

ALProtocol GS2GC_[包号]_[序号]_[功能描述][中文说明] <[包号十进制], [序号十进制]>
{
    // 返回字段
}
```

### 3. 生成协议类
```powershell
cd ServerProtocol/ProtocolScripts && ./ALProtocolMaker.exe -a
```
等待命令执行完成，确认生成产物已更新；生成规则统一以 `protocol` skill 为准。

### 4. 确认生成产物
- `ServerProtocol/src/GC2GS/p[包号]_[功能]Op/GC2GS_[包号]_[序号]_[功能].java`
- `ServerProtocol/src/GS2GC/p[包号]_[功能]Op/GS2GC_[包号]_[序号]_[功能].java`

## 注意事项
- 请求（GC2GS）和响应（GS2GC）使用相同的包号和序号
- `<>` 中的包号和序号是十进制整数（如 `<36, 1>`，不是 `<036, 001>`）