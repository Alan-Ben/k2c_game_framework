using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUIMonoHeroInfoBusinessItem
    {
        [ALHeader("这条item的GO")]
        public GameObject goItem;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("解锁描述")]
        public Text txtUnlockDesc;
        [ALHeader("已解锁时需要展示的GO列表")]
        public List<GameObject> goUnlockShowList;
        [ALHeader("已解锁时需要隐藏的GO列表")]
        public List<GameObject> goUnlockHideList;
    }

    /// <summary>
    /// 骑士信息经营页签
    /// </summary>
    public class GGUIMonoHeroInfoBusinessPage : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("升级消耗道具")]
        public NPGGUIMonoCommonItem monoUpgradeCostItem;
        [ALHeader("经营技能图标")]
        public RawImage imgIcon;
        [ALHeader("经营技能名称")]
        public Text txtName;
        [ALHeader("经营技能等级")]
        public Text txtLevel;
        [ALHeader("经营技能描述")]
        public Text txtDesc;
        [ALHeader("下一级加成")]
        public Text txtNextLevelAdd;
        [ALHeader("满级时需要展示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("额外经营技能列表")]
        public List<GGUIMonoHeroInfoBusinessItem> monoAdditionItemList;
        [ALHeader("升级成功特效id")]
        public long upgradeSfxId;
        [ALHeader("升级成功特效父节点")]
        public Transform upgradeSfxParent;
        [ALHeader("可升级红点")]
        public GameObject goUpgradeRedTip;
    }
}

