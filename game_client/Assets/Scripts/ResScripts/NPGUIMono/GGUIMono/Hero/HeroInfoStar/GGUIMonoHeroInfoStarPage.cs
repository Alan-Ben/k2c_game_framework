using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUIHeroStarPageButtonState
    {
        [ALHeader("可升级时的颜色")]
        public Color canUpgradeColor = Color.white;
        [ALHeader("不可升级时的颜色")]
        public Color canNotUpgradeColor = Color.white;
        [ALHeader("需要设置颜色的控件列表")]
        public List<MaskableGraphic> targetList;

        /// <summary>
        /// 根据是否可以点击设置颜色
        /// </summary>
        /// <param name="_canUpgrade"></param>
        public void setState(bool _canUpgrade)
        {
            foreach (MaskableGraphic item in targetList)
            {
                ALUGUICommon.setUIObjColor(item, _canUpgrade ? canUpgradeColor : canNotUpgradeColor);
            }
        }
    }

    /// <summary>
    /// 伙伴信息觉醒页签
    /// </summary>
    public class GGUIMonoHeroInfoStarPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("星级")]
        public GGUIMonoHeroCommonStar monoStar;
        [ALHeader("效果加成值")]
        public Text txtAddValue;
        [ALHeader("效果加成百分比")]
        public Text txtAddValuePer;
        [ALHeader("火星实力加成百分比")]
        public Text txtMarsTeamPowerAddPer;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗道具")]
        public NPGGUIMonoCommonItem monoUpgradeCostItem;
        [ALHeader("升星条件描述")]
        public Text txtUpgradeConditionDesc;
        [ALHeader("觉醒技能列表")]
        public GGUIMonoHeroStarSkillContainer monoStarSkillContainer;
        [ALHeader("资质技能列表")]
        public GGUIMonoHeroTalentSkillContainer monoTalentSkillContainer;
        [ALHeader("默认不可升星状态需要显示的GO列表")]
        public List<GameObject> goCanNotUpgradeShowList;
        [ALHeader("默认不可升星状态需要隐藏的GO列表")]
        public List<GameObject> goCanNotUpgradeHideList;
        [ALHeader("满星时需要显示的GO列表")]
        public List<GameObject> goMaxStarShowList;
        [ALHeader("满星时需要隐藏的GO列表")]
        public List<GameObject> goMaxStarHideList;
        [ALHeader("升星条件未通过时条件描述参数颜色")]
        public Color conditionNotPassDescColor = Color.red;
        [ALHeader("按钮底图 条件是否满足时显示颜色配置")]
        public GGUIHeroStarPageButtonState monoButtonColorState;
        [ALHeader("按钮文本 条件是否满足时显示颜色配置")]
        public GGUIHeroStarPageButtonState monoButtonTextColorState;
        [ALHeader("按钮可升级红点")]
        public GameObject goUpgradeReTip;
    }
}

