using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 现金礼包每日礼包积分兑换商店窗口
    /// </summary>
    public class GGUIMonoCashGiftPackDailyScoreShop : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("拥有的积分")]
        public NPGGUIMonoCommonItem monoCommonItem;
        [ALHeader("商品列表")]
        public GGUIMonoShopNormalPageGrid monoShopGrid;
        [ALHeader("商店表id")]
        public long shopRefId;
        [ALHeader("商店消耗类型")]
        public ECurrency costCurrencyType;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6705); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6705); } }
    }
}