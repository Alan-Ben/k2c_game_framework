using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤入口按钮item
    /// </summary>
    public class GGUIMonoConsortSkinBtnItem : _AGGUIMonoOptionItem
    {
        [ALHeader("当前穿戴皮肤图标")]
        public RawImage imgCurSkinIcon;
        [ALHeader("当前穿戴皮肤品质框")]
        public Image imgCurSkinQualityBg;
        [ALHeader("当前穿戴皮肤等级")]
        public Text txtSkinLevel;
        [ALHeader("当前穿戴皮肤星级")]
        public GGUIMonoHeroCommonStar monoSkinStar;
        [ALHeader("当前穿戴默认皮肤时需要显示的GO列表")]
        public List<GameObject> goDefaultSkinShowList;
        [ALHeader("当前穿戴默认皮肤时需要隐藏的GO列表")]
        public List<GameObject> goDefaultSkinHideList;
    }
}