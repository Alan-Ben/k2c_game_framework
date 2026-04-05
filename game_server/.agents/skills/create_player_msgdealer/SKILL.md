---
name: create_player_msgdealer
description: 为已有协议类创建消息处理器（MsgDealer）。用户说"创建 MsgDealer"、"新建消息处理器"、"为协议添加处理器"时触发。前提是协议类已通过 protocol skill 定义并生成。Writer 部分由 create_protocol_writer skill 处理。
---

# 新建 MsgDealer Skill

## 适用场景
- 已有协议类（`.alpro` 已通过 `protocol` skill 定义并生成），需要创建对应的消息处理器

## 步骤

### 1. 解析协议类名
从用户输入中提取：
- 方向：`GC2GS`（客户端→服务器）或其他
- 包号：三位数字（如 `036`）
- 序号：三位数字（如 `001`）
- 功能名：驼峰英文（如 `ReqTreasureHuntOreCapture`）

### 2. 定位协议类和包目录
- 协议类路径：在 `ServerProtocol/src/` 下搜索该类，确认字段和包名
- MsgDealer 目录：`UserServer/src/NPUSServer/NPUserMsgDispather/p[包号]_[功能]Op/`
  - 若目录不存在则创建
- Writer 目录：`UserServer/src/NPUSServer/NPUserMsgDispather/Write/`

### 3. 参考现有 MsgDealer 风格
在 MsgDealer 目录中搜索已有的同包号处理器，确认 import 路径和代码风格。

### 4. 创建 MsgDealer 类
文件名：`MsgDealer_[方向]_[包号]_[序号]_[功能名].java`

```java
package NPUSServer.NPUserMsgDispather.p[包号]_[功能]Op;

/**
 * [功能中文描述] 消息处理器
 */
public class MsgDealer_[协议类名] extends NPUserMsgDealer<[协议类名]>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, [协议类名] _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData) return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.[对应事件]);

        // TODO: 调用对应 Component 方法

        // _commiter.commitSucRes(US2GCWriter_[包号]_[功能]Op.make_[序号]_[功能名](...));
    }
}
```

### 5. 验证
- 检查 MsgDealer 类名命名规范：`MsgDealer_[方向]_[包号]_[序号]_[功能名]`
- 服务器启动时自动注册，无需手动配置