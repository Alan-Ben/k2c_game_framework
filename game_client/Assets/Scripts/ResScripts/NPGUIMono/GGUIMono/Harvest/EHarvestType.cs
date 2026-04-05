using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 资源收集目标对象枚举
    /// </summary>
    public enum EHarvestType
    {
        NONE, //0 ==== 
        [InspectorName("DEFAULT（默认产出，获取不到其他类型的时候获取本类型）")]
        DEFAULT, //1 ==== 默认产出，获取不到其他类型的时候获取本类型
        [InspectorName("GEM（钻石）")]
        GEM,//2 ==== 钻石
        [InspectorName("SILVER（银币）")]
        SILVER, //3 ==== 银币
        [InspectorName("FOOD（食物）")]
        FOOD, //4 ==== 食物
        [InspectorName("SOLDIER（士兵）")]
        SOLDIER,//5 ==== 士兵
        [InspectorName("P_EXP（玩家经验）")]
        P_EXP, //6 ==== 玩家经验
        [InspectorName("DINNER_COIN（宴会币）")]
        DINNER_COIN,//7 ==== 宴会币
        [InspectorName("DINNER_SCORE（宴会分数）")]
        DINNER_SCORE,//8 ==== 宴会分数
        [InspectorName("DAILY_QUEST_ACTIVE_POINT（日常任务积分）")]
        DAILY_QUEST_ACTIVE_POINT, //9 ==== 日常任务积分
        [InspectorName("ACHIEVE_POINT（成就点）")]
        ACHIEVE_POINT, //10 ==== 成就点
        [InspectorName("CHAPTER_SUSPEND_EVENT（关卡挂起事件）")]
        CHAPTER_SUSPEND_EVENT,//11 ==== 关卡挂起事件
        [InspectorName("BAG_ITEM（背包道具）")]
        BAG_ITEM,//12 ==== 背包道具
        [InspectorName("HERO_EXP（英雄经验）")]
        HERO_EXP, //13 ==== 英雄经验
        [InspectorName("TOWER_COIN（迷宫币）")]
        TOWER_COIN, //14 ==== 迷宫币
        [InspectorName("GUILD_COIN（联盟币）")]
        GUILD_COIN, //15 ==== 联盟币
        [InspectorName("DUNGEON_COIN（副本币）")]
        DUNGEON_COIN, //16 ==== 副本币
        [InspectorName("MARS_ENERGY（火星能量）")]
        MARS_ENERGY, //17 ==== 火星能量
        [InspectorName("GUILD_BOX_ACTIVE_POINT（联盟宝箱活跃点）")]
        GUILD_BOX_ACTIVE_POINT, //18 ==== 联盟宝箱活跃点
        [InspectorName("INN_GUEST（旅店客人）")]
        INN_GUEST,
        [InspectorName("INN_FINESSE（旅店熟练度）")]
        INN_FINESSE,
        [InspectorName("BATTLE_PASS_COIN（战令币）")]
        BATTLE_PASS_COIN, //战令币
    }
}