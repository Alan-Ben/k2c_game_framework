using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴觉醒升星确认界面
    /// </summary>
    public class GGUIMonoHeroStarUpgradeCheck : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("当前星级")]
        public GGUIMonoHeroCommonStar monoCurStar;
        [ALHeader("下个星级")]
        public GGUIMonoHeroCommonStar monoNextStar;
        [ALHeader("当前基础值")]
        public Text txtCurBaseValue;
        [ALHeader("下个基础值")]
        public Text txtNextBaseValue;
        [ALHeader("当前系数")]
        public Text txtCurPerValue;
        [ALHeader("下个系数")]
        public Text txtNextPerValue;
        [ALHeader("当前火星实力加成")]
        public Text txtCurMarsTeamPowerAddPerValue;
        [ALHeader("下个火星实力加成")]
        public Text txtNextMarsTeamPowerAddPerValue;
        [ALHeader("觉醒技能列表")]
        public GGUIMonoHeroStarSkillContainer monoStarSkillContainer;
        [ALHeader("资质技能图标")]
        public RawImage imgTalentSkillIcon;
        [ALHeader("资质技能等级名称")]
        public Text txtTalentSkillLevelName;
        [ALHeader("资质技能描述")]
        public Text txtTalentSkillDesc;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗道具")]
        public NPGGUIMonoCommonItem monoUpgradeCostItem;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1011); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1011); } }
    }
}