using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用活动兑换商店界面列表item
    /// </summary>
    public class GGUIMonoActivityExchangeShopGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("要购买的物品")]
        public NPGGUIMonoCommonItem monoSellItem;
        [ALHeader("购买消耗物品")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("物品特殊品质图")]
        public RawImage qualityImg;
        [ALHeader("购买按钮")]
        public GameObject btnBuy;
        [ALHeader("剩余限购次数")]
        public Text txtLeftBuyCount;
        [ALHeader("推荐标识")]
        public GameObject goRecommend;
        [ALHeader("售罄时需要展示的GO列表")]
        public List<GameObject> goSellOutShowList;
        [ALHeader("售罄时需要隐藏的GO列表")]
        public List<GameObject> goSellOutHideList;
    }
}
