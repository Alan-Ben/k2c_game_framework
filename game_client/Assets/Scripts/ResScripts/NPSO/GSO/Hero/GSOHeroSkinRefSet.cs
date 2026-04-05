using ALPackage;
using System;
using System.Collections.Generic;

/// <summary>
/// 伙伴皮肤表
/// </summary>
[Serializable]
public class HeroSkinRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一识别标记
    public long hero_id;//伙伴id
    public bool is_hide;//皮肤是否隐藏
    public long sort_v;//排序值(按照由小到大排序)
    public List<long> unlock_gain_talent_skill_id_list;//解锁时获得的资质技能id列表
    public List<NPCommonCostItem> unlock_gain_item_list;//解锁时获得的道具列表
    public NPCommonCostItem unlock_item;//解锁道具（CommonCostItem）
    public NPGTextureIndex card_image;//卡牌半身像
    public NPGGoIndex td_show;//全身形象
    public NPGGoIndex td_bg_index;//伙伴形象背景GO
}

public class GSOHeroSkinRefSet : _TALSOBasicRefSet<HeroSkinRefObj> 
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/hero_refdata.unity3d"; } }
    public static string objName { get { return "hero_skin"; } }
}
