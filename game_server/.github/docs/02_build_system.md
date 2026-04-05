# 构建和部署系统

## 概述

GOB 游戏服务器采用多层次的构建系统，结合 Python 脚本自动化代码生成和 Apache Ant 构建工具，实现从协议定义到最终部署的完整自动化流程。系统的核心特点是代码生成驱动的开发模式。

## 构建系统架构

### 整体构建流程
```
[协议定义] → [代码生成] → [编译构建] → [打包部署]
    ↓           ↓           ↓           ↓
[.alpro]   [Python脚本]  [Ant构建]   [JAR文件]
    ↓           ↓           ↓           ↓
[错误码]   [Java类生成]  [依赖编译]  [服务器部署]
```

### 关键目录结构
```
gob_tp_server/
├── bat/                    # 代码生成脚本目录
│   ├── build_enum.py      # 枚举生成
│   ├── build_err.py       # 错误码生成
│   ├── build_rpc.py       # 协议生成
│   ├── build_events.py    # 事件生成
│   └── err/               # 错误码定义文件
├── build/                 # Ant 构建系统
│   ├── build2.xml         # 主构建配置
│   ├── build2.bat         # Windows 构建脚本
│   └── apache-ant-1.9.4/  # Ant 工具
├── ServerProtocol/        # 协议定义
│   └── ProtocolScripts/   # .alpro 协议脚本
└── Lib/                   # 编译输出 JAR 文件
```

## 代码生成系统

### 1. 枚举生成 (build_enum.py)

**执行顺序**：第一步（其他系统依赖枚举）

**功能**：
- 扫描所有枚举定义文件
- 生成跨语言的枚举类型
- 包含类型安全转换和性能优化
- 自动生成边界检查和缓存机制

**使用方法**：
```bash
cd bat
python build_enum.py
```

### 2. 错误码生成 (build_err.py)

**执行顺序**：第二步（协议生成可能需要错误码）

#### 错误码定义格式
错误码文件位于 `bat/err/` 目录，命名格式：`[主错误码序号]_[类名]_[描述].txt`

**示例文件**：`001_CommErr_通用错误.txt`
```txt
java_path= ../Common/src              # Java代码输出路径
java_package=NPCommon.ErrMain         # Java包名
csharp_type = main                    # C#程序集类型(main/hotfix)
id	name	desc                      # 表头
1	SYS_ERR	系统错误                   # 错误码定义行
2	REF_NOT_FOUND	配表找不到         # 子错误码定义
```

#### 错误码计算规则
- **最终错误码** = `主错误码 * 10000 + 子错误码`
- **示例**：`001_CommErr` 的 `SYS_ERR(1)` = 1 * 10000 + 1 = 10001

**生成的Java类示例**：
```java
public enum CommErr {
    SYS_ERR(10001, "系统错误"),
    REF_NOT_FOUND(10002, "配表找不到");
    
    private final int code;
    private final String desc;
    
    // 自动生成的方法
    public static CommErr getSucc() { return null; }
    public static boolean isSucc(CommErr err) { return err == null; }
}
```

**使用方法**：
```bash
cd bat
python build_err.py
```

### 3. 协议生成 (build_rpc.py)

**执行顺序**：第三步（消息处理器依赖协议类）

#### 协议定义系统

**三层架构自动生成跨语言代码**：

##### 第一层：通信协议（网络传输层）
**格式**：`[方向]_[主协议号]_[副协议号]_[功能描述]`

**示例**：
```alpro
JavaPackage GC2GS.p036_TreasureHuntOp;  // 请求协议包
JavaPackage GS2GC.p036_TreasureHuntOp;  // 响应协议包

ALProtocol GC2GS_036_001_ReqTreasureHuntOreCapture[太空寻宝-捕捉矿石] {
    boolean isAKey[是否一键];
    boolean isAdvance[是否高级];
    long areaId[区域ID];
}

ALProtocol GS2GC_036_001_RetTreasureHuntOreCapture[返回捕捉结果] {
    Common.CommonObj.RESULT result;
    TreasureHunt_CaptureResult[] captureResults[捕捉结果列表];
}
```

**网络包结构**：`[主协议号(1字节)] + [副协议号(1字节)] + [协议数据内容(变长)]`

##### 第二层：数据结构（数据复用层）
**格式**：`[功能模块]_[具体功能]`

**示例**：
```alpro
ALClass TreasureHunt_OreInfo[矿石信息] {
    int oreType[矿石类型];
    int quality[品质];
    long count[数量];
}

ALClass NPCommon_ItemInfo[通用物品信息] {
    int itemId[物品ID];
    long count[数量];
    long expireTime[过期时间];
}
```

##### 第三层：枚举类型（常量定义层）
- 包含类型安全转换和性能优化
- 自动生成边界检查和缓存机制

#### 协议生成模板系统

**RPC类生成模板** (`tempalte_rpc_class.java`)：
```java
package <%ClassPackage%>;

/**
 * <%ClassComment%>
 * 此文件由工具自动生成，请勿手动修改
 */
public class <%ClassName%> extends _ABasicRPCClass {
    // 自动生成的字段和方法
}
```

**消息处理器模板** (`tempalte_rpc_handler.java`)：
```java
package <%HandlerPackage%>;

public class <%HandlerClassName%> extends NPUserMsgDealer<<%RequestClassName%>> {
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, 
                               <%RequestClassName%> _msg) {
        // TODO: 实现业务逻辑
    }
}
```

#### 数据类型映射表
| .alpro类型 | Java类型 | C#类型 | 特点 |
|------------|----------|---------|------|
| int | int | int | 4字节整数 |
| long | long | long | 8字节长整数 |
| boolean | boolean | bool | 1字节布尔 |
| string | String | string | UTF-8字符串 |
| 基础类型[] | ArrayList<包装类> | List<T> | 动态数组 |
| bytebuffer | byte[] | byte[] | 二进制数据 |
| 协议对象 | 完整包名类 | 命名空间类 | 嵌套对象 |

**使用方法**：
```bash
cd bat
python build_rpc.py
```

### 4. 事件生成 (build_events.py)

**执行顺序**：第四步（可选，用于特定功能）

**功能**：
- 生成游戏事件类
- 支持事件监听和分发机制
- 用于组件间的松耦合通信

**使用方法**：
```bash
cd bat
python build_events.py
```

### 5. 数据库代码生成

**执行位置**：`DBTool/` 目录

**功能**：
- 根据 Python 表定义生成 BO (Business Object) 类
- 生成 BM (Business Manager) 类
- 支持数据库 CRUD 操作的自动化

**使用方法**：
```bash
cd DBTool
python genAll.py
```

## Ant 构建系统

### 主构建配置 (build2.xml)

#### 构建顺序和依赖
```xml
<project name="GOE_Server" default="build" basedir="./">
    <!-- 构建顺序：ServerProtocol → Common → GameRes → GameLogicCommon → 各服务器模块 -->
    
    <target name="init">
        <!-- 清理旧版本 JAR 文件 -->
        <delete file="../Lib/Common.jar"/>
        <delete file="../Lib/ServerProtocol.jar"/>
        <delete file="../Lib/GameRes.jar"/>
        <delete file="../Lib/GameLogicCommon.jar"/>
    </target>
    
    <target name="ServerProtocol">
        <!-- 编译协议相关类 -->
    </target>
    
    <target name="Common">
        <!-- 编译通用类库 -->
    </target>
    
    <!-- 其他构建目标 -->
</project>
```

#### Java 编译配置
```xml
<presetdef name="javac">
    <javac encoding="utf-8" 
           debug="true" 
           debuglevel="lines,vars,source" 
           includeantruntime="false" 
           source="1.8" 
           target="1.8"/>
</presetdef>
```

### Windows 构建脚本 (build2.bat)

```batch
@echo off
chcp 65001
cd /d "%~dp0"

echo 开始构建项目...
call apache-ant-1.9.4\bin\ant.bat build -buildfile build2.xml

if %errorlevel% neq 0 (
    echo 构建失败！
    pause
    exit /b 1
)

echo 构建完成！
pause
```

## 完整构建流程

### 标准构建顺序

**重要**：构建顺序不能颠倒，存在严格的依赖关系

```bash
# 1. 枚举生成（先执行，因为其他系统依赖枚举）
cd bat
python build_enum.py

# 2. 错误码生成（协议生成可能需要错误码）
python build_err.py

# 3. 协议生成（消息处理器依赖协议类）
python build_rpc.py

# 4. 事件生成（可选，用于特定功能）
python build_events.py

# 5. 数据库生成（BO类生成）
cd ../DBTool
python genAll.py

# 6. 最终构建
cd ../build
build2.bat
```

### 一键构建脚本示例

**创建完整构建脚本** (`full_build.bat`)：
```batch
@echo off
echo 开始完整构建流程...

echo 1. 生成枚举...
cd bat
python build_enum.py
if %errorlevel% neq 0 goto error

echo 2. 生成错误码...
python build_err.py
if %errorlevel% neq 0 goto error

echo 3. 生成协议...
python build_rpc.py
if %errorlevel% neq 0 goto error

echo 4. 生成事件...
python build_events.py
if %errorlevel% neq 0 goto error

echo 5. 生成数据库代码...
cd ../DBTool
python genAll.py
if %errorlevel% neq 0 goto error

echo 6. Ant 构建...
cd ../build
call build2.bat
if %errorlevel% neq 0 goto error

echo 构建完成！
goto end

:error
echo 构建失败！
pause
exit /b 1

:end
pause
```

## 增量构建策略

### 仅协议更新
```bash
cd bat
python build_rpc.py
cd ../build
build2.bat
```

### 仅错误码更新
```bash
cd bat
python build_err.py
cd ../build
build2.bat
```

### 仅数据库更新
```bash
cd DBTool
python genAll.py
cd ../build
build2.bat
```

## 热更新部署

### 热更新文件复制
```bash
cd bat
cp_hotifx.bat          # 复制热更新文件到部署位置
cd ../build
build2.bat             # 重新构建
```

### ActivitiesV01 热更新模块
- **位置**：`ActivitiesV01/` 目录
- **用途**：游戏内容更新和功能修复
- **特点**：可以覆盖基础功能，支持运行时更新

### 热更新最佳实践
1. **开发热更新内容**：在 `ActivitiesV01/` 等热更新模块中开发
2. **复制热更新文件**：运行 `cp_hotifx.bat`
3. **构建验证**：执行完整构建流程
4. **部署测试**：在测试环境验证功能

## 配置管理

### 环境配置
- **基础配置**：`conf/[ServerName]Conf.properties`
- **环境覆盖**：`customConf/[ServerName]Conf.properties`

### 构建时配置生成
```bash
cd bat/ConfGenerate
gen_conf.bat           # 生成基础配置
gen_custom_conf.bat    # 生成自定义配置
```

## 部署和打包

### JAR 文件输出
构建完成后，所有 JAR 文件输出到 `/Lib/` 目录：
- `Common.jar` - 通用类库
- `ServerProtocol.jar` - 协议类库
- `GameRes.jar` - 游戏资源类库
- `GameLogicCommon.jar` - 游戏逻辑通用类库
- 各服务器模块的 JAR 文件

### 部署模板
部署模板位于 `/build/template/` 目录，包含：
- 服务器启动脚本
- 配置文件模板
- 部署说明文档

## 常见问题和解决方案

### 1. 构建顺序错误
**问题**：跳过代码生成步骤直接执行 Ant 构建

**解决方案**：
```bash
# 始终按照完整顺序执行
cd bat && python build_enum.py && python build_err.py && python build_rpc.py
cd ../DBTool && python genAll.py
cd ../build && build2.bat
```

### 2. Python 环境问题
**问题**：Python 脚本执行失败

**解决方案**：
```bash
# 安装必要的 Python 依赖
cd bat
pip_install.bat

# 检查 Python 版本和路径
python --version
```

### 3. Ant 构建失败
**问题**：Java 编译错误或依赖问题

**解决方案**：
- 检查 Java 版本（要求 JDK 1.8+）
- 确保代码生成步骤成功完成
- 检查 Ant 配置和路径设置

### 4. 热更新部署问题
**问题**：热更新文件未正确复制

**解决方案**：
```bash
# 确保热更新脚本正确执行
cd bat
cp_hotifx.bat

# 检查目标目录文件是否更新
# 重新执行完整构建
```

---

**相关文档**：
- [协议生成系统](12_protocol_generation.md)
- [错误处理系统](13_error_handling.md)
- [热更新系统](15_hotfix_system.md)
