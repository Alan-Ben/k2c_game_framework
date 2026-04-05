using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用活动兑换商店界面
    /// </summary>
    public class GGUIMonoActivityExchangeShop : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("商品列表")]
        public GGUIMonoActivityExchangeShopGrid monoShopGrid;
        [ALHeader("拥有的兑换券")]
        public NPGGUIMonoCommonItem monoCommonItem;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6000); } }
    }
}