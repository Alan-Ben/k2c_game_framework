using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 骑士信息资质页签
    /// </summary>
    public class GGUIMonoHeroInfoTalentPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("总资质信息按钮")]
        public GameObject btnTalentInfo;
        [ALHeader("总资质")]
        public Text txtTotalTalent;
        [ALHeader("资质技能列表")]
        public GGUIMonoHeroTalentSkillContainer monoTalentSkillContainer;
        [ALHeader("资质名称")]
        public Text txtName;
        [ALHeader("资质描述")]
        public Text txtDesc;
        [ALHeader("使用升级道具名")]
        public Text txtUseItemName;
        [ALHeader("升级消耗道具")]
        public NPGGUIMonoCommonItem monoUpgradeCostItem;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("连升十级勾选框")]
        public NPGGUIMonoCommonToggleEx toggleTenLevelUp;
        [ALHeader("未解锁或升级条件提示文本")]
        public Text txtLockOrCanNotUpgradeDesc;
        [ALHeader("不可升级时需要显示的GO列表")]
        public List<GameObject> goCanNotUpgradeShowList;
        [ALHeader("不可升级时需要隐藏的GO列表")]
        public List<GameObject> goCanNotUpgradeHideList;
        [ALHeader("可以连升十级时显示的GO列表")]
        public List<GameObject> goTenLevelUpShowList;
        [ALHeader("可以连升十级时隐藏的GO列表")]
        public List<GameObject> goTenLevelUpHideList;
        [ALHeader("升级成功特效id")]
        public long upgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;
        [ALHeader("升级按钮红点")]
        public GameObject goUpgradeRedTip;
    }
}

