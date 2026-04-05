---
name: hotloader_generator
description: 为配置表生成 HotLoader 热更类时使用。用户说“创建 HotLoader”“配置表支持热更”“为某个 ref 表生成热更新代码”时触发。兼容原 Claude agent `hotloader-generator`。
---

# HotLoader 生成 Skill

## 适用场景
- 为新的配置表创建 HotLoader
- 为已有配置表补充指定字段的热更新逻辑
- 按项目既有模式实现热更校验、日志和错误处理

## 先做的事
1. 读取 `NPGameRes/UsHotRefDataMgr/HotLoaders/CLAUDE.md`
2. 搜索 `NPGameRes/UsHotRefDataMgr/HotLoaders/` 下的现有实现
3. 找到与目标表最接近的 HotLoader 作为参照

## 执行步骤
1. 根据表名推导对应 Ref 类、包路径和 HotLoader 命名。
2. 识别需要热更的字段及其类型，判断更新方式和校验逻辑。
3. 按现有模式生成完整 HotLoader 类，实现必要的抽象方法和字段更新逻辑。
4. 补充日志、异常处理、字段校验，并检查是否需要在管理器中注册。

## 编码要求
- 类名遵循 `HotLoader_[TableName]` 风格
- 使用 `import`，不要写全限定类名
- 注释使用中文
- 日志沿用项目现有约定，例如 `CommLog.info`
- 字段更新优先复用已有 setter/更新模式

## 注意事项
- 必须先看 HotLoaders 目录下的说明和样例，再生成代码
- 不要凭空假设热更框架基类，必须以仓库现有实现为准
- 如字段存在数组、嵌套对象、枚举等特殊结构，要先找同类实现再写
- 完成后提醒用户是否还需要注册、配置或联调验证
