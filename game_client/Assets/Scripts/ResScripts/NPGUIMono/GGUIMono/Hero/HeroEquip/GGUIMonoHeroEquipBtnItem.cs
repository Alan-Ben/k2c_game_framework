using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴藏品入口按钮item
    /// </summary>
    public class GGUIMonoHeroEquipBtnItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("品质框")]
        public Image imgQualityBg;
        [ALHeader("额外等级图标")]
        public RawImage imgEquipAdditionLevelIcon;
        [ALHeader("品质图标GO父节点")]
        public Transform goQualityIconParent;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("有佩戴藏品时需要显示的GO列表")]
        public List<GameObject> goWearEquipShowList;
        [ALHeader("有佩戴藏品时需要隐藏的GO列表")]
        public List<GameObject> goWearEquipHideList;
        [ALHeader("有藏品可装备或者有更高品质藏品可替换时需要显示的红点")]
        public GameObject goRedTip;
    }
}