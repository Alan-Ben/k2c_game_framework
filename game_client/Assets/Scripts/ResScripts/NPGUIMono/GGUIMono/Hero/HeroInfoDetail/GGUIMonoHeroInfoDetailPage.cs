using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴信息页签
    /// </summary>
    public class GGUIMonoHeroInfoDetailPage : _AALBasicUIWndMono
    {
        [ALHeader("升级按钮")]
        public GameObject btnLvlUpgrade;
        [ALHeader("升级消耗")]
        public NPGGUIMonoCommonItem monoLevelUpCostItem;
        [ALHeader("升阶按钮")]
        public GameObject btnStepUpgrade;
        [ALHeader("连升十级勾选框")]
        public NPGGUIMonoCommonToggleEx toggleTenLevelUp;
        [ALHeader("加护按钮")]
        public GameObject btnBless;
        [ALHeader("皮肤按钮")]
        public GameObject btnSkin;
        [ALHeader("藏品按钮")]
        public GameObject btnEquip;
        [ALHeader("属性信息")]
        public GGUIMonoHeroPowerLevelInfo monoPowerLevelInfo;
        [ALHeader("可以连升十级时显示的GO列表")]
        public List<GameObject> goTenLevelUpShowList;
        [ALHeader("可以连升十级时隐藏的GO列表")]
        public List<GameObject> goTenLevelUpHideList;
        [ALHeader("可升阶显示的go列表")]
        public List<GameObject> goStepUpgradeShow;
        [ALHeader("可升阶隐藏的go列表")]
        public List<GameObject> goStepUpgradeHide;
        [ALHeader("满级显示的go列表")]
        public List<GameObject> goLvLimitShow;
        [ALHeader("满级隐藏的go列表")]
        public List<GameObject> goLvLimitHide;
        [ALHeader("升级红点")]
        public GameObject goUpgradeRedTip;
        [ALHeader("升阶红点")]
        public GameObject goStepUpRedTip;

        [ALInfo("====藏品按钮相关配置====")]
        [ALHeader("藏品入口按钮item")]
        public GGUIMonoHeroEquipBtnItem monoEquipBtnItem;

        [ALInfo("====皮肤按钮相关配置====")]
        [ALHeader("只有默认皮肤时需要隐藏的GO列表")]
        public List<GameObject> goNoSkinHideList;
        [ALHeader("皮肤入口按钮item")]
        public GGUIMonoHeroSkinBtnItem monoSkinBtn;
    }
}