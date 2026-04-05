using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 使用物品-伙伴列表item
    /// </summary>
    public class GGUIMonoBagItemUseHeroGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("使用按钮")]
        public GameObject btnUse;
        [ALHeader("物品不足时需要置灰的List")]
        public List<MaskableGraphic> grayImgList;
        [ALHeader("物品不足按钮")]
        public GameObject btnNotEnough;
        [ALHeader("物品不足时需要显示的GO列表")]
        public List<GameObject> goItemNotEnoughShowList;
        [ALHeader("物品不足时需要隐藏的GO列表")]
        public List<GameObject> goItemNotEnoughHideList;
        [ALHeader("伙伴头像信息")]
        public GGUIMonoHeroSpIconItem monoHeroIcon;
        [ALHeader("对应展示属性图标")]
        public RawImage imgValueType;
    }
}
