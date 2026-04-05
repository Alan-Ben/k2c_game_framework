---
name: create_protocol_writer
description: 为响应协议创建或更新 Writer 工具类，包括玩家回包（US2GCWriter）和服务器间回包（NP2XX_RB_Writer）。用户说"新建 Writer"、"创建 Writer"、"添加 Writer 方法"时触发。通常在 protocol skill 完成协议定义后，配合 create_player_msgdealer 或 create_server_protocol 执行，或单独为某包号追加新的 make 方法。
---

# 新建/更新 Writer 类 Skill

## 适用场景
- 为响应协议创建 Writer 工具类，或向已有 Writer 追加新的 make 方法
- 两种类型：玩家回包（US2GC）、服务器间回包（NP2XX_RB）

---

## 类型一：玩家回包 Writer（US2GCWriter）

### 文件位置
`UserServer/src/NPUSServer/NPUserMsgDispather/Write/US2GCWriter_[包号]_[功能]Op.java`

### 确认响应协议字段
在 `ServerProtocol/src/GS2GC/p[包号]_[功能]Op/` 下找到响应协议类，确认字段名和类型。

### 新建文件模板
```java
package NPUSServer.NPUserMsgDispather.Write;

/** [功能描述] 响应 Writer */
public class US2GCWriter_[包号]_[功能]Op
{
    /** 构造 [序号] [功能描述] 响应 */
    public static [响应协议类名] make_[序号]_[功能名](/* 参数列表 */)
    {
        [响应协议类名] ret = new [响应协议类名]();
        ret.setResult(CommErr.getSucc());
        // 设置其他字段
        return ret;
    }
}
```

---

## 类型二：服务器间回包 Writer（NP2XX_RB_Writer）

### 文件位置
`Common/src/NPServerProtocolWriter/NP2[目标]/RequestBack/NP2[目标]_RB_Writer_[包号]_[功能]Op.java`

### 确认响应协议字段
在 `ServerProtocol/src/NP2[目标]_RB/p[包号]_[功能]Op/` 下找到响应协议类，确认字段名和类型。

### 新建文件模板
```java
package NPServerProtocolWriter.NP2[目标].RequestBack;

/** [功能描述] 响应 Writer */
public class NP2[目标]_RB_Writer_[包号]_[功能]Op
{
    /** 构造 [序号] [功能描述] 响应 */
    public static NP2[目标]_RB_[包号]_[序号]_[功能名] make_[序号]_[功能名](/* 参数列表 */)
    {
        NP2[目标]_RB_[包号]_[序号]_[功能名] proto = new NP2[目标]_RB_[包号]_[序号]_[功能名]();
        // 设置字段（若协议有 result 字段则设置 CommErr.getSucc()）
        return proto;
    }
}
```

---

## 向已有 Writer 追加 make 方法
在类的末尾 `}` 之前插入新方法，格式同上。

## 注意事项
- 方法命名：`make_[序号]_[功能名]`，序号与协议序号对应
- 每个 make 方法只负责构造协议对象，不含业务逻辑
- US2GC 协议通常有 `result` 字段；RB 协议视协议定义而定
- 所有注释使用中文，4 个空格缩进
