---
name: create_player_component
description: 为玩家数据新增功能组件（继承 _ANPUserComponent）。用户说"新建组件"、"创建玩家组件"、"添加 Component"、"新增功能模块"时触发。通常需要配合 create_bo 创建数据表。
---

# 新建玩家组件 Skill

## 适用场景
- 需要为玩家新增功能组件（继承 `_ANPUserComponent`）

## 步骤

### 1. 确认组件名称
- 组件类名格式：`[功能名]Component`（如 `ActivityFundComponent`）
- 枚举值格式：全大写下划线（如 `ACTIVITY_FUND`）

### 2. 在枚举中注册组件类型
`ENPPlayerCompType` 位于 `NPCommon.Enum.NPCommonEnum`（搜索 `ENPPlayerCompType` 定位文件）
- 在已有枚举末尾追加新枚举值：`[功能名大写]`

### 3. 在 NPUSUserData 注册组件
搜索 `NPUSUserData` 定位文件（路径为 `UserServer/src/NPUSServer/NPUSUserMgr/NPUSUserData.java`）
需要在三处添加（搜索相邻组件定位行号）：

**① 成员变量声明区（~300行附近）**：
```java
// [功能中文描述]组件
private [功能名]Component _m_[功能名首字母小写]Comp;
```

**② 构造函数初始化区（~400行附近）**：
```java
_m_[功能名首字母小写]Comp = new [功能名]Component(this);
```

**③ getter 方法区（~800行附近）**：
```java
public [功能名]Component get[功能名]Component() {
    return _m_[功能名首字母小写]Comp;
}
```

### 4. 创建组件类
目录：`UserServer/src/NPUSServer/NPUSUserMgr/UserComp/[功能名]/`
文件名：`[功能名]Component.java`

```java
package NPUSServer.NPUSUserMgr.UserComp.[功能名];

/**
 * [功能中文描述]组件
 *
 * 主要功能：
 * 1. [功能点一]
 */
public class [功能名]Component extends _ANPUserComponent
{
    public [功能名]Component(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.[枚举值]);
    }

    /** 组件依赖列表，返回 null 表示无依赖 */
    @Override
    public ENPPlayerCompType[] getDependCompList() { return null; }

    /** 组件初始化，异步加载数据库数据 */
    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("[功能名]_init");
        process.addResDelegateProcess(
                action -> _loadDataFromDB(action::dealAction), "load_data",
                () -> USLog.error(getUSServer(), "[功能名]Component._init - load data failed, cid={}", getUserData().getCid()),
                false);
        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "[功能名]Component._init - process stopped, cid={}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }
            @Override public void onRootProecssSuc() { setInited(); }
        });
    }

    private void _loadDataFromDB(_ICallBackBool _handler)
    {
        // TODO: 查询数据库
        _handler.onRunOver(true);
    }

    @Override public void onInited() { /* 可选：注册事件等 */ }
    @Override public void dispose() { /* 可选：清理监听器等 */ }
}
```

### 5. 验证
- 搜索相邻组件确认三处注册都已添加
- 组件构造函数传入枚举值与 `ENPPlayerCompType` 中新增的值一致
- 检查 import 无完全限定类路径

## 注意事项
- `protected`/`private` 方法名以下划线开头：`_init`、`_loadDataFromDB`
- 无依赖时 `getDependCompList()` 返回 `null`，不返回空数组
- `_IEZProcessMonitor` 从 `NPCommon.CommonProcess` 导入，不是 `NPCommon.Util.EZProcess`
- 所有注释使用中文
- 4 个空格缩进
