using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EHeroMainTargetHeroType
    {
        NONE,
        [InspectorName("等级最低的伙伴(作用于已解锁大臣)")]
        LEVEL_MIN_HERO,
        [InspectorName("经营技能等级最低的伙伴(作用于已解锁大臣)")]
        BUSINESS_SKILL_LEVEL_MIN_HERO,
        [InspectorName("未佩戴藏品的伙伴(作用于已解锁大臣)")]
        NO_WEAR_EQUIP_HERO,
    }
    
    /// <summary>
    /// 伙伴主界面
    /// </summary>
    public class GGUIMonoHeroMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("伙伴列表")]
        public GGUIMonoHeroListGrid monoHeroGrid;
        [ALHeader("伙伴已获得数量文本")]
        public Text txtNum;
        [ALHeader("排序子窗口")]
        public GGUIMonoHeroMainSort monoSort;
        [ALHeader("筛选子窗口")]
        public GGUIMonoHeroMainFilter monoFilter;
        [ALHeader("实力前几个顾问需要特殊展示红点")]
        public long specShowRedTipCount = 5;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1000); } }
    }
}