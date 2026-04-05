using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 藏品升级页签界面
    /// </summary>
    public class GGUIMonoEquipUpgradePage : _AALBasicUIWndMono
    {
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("资质")]
        public Text txtTalent;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("连升十级勾选框")]
        public NPGGUIMonoCommonToggleEx toggleTenLevelUp;
        [ALHeader("满级时需要显示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("可以连升十级时显示的GO列表")]
        public List<GameObject> goTenLevelUpShowList;
        [ALHeader("可以连升十级时隐藏的GO列表")]
        public List<GameObject> goTenLevelUpHideList;
        [ALHeader("单次升级成功特效id")]
        public long singleUpgradeSfxId;
        [ALHeader("十连升级成功特效id")]
        public long tenUpgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;
    }
}

