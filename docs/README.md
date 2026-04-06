# K2C 游戏框架开发文档

## 项目概述

K2C 是一款 Unity 客户端 + Java 服务端的游戏项目框架，采用组件化架构设计。

### 技术栈
- **客户端**: Unity 2021.x + C# + ILRuntime热更
- **服务端**: Java + 自研服务器框架
- **通信协议**: 自定义二进制协议

---

## 目录结构

```
k2c_game_framework/
├── game_client/                    # Unity客户端
│   └── Assets/Scripts/
│       ├── LogicScripts/           # 核心逻辑代码
│       │   ├── GameComponent/      # 玩家数据组件模块
│       │   ├── GameWndGui/         # UI窗口模块
│       │   ├── GameSystem/         # 游戏系统
│       │   ├── GameScene/          # 场景管理
│       │   └── ...
│       ├── ALPackage/              # 协议包
│       └── ClientProtocol/         # 客户端协议定义
│
├── game_server/                    # Java服务端
│   ├── ClientProtocol/             # 客户端通信协议
│   │   ├── GC2GS/                  # 客户端->服务端请求
│   │   └── GS2GC/                  # 服务端->客户端响应
│   ├── GameServer/                 # 游戏服务器
│   ├── GatewayServer/              # 网关服务器
│   ├── LoginServer/                # 登录服务器
│   └── ...
│
└── docs/                           # 开发文档目录
    ├── Module_Player/              # 玩家基础模块
    ├── Module_Hero/                # 英雄模块
    ├── Module_Quest/               # 任务模块
    └── ...                         # 其他模块
```

---

## 模块总览

### 核心模块 (协议编号 p002-p011)

| 模块编号 | 模块名称 | 客户端组件 | 说明 |
|---------|---------|-----------|------|
| p002 | InitOp | - | 初始化操作 |
| p003 | FristTeamActivityOp | CommonActivity | 首期组队活动 |
| p004 | PlayerOp | NPPlayerInfo | 玩家基础操作 |
| p006 | BagItemOp | NPBag | 背包物品 |
| p007 | CommOp | - | 通用操作 |
| p008 | TravelOp | Travel | 游历系统 |
| p009 | MailOp | NPMail | 邮件系统 |
| p010 | BuildingOp | Building | 建筑系统 |
| p011 | ClientDataOp | NPClientData | 客户端数据 |

### 玩法模块 (协议编号 p012-p024)

| 模块编号 | 模块名称 | 客户端组件 | 说明 |
|---------|---------|-----------|------|
| p012 | ActivityTeamOp | CommonActivity | 组队活动 |
| p013 | HeroOp | Hero | 英雄/大臣系统 |
| p014 | ChildOp | Child | 子女系统 |
| p015 | ConsortOp | Consort | 伴侣/情人系统 |
| p016 | ChapterOp | Chapter | 章节系统 |
| p017 | ActivityOp | CommonActivity | 活动系统 |
| p018 | PlayerSkinOp | PlayerSkin | 玩家皮肤 |
| p019 | DinnerOp | Dinner | 宴会系统 |
| p021 | PlayerInfo | NPPlayerInfo | 玩家信息 |
| p022 | ChatOp | NPChat | 聊天系统 |
| p023 | ArenaOp | Arena | 竞技场 |
| p024 | DungeonOp | Tower/MiddayDungeon/EveningDungeon | 副本系统 |

### 功能模块 (协议编号 p028-p042)

| 模块编号 | 模块名称 | 客户端组件 | 说明 |
|---------|---------|-----------|------|
| p028 | QuestOp | Quest | 任务系统 |
| p030 | ShopOp | NPShop | 商店系统 |
| p031 | RankOp | NPRankCommon | 排行榜 |
| p032 | GuildOp | Guild | 公会系统 |
| p033 | SimpleActivityOp | CommonActivity | 简单活动 |
| p034 | InnOp | Inn | 旅店系统 |
| p035 | MuseumOp | Museum | 博物馆/藏品 |
| p036 | TreasureHuntOp | TreasureHunt | 太空寻宝 |
| p037 | GuildDungeonOp | GuildDungeon | 公会副本 |
| p038 | MarsOp | Mars | 火星系统 |
| p039 | MarsBuildingOp | Mars | 火星建筑 |
| p040 | MarsPeopleOp | Mars | 火星人员 |
| p041 | MarsExploreOp | Mars | 火星探索 |
| p042 | GuildRelatedOp | Guild | 公会相关 |

### 客户端独立组件 (无对应协议模块)

| 组件名称 | 说明 |
|---------|------|
| Achieve | 成就系统 |
| Anecdote | 轶事/奇遇系统 |
| Bonus | 奖励系统 |
| CuteActor | Q版形象 |
| FuncUnlock | 功能解锁 |
| Marquee | 跑马灯 |
| NPPlayerBuff | 玩家Buff |
| NPPlayerIcon | 玩家头像 |
| NPPlayerIconBgk | 玩家头像框 |
| NPPlayerBubble | 玩家气泡框 |
| NPFixedCD | 固定CD |
| NPLazyCD | 延迟CD |
| NPRecord | 玩家记录 |
| NPResource | 玩家资源 |
| NPTutorial | 引导系统 |
| PlayerForeverAdd | 永久属性加成 |
| PlayerPermissions | 玩家权限 |
| PlayerTitle | 玩家称号 |
| PrivilegeCard | 权益卡 |
| PushGift | 推送礼包 |
| RedDot | 红点系统 |
| RechargeRebate | 充值返利 |
| Recruit | 招募系统 |
| RushExchange | 限时兑换 |
| SevenDayGoals | 七日目标 |
| SevenDayLogin | 七日登录 |
| SpecialItem | 特殊物品 |
| StageGoal | 阶段目标 |
| WeekCard | 周卡 |

---

## 文档规范

每个模块包含以下文档:

1. **数据配表文档** (`01_数据配表.md`)
   - Excel/JSON 配置文件分析
   - 字段定义和用途

2. **协议文档** (`02_协议文档.md`)
   - 客户端请求协议 (GC2GS)
   - 服务端响应协议 (GS2GC)
   - 消息字段定义

3. **客户端开发文档** (`03_客户端开发.md`)
   - Component 组件数据层
   - UI窗口开发
   - 场景管理

4. **服务端开发文档** (`04_服务端开发.md`)
   - 模块架构
   - 数据存储
   - 业务逻辑

---

## 快速导航

### 按功能分类

- [玩家系统](./Module_Player/) - 玩家基础数据、资源、属性
- [英雄系统](./Module_Hero/) - 英雄管理、装备、技能
- [任务系统](./Module_Quest/) - 主线任务、日常任务
- [背包系统](./Module_Bag/) - 物品管理、使用
- [公会系统](./Module_Guild/) - 公会管理、公会战
- [建筑系统](./Module_Building/) - 建筑建造、升级
- [章节系统](./Module_Chapter/) - 关卡推进
- [活动系统](./Module_Activity/) - 各类活动玩法
- [商店系统](./Module_Shop/) - 商店购买
- [聊天系统](./Module_Chat/) - 聊天频道

---

*文档生成时间: 2026-04-05*
