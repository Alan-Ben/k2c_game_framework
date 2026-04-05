using ALPackage;
using NPEnum;
using UnityEngine;

/// <summary>
/// 品质额外表
/// </summary>
[System.Serializable]
public class NPQualityExtRefObj : _IALBasicRefObj
{
    public long _refId { get { return (long)quality; } }

    public EQuality quality;//品质
    public long item_sfx_id;//获得奖励item品质特效
    public NPGTextureIndex hero_card_bg;//伙伴卡牌背景图
    public NPGTextureIndex hero_name_bg;//伙伴名称背景图
    public NPGSpriteIndex hero_head_bg;//伙伴头像背景框（圆形）
    public int hero_skin_star;//伙伴皮肤对应星级数
    public NPGGoIndex quality_go_index;//通用含特效的品质GO
    public NPGTextureIndex consort_name_bg;//妃子名称背景框
    public NPGTextureIndex consort_card_bg;//妃子卡片背景框
    public NPGSpriteIndex consort_head_bg;//妃子头像背景框（圆形）
    public int consort_skin_star;//妃子皮肤对应星级数
    public NPGTextureIndex shop_item_bg;//商店商品背景图
    public NPGSpriteIndex equip_card_bg;//藏品卡片背景图
    public NPGSpriteIndex treasure_hunt_ore_card_bg;//太空寻宝矿石品质背景图	
    public NPGSpriteIndex treasure_hunt_treasure_card_bg;//太空寻宝奇物品质背景图
    public NPGTextureIndex museum_item_bg; //博物馆藏品背景图
    public NPGTextureIndex vip_tab_bg;//VIP页签背景图
    public NPGTextureIndex mars_explore_event_quality_icon;//火星探索事件品质图标
}


public class NPGSOQualityExtRefSet : _TALSOBasicRefSet<NPQualityExtRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "quality_ext"; } }
}
