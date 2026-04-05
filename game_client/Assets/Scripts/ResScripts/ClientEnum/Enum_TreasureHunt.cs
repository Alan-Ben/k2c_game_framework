using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石状态
    /// </summary>
    public enum ETreasureHuntOreState
    {
        NONE,
        [InspectorName("未获得")]
        NOT_GET,
        [InspectorName("已获得且可激活 普通矿石状态")]
        NOT_ACTIVATE_NORMAL,
        [InspectorName("已获得且已激活 普通矿石状态")]
        ACTIVATED_NORMAL,
        [InspectorName("已获得且可激活 高级矿石状态")]
        NOT_ACTIVATE_ADVANCED,
        [InspectorName("已获得且已激活 高级矿石状态")]
        ACTIVATED_ADVANCED,
    }
    
    /// <summary>
    /// 奇物状态
    /// </summary>
    public enum ETreasureHuntTreasureState
    {
        NONE,
        [InspectorName("未获得")]
        NOT_GET,
        [InspectorName("已获得可激活")]
        GOT_NOT_ACTIVATE,
        [InspectorName("已获得已激活")]
        GOT_ACTIVATED,
    }

    /// <summary>
    /// 技能状态
    /// </summary>
    public enum ETreasureHuntSkillState
    {
        NONE,
        [InspectorName("不可激活 状态")]
        LOCK,
        [InspectorName("可激活 状态")]
        UNLOCK_NOT_ACTIVATE,
        [InspectorName("已激活_未满级 状态")]
        UNLOCK_ACTIVATE,
        [InspectorName("已激活_已满级 状态")]
        UNLOCK_ACTIVATE_MAX_LEVEL,
    }

    /// <summary>
    /// 组合图鉴状态
    /// </summary>
    public enum ETreasureHuntCompositeCatalogState
    {
        NONE,
        [InspectorName("未拥有")]//存在未获得的普通矿石
        NOT_GET,
        [InspectorName("拥有普通组合图鉴")]//已获取组合图鉴相关的全部普通矿石(只要获取到就行, 对应矿石的技能激不激活都行), 但未获取到所有高级矿石
        GOT_NORMAL,
        [InspectorName("拥有高级组合图鉴")]//已获取组合图鉴相关的全部高级矿石(只要获取到就行, , 对应矿石的技能激不激活都行)
        GOT_ADVANCED,
    }

    /// <summary>
    /// 游玩模式
    /// </summary>
    public enum ETreasureHuntGamePlayMode
    {
        [InspectorName("正常玩")]
        NORMAL,
        [InspectorName("一键单次游玩")]
        AKEY_ONCE_PLAY,
        [InspectorName("一键多次游玩")]
        AKEY_MULTI_PLAY,
    }

    /// <summary>
    /// 可放入实验室的物品类型
    /// </summary>
    public enum ETreasureHuntCanPutInLabThingsType
    {
        [InspectorName("奇物")]
        TREASURE,
    }

    /// <summary>
    /// 可放入实验室的物体状态
    /// </summary>
    public enum ETreasureHuntCanPutInLabThingsState
    {
        [InspectorName("未获取")]
        NOT_GET,
        [InspectorName("已获取未放入")]
        GET_NOT_PUT_IN,
        [InspectorName("已获取已放入")]
        GET_PUT_IN,
    }
}