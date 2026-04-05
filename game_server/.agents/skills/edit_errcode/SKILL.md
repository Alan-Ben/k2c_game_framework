---
name: edit_errcode
description: 维护错误码定义与生成产物（XxxErr）。用户说"新建错误码"、"添加错误码"、"创建 XxxErr"、"编辑错误码"、"修改错误码"、"调整 XxxErr"、"需要错误码"时触发。适用于新建错误码模块，以及对已有错误码进行增删改查类维护的场景。
---

# 错误码维护 Skill

## 适用场景
- 需要为新功能模块定义一组错误码
- 需要为已有 `XxxErr` 补充、修改、废弃错误码项
- 需要调整错误码名称、中文描述、生成产物或归属文件

## 步骤

### 1. 先判断是“新建”还是“编辑”
- 新建：当前功能还没有对应的 `XxxErr` 定义文件
- 编辑：`bat/err/` 下已存在对应错误码定义文件，本次只是在原文件中调整内容

### 2. 新建错误码文件时，确认功能名和文件命名
- 查看已有序号：`ls bat/err/` 找最大序号
- 新序号 = 最大序号 + 1（三位补零，如 `062`）
- 文件名格式：`[序号]_[类名]Err_[中文描述].txt`
  - 示例：`062_HeroFightErr_英雄战斗错误.txt`

### 3. 新建或编辑错误码定义文件
路径：`bat/err/[序号]_[类名]Err_[描述].txt`

文件内容格式：
```
java_path= ../Common/src
java_package=NPCommon.ErrMain
csharp_type = hotfix
id	name	desc
1	REF_NOT_FOUND	配表找不到
2	NOT_ENOUGH	数量不足
3	ALREADY_DONE	已经完成
```

规则：
- `id` 从 1 开始递增
- `name` 全大写，下划线分隔
- 首项固定为 `NONE`（id=0）可省略，生成器会自动处理
- `csharp_type`：普通功能用 `hotfix`，核心通用用 `main`

编辑已有文件时额外要求：
- 保持原文件序号和类名不变，不要因为补充错误码而新建重复 `XxxErr`
- 新增错误码时，优先在原文件末尾追加，避免重排已有 `id`
- 修改已有错误码时，优先做最小变更，只调整目标项
- 已上线错误码不要随意复用或改成其他语义，避免历史错误码含义漂移
- 如确需删除错误码项，先确认没有业务引用和兼容性风险；无特殊必要时更推荐保留旧项并停止使用

### 4. 生成错误码类
```bash
cd bat && python build_err.py
```

### 5. 确认生成产物
- Java 类：`Common/src/NPCommon/ErrMain/[类名]Err.java`
- C# 类：`ClientProtocol/ResultRegisters/[类名]Err.cs`
- 如果是编辑已有错误码，还要确认对应常量、错误码值和描述已同步更新

### 6. 使用方式
- 直接返回错误码（不要用 `Result.failed()` 包装）：`return HeroFightErr.REF_NOT_FOUND;`
- `ResultOne` 场景：`return ResultOne.failed(HeroFightErr.NOT_ENOUGH);`

## 注意事项
- 任何对错误码定义文件或生成产物的编辑，都应使用本 Skill 的流程处理
- 先改 `bat/err/` 下的定义文件，不要直接手改生成后的 Java/C# 错误码类
- 修改完成后必须重新执行 `build_err.py`，并确认生成产物已更新

## 错误码计算规则
最终错误码 = 文件序号 × 10000 + 子错误码 id
- 示例：`062_HeroFightErr` 的 `REF_NOT_FOUND(1)` = 62 × 10000 + 1 = 620001