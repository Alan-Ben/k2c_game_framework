---
name: protocol
description: 统一处理 `.alpro` 协议、结构体和枚举的创建与编辑。用户说"创建协议"、"编辑协议"、"修改协议字段"、"新增/删除字段"、"新增枚举"、"调整包号/序号"、"alpro"时触发。协议创建与编辑都必须先使用本 skill。
---

# 协议/.alpro Skill

## 适用场景
- 新建客户端协议（`GC2GS` / `GS2GC`）
- 新建服务器间协议（`To[目标]_R` / `To[目标]_RB`）
- 编辑已有协议字段、结构体、枚举、注释、`import`、包号/序号
- 在现有 `.alpro` 文件中追加新的 struct、enum 或协议定义
- 调整协议所在目录、拆分共享定义、维护跨文件引用

## 强制规则
1. 所有协议、结构体、枚举定义都先改 `.alpro`，禁止直接改生成后的 Java/C# 协议类
2. 协议的创建与编辑统一先使用 `protocol` skill，再继续后续落地步骤
3. 创建新的协议文件或包号前，必须先检查现有目录避免冲突
4. 客户端请求（`GC2GS`）和响应（`GS2GC`）使用相同包号和序号，放在同一目录
5. 涉及跨文件类型时，必须同步维护 `import`，不能只改当前定义块
6. 修改 `.alpro` 后必须执行协议生成，并确认产物已更新
7. 使用 ByteBuffer 序列化时，必须先构造正确的 struct/协议对象，不要直接写原始数据
8. 若涉及协议改名或跨文件迁移，先用本 skill 确认变更范围，再配合 `protocol_rename`

## 路径约定
### 客户端协议
- 协议脚本目录：`ServerProtocol/ProtocolScripts/GC2GS/p[包号]_[功能]Op/`
- 请求命名：`GC2GS_[包号]_[序号]_[功能].alpro`
- 响应命名：`GS2GC_[包号]_[序号]_[功能].alpro`

### 服务器间协议
- 协议脚本目录：`ServerProtocol/ProtocolScripts/S_[目标]Request/p[包号]_[功能]Op/`
- 请求命名：`To[目标]_R_[包号]_[序号]_[功能].alpro`
- 响应命名：`To[目标]_RB_[包号]_[序号]_[功能].alpro`

### 共享 struct / enum
- 按现有 `Common/`、`Enum/`、业务目录就近维护
- 优先复用已有 `.alpro` 文件，避免重复定义同名 struct/enum

## 步骤
### 1. 识别变更类型
先明确以下信息：
- 是客户端协议、服务器间协议，还是共享 struct/enum
- 是新建、编辑字段、调整包号/序号、补充枚举，还是改名/迁移
- 是否会影响已有 `.alpro` 的 `import`、字段类型或数组类型引用

### 2. 检查目录与包号
新建前先检查目标目录，确认包号未冲突：

```powershell
Get-ChildItem "ServerProtocol/ProtocolScripts/GC2GS"
Get-ChildItem "ServerProtocol/ProtocolScripts/S_[目标]Request"
```

客户端历史已用包号示例：
- `p002_InitOp`
- `p004_PlayerOp`
- `p024_DungeonOp`
- `p033_SimpleActivityOp`
- `p036_TreasureHuntOp`

> 实际可用包号始终以目录现状为准，不只看文档示例。

### 3. 按 `.alpro` 语法创建或编辑定义
#### 文件基本结构
```alpro
import Common.NPCommonObj.alpro;       // 导入其他alpro文件（可选）

JavaPackage Common.HeroObj;            // Java包路径（必须）
csharpspace Common.HeroObj;            // C#命名空间（服务端专用文件可省略）
```

#### 结构体定义（无包号）
```alpro
ALProtocol Hero_SkinInfo[皮肤信息]
{
    long skinId[皮肤id];
}
```

#### 网络协议定义（带包号和序号）
```alpro
ALProtocol GC2GS_004_002_ReqGmCommand[GM命令请求] <4, 2>
{
    string command[命令内容];
}
```

#### 枚举定义（ALEnum）
```alpro
ALEnum ELevy_Type[征收类型]
{
    NONE;
    SILVER[银币 -> 金币];
    WAIT_RALLY[等待集结];
}
```

#### 支持的字段类型
| 类型 | 说明 |
|------|------|
| `int` / `long` / `short` | 整数 |
| `bool` | 布尔值 |
| `float` | 浮点数 |
| `string` / `String` | 字符串（等价） |
| `bytebuffer` | 原始字节缓冲区 |
| `类型[]` | 数组 |
| `同文件结构体名` | 同文件嵌套结构体 |
| `跨文件全路径` | 跨文件结构体引用 |
| `枚举全路径` | 跨文件枚举引用 |

#### 语法与注释规则
- 字段、结构体、枚举值注释优先使用 `[中文描述]`
- 协议 `<包号, 序号>` 使用十进制整数，如 `<36, 1>`，不要写成 `<036, 001>`
- 新增业务枚举时首项固定为 `NONE`
- 枚举值使用全大写命名，必要时用下划线分隔

### 4. 编辑已有协议时的额外检查
- 若修改字段类型，检查其他 `.alpro` 文件是否引用该 struct/enum
- 若增删字段，确认对应 Writer、MsgDealer、Handler 是否需要同步调整
- 若拆分定义到新文件，补齐新旧文件的 `import`
- 若只删除同文件中的某个协议定义块，不要误删同文件其他定义

### 5. 生成协议代码
在协议脚本目录执行：

- 如果任务中存在多个 `.alpro` 脚本修改，需要在最后一个脚本修改完成后再统一生成协议
- 如果同时存在代码修改，则 `.alpro` 文件的修改必须在代码修改之前处理

```powershell
Set-Location "ServerProtocol/ProtocolScripts"
.\ALProtocolMaker.exe -a
```

若生成报错，每轮优先修复第一条报错，再重新生成，直到无报错。

### 6. 确认生成产物
至少确认以下内容：
- `ServerProtocol/src/` 下对应生成类已更新
- 旧字段/旧类型名没有在新生成代码中残留
- 新增协议的包路径与 `.alpro` 中的 `JavaPackage` 一致

## 验收清单
- `.alpro` 定义已按目标场景创建或编辑完成
- 包号、序号、目录位置无冲突
- `import`、跨文件引用、同包请求/响应关系正确
- 已执行 `ALProtocolMaker.exe -a` 且生成成功



