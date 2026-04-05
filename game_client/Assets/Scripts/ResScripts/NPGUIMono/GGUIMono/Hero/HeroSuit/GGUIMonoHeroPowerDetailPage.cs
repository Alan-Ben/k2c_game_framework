using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴战力详情页签
    /// </summary>
    public class GGUIMonoHeroPowerDetailPage : _AALBasicUIWndMono
    {
        [ALHeader("简介")]
        public Text txtIntroduction;

        [ALInfo("====实力====")]
        [ALHeader("总实力")]
        public Text txtTotalPower;
        [ALHeader("基础实力")]
        public Text txtBasePower;
        [ALHeader("星级实力百分比")]
        public Text txtStarPowerPer;
        [ALHeader("家人实力百分比")]
        public Text txtConsortPowerPer;
        [ALHeader("藏品实力百分比")]
        public Text txtEquipPowerPer;
        [ALHeader("星辉实力百分比")]
        public Text txtHaloPowerPer;
        [ALHeader("太空寻宝实力百分比")]
        public Text txtTreasureHuntPowerPer;
        [ALHeader("星级实力")]
        public Text txtStarPower;
        [ALHeader("家人实力")]
        public Text txtConsortPower;
        [ALHeader("道具实力")]
        public Text txtBagItemPower;
        [ALHeader("游历实力")]
        public Text txtTravelPower;
        [ALHeader("谈判实力")]
        public Text txtArenaPower;
        [ALHeader("星辉实力")]
        public Text txtHaloPower;
        [ALHeader("太空寻宝实力")]
        public Text txtTreasureHuntPower;

        [ALInfo("====资质====")]
        [ALHeader("总资质")]
        public Text txtTotalTalent;
        [ALHeader("基础资质")]
        public Text txtBaseTalent;
        [ALHeader("技能资质")]
        public Text txtSkillTalent;
        [ALHeader("进阶资质")]
        public Text txtStepUpTalent;
        [ALHeader("藏品资质")]
        public Text txtEquipTalent;
        [ALHeader("家人资质")]
        public Text txtConsortTalent;
        [ALHeader("服装资质")]
        public Text txtSkinTalent;
        [ALHeader("太空寻宝资质")]
        public Text txtTreasureHuntTalent;

        [ALInfo("====等级上限====")]
        [ALHeader("总等级上限")]
        public Text txtTotalLevelLimit;
        [ALHeader("基础等级上限")]
        public Text txtBaseLevelLimit;
    }
}

