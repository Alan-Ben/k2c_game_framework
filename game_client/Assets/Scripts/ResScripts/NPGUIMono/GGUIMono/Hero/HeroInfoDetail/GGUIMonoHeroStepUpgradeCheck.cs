using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴升阶确认弹窗
    /// </summary>
    public class GGUIMonoHeroStepUpgradeCheck : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("升阶按钮")] 
        public GameObject btnStepUpgrade;
        [ALHeader("伙伴卡牌展示")]
        public GGUIMonoHeroCommonCardItem monoHeroCard;
        [ALHeader("当前等级上限")]
        public Text txtCurLevelLimit;
        [ALHeader("下一等级上限")]
        public Text txtNextLevelLimit;
        [ALHeader("当前资质")]
        public Text txtCurTalent;
        [ALHeader("下一资质")]
        public Text txtNextTalent;
        [ALHeader("升阶消耗")] 
        public NPGGUIMonoCommonItemContainer monoStepCostUpgrade;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1004); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1004); } }
    }
}