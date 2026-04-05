using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴星辉等级列表item
    /// </summary>
    public class GGUIMonoHeroHaloLevelContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClickItem;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("等级图标")]
        public RawImage imgIcon;
        [ALHeader("激活需要展示的GO列表")]
        public List<GameObject> goActivateShowList;
        [ALHeader("激活需要隐藏的GO列表")]
        public List<GameObject> goActivateHideList;
        [ALHeader("是当前选中的item时文本颜色")]
        public Color curSelectTextColor;
        [ALHeader("不是当前选中的item时文本颜色")]
        public Color notSelectTextColor;
        [ALHeader("上面进度GO")]
        public GameObject goProcessUp;
        [ALHeader("下面进度GO")]
        public GameObject goProcessDown;
        [ALHeader("上面进度GO的父节点")]
        public GameObject goProcessUpParent;
        [ALHeader("下面进度GO的父节点")]
        public GameObject goProcessDownParent;
        [ALHeader("激活与升级动画")]
        public CommonAnimationSingleInfo aniUpgrade;
        [ALHeader("可升级红点")]
        public GameObject goUpgradeRedTip;
    }
}