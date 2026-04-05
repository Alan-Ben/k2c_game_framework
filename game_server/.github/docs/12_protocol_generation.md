# 协议生成系统

## 概述

协议生成系统是 GOB 游戏服务器的核心基础设施，通过 `.alpro` 协议定义文件自动生成跨语言的通信协议类。系统支持 Java 和 C# 代码生成，确保客户端和服务器间的协议一致性。

## 系统架构

### 生成流程
```
[.alpro定义] → [协议解析器] → [模板引擎] → [Java/C#代码] → [编译集成]
      ↓              ↓             ↓            ↓            ↓
  [协议脚本]    [语法分析]    [代码模板]    [类文件]    [JAR/DLL]
```

### 三层协议架构

#### 第一层：通信协议（网络传输层）
**职责**：定义客户端和服务器间的具体通信消息
**格式**：`[方向]_[主协议号]_[副协议号]_[功能描述]`

```alpro
// 客户端到服务器请求
ALProtocol GC2GS_036_001_ReqTreasureHuntOreCapture[太空寻宝-捕捉矿石] {
    boolean isAKey[是否一键];
    boolean isAdvance[是否高级];
    long areaId[区域ID];
}

// 服务器到客户端响应
ALProtocol GS2GC_036_001_RetTreasureHuntOreCapture[返回捕捉结果] {
    Common.CommonObj.RESULT result[操作结果];
    long exp[获得经验];
    TreasureHunt_CaptureResult[] captureResults[捕捉结果列表];
}
```

**网络传输格式**：
```
[主协议号(1字节)] + [副协议号(1字节)] + [协议数据内容(变长)]
```

#### 第二层：数据结构（数据复用层）
**职责**：定义可在多个协议间复用的数据结构
**格式**：`[功能模块]_[具体功能]`

```alpro
// 寻宝相关数据结构
ALClass TreasureHunt_AreaInfo[区域信息] {
    long areaId[区域ID];
    int areaType[区域类型];
    string areaName[区域名称];
    boolean isUnlocked[是否解锁];
    long cooldownEndTime[冷却结束时间];
}

ALClass TreasureHunt_CaptureResult[捕捉结果] {
    int itemId[物品ID];
    long count[数量];
    int quality[品质];
    long captureTime[捕捉时间];
}

// 通用数据结构
ALClass NPCommon_ItemInfo[通用物品信息] {
    int itemId[物品ID];
    long count[数量];
    long expireTime[过期时间];
    string extraData[扩展数据];
}
```

#### 第三层：枚举类型（常量定义层）
**职责**：定义系统中使用的枚举常量

```alpro
ALEnum ETreasureHuntAreaType[寻宝区域类型] {
    NORMAL = 1[普通区域];
    RARE = 2[稀有区域];
    EPIC = 3[史诗区域];
    LEGENDARY = 4[传说区域];
}

ALEnum ETreasureHuntCaptureType[捕捉类型] {
    NORMAL = 1[普通捕捉];
    ADVANCE = 2[高级捕捉];
    AKEY = 3[一键捕捉];
}
```

## 协议定义语法

### 基本语法规则

#### 包声明
```alpro
JavaPackage GC2GS.p036_TreasureHuntOp;  // Java包路径
JavaPackage GS2GC.p036_TreasureHuntOp;  // 响应包路径
```

#### 协议定义
```alpro
ALProtocol 协议名[中文描述] {
    字段类型 字段名[字段描述];
    // 更多字段...
}
```

#### 类定义
```alpro
ALClass 类名[中文描述] {
    字段类型 字段名[字段描述];
    // 更多字段...
}
```

#### 枚举定义
```alpro
ALEnum 枚举名[中文描述] {
    常量名 = 值[中文描述];
    // 更多常量...
}
```

### 数据类型系统

#### 基础类型映射
| .alpro类型 | Java类型 | C#类型 | 字节长度 | 说明 |
|------------|----------|---------|----------|------|
| boolean | boolean | bool | 1 | 布尔值 |
| byte | byte | byte | 1 | 有符号字节 |
| short | short | short | 2 | 短整型 |
| int | int | int | 4 | 整型 |
| long | long | long | 8 | 长整型 |
| float | float | float | 4 | 单精度浮点 |
| double | double | double | 8 | 双精度浮点 |
| string | String | string | 变长 | UTF-8字符串 |
| bytebuffer | byte[] | byte[] | 变长 | 二进制数据 |

#### 复合类型
```alpro
// 数组类型
int[] numbers[数字数组];
string[] names[名称数组];
NPCommon_ItemInfo[] items[物品数组];

// 嵌套对象
TreasureHunt_AreaInfo areaInfo[区域信息];
Common.CommonObj.RESULT result[通用结果];
```

#### 可选字段和默认值
```alpro
ALProtocol ExampleProtocol[示例协议] {
    int requiredField[必需字段];
    int optionalField = 0[可选字段，默认值0];
    string name = "default"[默认名称];
}
```

## 代码生成机制

### 生成脚本核心逻辑

#### build_rpc.py 主要流程
```python
def build_rpc_main():
    # 1. 扫描所有.alpro文件
    alpro_files = scan_alpro_files("../ServerProtocol/ProtocolScripts/")
    
    # 2. 解析协议定义
    for alpro_file in alpro_files:
        protocols = parse_alpro_file(alpro_file)
        
        # 3. 生成Java类
        for protocol in protocols:
            if protocol.type == "ALProtocol":
                generate_protocol_class(protocol)
            elif protocol.type == "ALClass":
                generate_data_class(protocol)
            elif protocol.type == "ALEnum":
                generate_enum_class(protocol)
    
    # 4. 生成消息处理器模板
    generate_message_handlers()
```

#### 协议解析器
```python
class ProtocolParser:
    def parse_protocol(self, content):
        # 解析协议定义
        protocol = Protocol()
        protocol.name = extract_protocol_name(content)
        protocol.comment = extract_comment(content)
        protocol.fields = parse_fields(content)
        return protocol
    
    def parse_fields(self, content):
        fields = []
        field_lines = extract_field_lines(content)
        
        for line in field_lines:
            field = Field()
            field.type = extract_field_type(line)
            field.name = extract_field_name(line)
            field.comment = extract_field_comment(line)
            field.default_value = extract_default_value(line)
            fields.append(field)
        
        return fields
```

### 代码模板系统

#### Java协议类模板 (tempalte_rpc_class.java)
```java
package <%ClassPackage%>;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicProtocolPack.BasicObj._ABasicRPCProtoObj;

/**
 * <%ClassComment%>
 * 此文件由工具自动生成，请勿手动修改
 * 生成时间: <%GenerateTime%>
 */
public class <%ClassName%> extends _ABasicRPCProtoObj implements _IALProtocolStructure {
    
    <%FieldDeclarations%>
    
    public <%ClassName%>() {
        super();
        <%InitializeFields%>
    }
    
    <%GetterSetterMethods%>
    
    @Override
    public void serialize(ByteBuffer buffer) {
        <%SerializeLogic%>
    }
    
    @Override
    public void deserialize(ByteBuffer buffer) {
        <%DeserializeLogic%>
    }
    
    @Override
    public int getProtocolId() {
        return <%ProtocolId%>;
    }
    
    @Override
    public String getProtocolName() {
        return "<%ClassName%>";
    }
}
```

#### 消息处理器模板 (tempalte_rpc_handler.java)
```java
package <%HandlerPackage%>;

import NPUSServer.NPUSUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUSUserMsgDispather._ANPUSUserBasicMsgItem;
import <%RequestPackage%>.<%RequestClassName%>;

/**
 * <%RequestClassName%> 的消息处理器
 * 此文件由工具自动生成，请根据业务需求修改
 */
public class <%HandlerClassName%> extends NPUserMsgDealer<<%RequestClassName%>> {
    
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, <%RequestClassName%> _msg) {
        // TODO: 实现具体的业务逻辑
        
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData) {
            _commiter.commitFailRes(CommErr.USER_NOT_FOUND.getCode());
            return;
        }
        
        // 示例代码：
        // 1. 参数验证
        // 2. 业务逻辑处理
        // 3. 构造响应
        
        // 临时响应，避免编译错误
        _commiter.commitFailRes(CommErr.NOT_IMPLEMENTED.getCode());
    }
}
```

### 生成文件组织

#### Java文件输出目录
```
Common/src/
├── GC2GS/                     # 客户端到服务器协议
│   ├── p002_InitOp/
│   ├── p004_PlayerOp/
│   ├── p036_TreasureHuntOp/
│   └── ...
├── GS2GC/                     # 服务器到客户端协议
│   ├── p002_InitOp/
│   ├── p004_PlayerOp/
│   ├── p036_TreasureHuntOp/
│   └── ...
├── Common/                    # 通用数据结构
│   ├── CommonObj/
│   ├── PlayerObj/
│   ├── TreasureHuntObj/
│   └── ...
└── NPEnum/                    # 枚举定义
    ├── ETreasureHuntAreaType.java
    ├── ECurrencyType.java
    └── ...
```

#### 消息处理器输出目录
```
UserServer/src/NPUSServer/NPUserMsgDispather/
├── p036_TreasureHuntOp/
│   ├── MsgDealer_GC2GS_036_001_ReqTreasureHuntOreCapture.java
│   ├── MsgDealer_GC2GS_036_002_ReqTreasureHuntAreaInfo.java
│   └── ...
└── Write/
    ├── US2GCWriter_036_TreasureHuntOp.java
    └── ...
```

## 高级特性

### 多态数据机制
```alpro
// 多态容器定义
ALClass MultiTypeContainer[多类型容器] {
    int dataType[数据类型枚举];
    bytebuffer data[具体数据];
}

// 使用示例
ALProtocol SendMultiTypeData[发送多类型数据] {
    MultiTypeContainer[] containers[容器数组];
}
```

**实现原理**：
- 通过 `枚举类型 + ByteBuffer` 实现统一容器
- 支持不同类型数据的统一存储和传输
- 运行时根据类型枚举进行数据反序列化

### 条件编译支持
```alpro
// 条件编译标记
#ifdef SERVER_ONLY
ALProtocol ServerInternalProtocol[服务器内部协议] {
    long internalData[内部数据];
}
#endif

#ifdef CLIENT_ONLY
ALProtocol ClientUIProtocol[客户端UI协议] {
    string uiData[UI数据];
}
#endif
```

### 版本兼容性
```alpro
ALProtocol VersionedProtocol[版本化协议] {
    int version = 1[协议版本];
    string data[数据];
    
    // 版本2新增字段
    #version >= 2
    long newField[新字段];
    #endversion
}
```

## 开发最佳实践

### 1. 协议设计原则

#### 向后兼容
```alpro
// ✅ 好的设计 - 只在末尾添加新字段
ALProtocol PlayerInfo[玩家信息] {
    long playerId[玩家ID];
    string playerName[玩家名称];
    int level[等级];
    // 新版本添加
    long exp[经验值];              // 版本2新增
    string title[称号];            // 版本3新增
}

// ❌ 坏的设计 - 在中间插入字段
ALProtocol PlayerInfo[玩家信息] {
    long playerId[玩家ID];
    long exp[经验值];              // 不要在中间插入
    string playerName[玩家名称];
    int level[等级];
}
```

#### 字段命名规范
```alpro
// ✅ 好的命名
ALProtocol TreasureHuntCapture[寻宝捕捉] {
    long areaId[区域ID];           // 使用有意义的名称
    boolean isAdvanceCapture[是否高级捕捉];  // 布尔值用is前缀
    int captureCount[捕捉次数];     // 数量用count后缀
    long lastCaptureTime[最后捕捉时间];  // 时间用Time后缀
}

// ❌ 坏的命名
ALProtocol TreasureHuntCapture[寻宝捕捉] {
    long a[区域];                  // 名称过短
    boolean flag[标记];            // 意义不明
    int num[数量];                 // 过于通用
}
```

### 2. 性能优化

#### 减少字段数量
```alpro
// ✅ 合并相关字段
ALClass PlayerPosition[玩家位置] {
    float x[X坐标];
    float y[Y坐标];
    float z[Z坐标];
}

// ❌ 过多细分
ALClass PlayerPosition[玩家位置] {
    float positionX[X坐标];
    float positionY[Y坐标];
    float positionZ[Z坐标];
    float rotationX[X旋转];
    float rotationY[Y旋转];
    float rotationZ[Z旋转];
    float scaleX[X缩放];
    float scaleY[Y缩放];
    float scaleZ[Z缩放];
}
```

#### 使用合适的数据类型
```alpro
// ✅ 根据数据范围选择类型
ALProtocol ItemInfo[物品信息] {
    int itemId[物品ID];            // 4字节足够
    short quality[品质];           // 2字节足够 (1-5)
    long count[数量];              // 可能很大，用8字节
    boolean isEquipped[是否装备];   // 1字节布尔值
}

// ❌ 过度使用long类型
ALProtocol ItemInfo[物品信息] {
    long itemId[物品ID];           // 浪费空间
    long quality[品质];            // 浪费空间
    long count[数量];
    long isEquipped[是否装备];     // 应该用boolean
}
```

### 3. 错误处理

#### 统一错误响应格式
```alpro
// 统一的结果结构
ALClass RESULT[操作结果] {
    int code[错误码];
    string message[错误消息];
}

// 所有响应协议都包含结果
ALProtocol RetTreasureHuntCapture[寻宝捕捉响应] {
    RESULT result[操作结果];       // 必须包含
    // 其他响应数据...
}
```

## 调试和故障排除

### 1. 协议生成失败

#### 常见错误和解决方案

**语法错误**
```
错误：Unexpected token ';' at line 15
解决：检查协议定义语法，确保字段定义正确
```

**类型错误**
```
错误：Unknown type 'UnknownType' in field definition
解决：确保使用的类型已定义或为基础类型
```

**包名错误**
```
错误：Invalid package name 'Invalid.Package.Name'
解决：检查JavaPackage声明，确保包名符合Java规范
```

### 2. 生成代码编译错误

#### 依赖问题
```java
// 错误：找不到符号
import GC2GS.p036_TreasureHuntOp.GC2GS_036_001_ReqTreasureHuntOreCapture;

// 解决：确保协议生成成功，检查包路径
```

#### 泛型问题
```java
// 错误：类型参数不匹配
public class MsgDealer extends NPUserMsgDealer<WrongType> {

// 解决：确保泛型参数与协议类型匹配
public class MsgDealer extends NPUserMsgDealer<GC2GS_036_001_ReqTreasureHuntOreCapture> {
```

### 3. 运行时协议错误

#### 序列化错误
```java
// 添加调试日志
@Override
public void serialize(ByteBuffer buffer) {
    try {
        USLog.debug("Serializing protocol: {}", getProtocolName());
        // 序列化逻辑
    } catch (Exception e) {
        USLog.error("Protocol serialization failed", e);
        throw e;
    }
}
```

#### 版本兼容性问题
```java
// 检查协议版本
if (protocolVersion > getCurrentVersion()) {
    USLog.warn("Protocol version mismatch: client={}, server={}", 
        protocolVersion, getCurrentVersion());
    // 处理版本差异
}
```

## 扩展和自定义

### 1. 自定义代码模板

#### 创建自定义模板
```java
// custom_template.java
package <%ClassPackage%>;

/**
 * <%ClassComment%>
 * 自定义生成模板
 */
public class <%ClassName%> extends CustomBaseClass {
    <%CustomLogic%>
}
```

#### 注册自定义模板
```python
# 在build_rpc.py中添加
custom_templates = {
    'custom_class': 'custom_template.java',
    'custom_handler': 'custom_handler_template.java'
}
```

### 2. 插件系统

#### 协议处理插件
```python
class ProtocolPlugin:
    def pre_generate(self, protocol):
        """生成前处理"""
        pass
    
    def post_generate(self, protocol, generated_code):
        """生成后处理"""
        return generated_code
    
    def validate(self, protocol):
        """协议验证"""
        return True
```

---

**相关文档**：
- [构建和部署系统](02_build_system.md)
- [消息处理系统](11_message_system.md)
- [错误处理系统](13_error_handling.md)
