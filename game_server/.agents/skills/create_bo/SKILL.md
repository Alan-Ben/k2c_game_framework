---
name: create_bo
description: 为新功能创建数据库表和对应的 BO 类（通过 Python 定义文件生成）。用户说"新建 BO"、"创建数据库表"、"新建 DB 表"、"创建 BO 类"、"需要持久化"时触发。
---

# 新建 BO 类（数据库表）Skill

## 适用场景
- 需要为新功能创建数据库表和对应的 BO 类

## 强制规则
- 数据库相关更改必须生成和更新 BO 类，不能只修改定义文件或手写维护生成产物

## 步骤

### 1. 确认表名和所属服务器
- 表名格式：全小写下划线（如 `player_hero_fight`）
- 所属服务器目录：`DBTool/source_db/[ServerName]/`
  - 玩家数据通常在 `UserServer/`

### 2. 创建 Python 定义文件
路径：`DBTool/source_db/[ServerName]/[表名].py`

```python
tableComment = "[表中文描述]"
field = [
    ["long", "cid", "玩家CID"],
    ["int", "field_name", "字段描述"],
    ["long", "time_ms", "时间戳（毫秒）"],
    ["bool", "is_done", "是否完成"],
    ["string", "extra", "扩展数据"],
    # ["text", "data_json", "JSON数据"],       # 长文本
    # ["bytes", "data_bytes", "二进制数据"],    # 二进制
]
key = ["cid"]     # 普通索引键（大多数情况只需 cid）
ukey = []         # 唯一键（除非必要否则不加）
dbTag = "main"    # 数据库标签：main/log
```

字段类型对照：
| Python类型 | Java类型 | SQL类型 |
|------------|----------|---------|
| int | int | int(11) |
| long | long | bigint(20) |
| bool | boolean | tinyint(1) |
| string | String | varchar(500) |
| text | String | text |
| bytes | byte[] | blob |

### 3. 生成 BO 类
```bash
cd DBTool && python genAll.py
```

### 4. 确认生成产物
BO 类包：`USDB.Bo`（搜索类名确认，如 `PlayerActivityFundBO`）

### 5. 标准使用模式

**创建并插入**：
```java
[TableName]BO bo = new [TableName]BO();
bo.setCid(getBM(), getUserData().getCid());
bo.setFieldName(getBM(), value);
bo.insert(getBM());
long dbId = bo.getId(); // 缓存主键ID
```

**增量更新（推荐）**：
```java
ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
updateValue.addValueObj("field_name", newValue);
getBM().getBM([TableName]BO.class).update("id", _m_dbId, updateValue);
```

**查询**：
```java
getUSServer().getBM().getBM([TableName]BO.class).findAll(
    "cid", getUserData().getCid(),
    new _ASelectCallback<List<[TableName]BO>>() {
        @Override
        public void dealSuc(List<[TableName]BO> _boList) {
            // 处理结果
        }
        @Override
        public void dealFail() {
            // 处理失败
        }
    }
);
```

## 注意事项
- 优先使用 `ALMySqlUpdateValue` 增量更新，避免全字段更新
- `key = ["cid"]` 足够大部分场景，不要过度加索引
- 不要直接暴露 BO 对象，推荐用数据对象层封装业务逻辑
- 首次插入时缓存 `bo.getId()` 作为后续更新的主键
