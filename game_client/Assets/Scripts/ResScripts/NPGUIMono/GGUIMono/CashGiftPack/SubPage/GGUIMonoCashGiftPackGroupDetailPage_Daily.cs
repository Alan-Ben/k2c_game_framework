using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 礼包组详情页面-每日
    /// </summary>
    public class GGUIMonoCashGiftPackGroupDetailPage_Daily : GGUIMonoCashGiftPackGroupDetailPage
    {
        [ALHeader("积分商店按钮")]
        public GameObject btnShop;
        [ALHeader("商品列表容器")]
        public GGUIMonoCashGiftPackPageGoodsContainer monoGoodsContainer;
    }
}
