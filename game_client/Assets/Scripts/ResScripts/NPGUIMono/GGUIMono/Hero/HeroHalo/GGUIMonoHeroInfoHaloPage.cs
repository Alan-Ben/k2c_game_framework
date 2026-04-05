using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴信息光环页签
    /// </summary>
    public class GGUIMonoHeroInfoHaloPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("效果总览按钮")]
        public GameObject btnDetail;
        [ALHeader("当前等级")]
        public Text txtCurLevel;
        [ALHeader("上个实力加成")]
        public Text txtLastPower;
        [ALHeader("当前实力加成")]
        public Text txtCurPower;
        [ALHeader("上个实力百分比")]
        public Text txtLastPowerPer;
        [ALHeader("当前实力百分比")]
        public Text txtCurPowerPer;
        [ALHeader("等级item列表")]
        public GGUIMonoHeroHaloLevelContainer monoHaloLevelContainer;
        [ALHeader("套系技能列表")]
        public GGUIMonoHeroHaloSuitSkillContainer monoSuitSkillContainer;
        [ALHeader("点亮按钮")]
        public GameObject btnActivate;
        [ALHeader("未点亮时需要显示的GO列表")]
        public List<GameObject> goNotActivateShowList;
        [ALHeader("未点亮时需要隐藏的GO列表")]
        public List<GameObject> goNotActivateHideList;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("已升级时需要显示的GO列表")]
        public List<GameObject> goAlreadyUpgradeShowList;
        [ALHeader("已升级时需要隐藏的GO列表")]
        public List<GameObject> goAlreadyUpgradeHideList;
        [ALHeader("升级按钮不可点击时需要显示的GO列表")]
        public List<GameObject> goCanNotUpgradeShowList;
        [ALHeader("升级按钮不可点击时需要隐藏的GO列表")]
        public List<GameObject> goCanNotUpgradeHideList;
        [ALHeader("升级按钮不可点击时需要置灰的列表")]
        public List<MaskableGraphic> canNotUpgradeGrayList;
        [ALHeader("已满级时需要显示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("已满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("没有套系技能列表时需要显示的GO列表")]
        public List<GameObject> goNotHaveSuitSkillShowList;
        [ALHeader("没有套系技能列表时需要隐藏的GO列表")]
        public List<GameObject> goNotHaveSuitSkillHideList;
        [ALHeader("激活与升级动画")]
        public CommonAnimationSingleInfo aniUpgrade;
        [ALHeader("点亮按钮红点")]
        public GameObject goActivateRedTip;
        [ALHeader("升级按钮红点")]
        public GameObject goUpgradeRedTip;
    }
}

