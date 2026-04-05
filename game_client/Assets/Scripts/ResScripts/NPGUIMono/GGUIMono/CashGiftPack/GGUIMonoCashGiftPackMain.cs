using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 现金礼包特殊展示处理类型
    /// </summary>
    public enum ECashGiftPackSpecialDealType
    {
        NONE,
        [InspectorName("SHOW_MONTH_CARD_GUIDE_HAND（显示月卡购买手指指引）")]
        SHOW_MONTH_CARD_GUIDE_HAND,
        [InspectorName("SHOW_YEAR_CARD_GUIDE_HAND（显示年卡购买手指指引）")]
        SHOW_YEAR_CARD_GUIDE_HAND,
    }

    [System.Serializable]
    public class MoveItemAdditionDistanceParam
    {
        [ALHeader("左侧额外向内移动距离")]
        public float leftAddDistance;
        [ALHeader("右侧额外向内移动距离")]
        public float rightAddDistance;
    }

    //页签类型
    public enum ECashGiftPackMainTabType
    {
        [InspectorName("GEM（钻石商店）")]
        GEM,
        [InspectorName("ACTIVITY（活动礼包）")]
        ACTIVITY,
        [InspectorName("PERMANENT（常驻礼包）")]
        PERMANENT,
        [InspectorName("RECHARGE_REBATE（充值返利）")]
        RECHARGE_REBATE,
        [InspectorName("PRIVILEGE_CARD（权益卡）")]
        PRIVILEGE_CARD,
        [InspectorName("FUND（基金）")]
        FUND,
    }

    /// <summary>
    /// 页签配置
    /// </summary>
    [System.Serializable]
    public class GGUICashGiftPackMainTabMono
    {
        [ALHeader("页签类型")]
        public ECashGiftPackMainTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("资源id")]
        public long resId;
    }

    /// <summary>
    /// 现金礼包主界面
    /// </summary>
    public class GGUIMonoCashGiftPackMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("页签滚动组件")]
        public ScrollRect tabScrollRect;
        [ALHeader("页签content")]
        public RectTransform tabContentTransform;
        [ALHeader("页签额外向内移动距离参数")]
        public MoveItemAdditionDistanceParam moveItemAdditionDistanceParam;
        [ALHeader("页签侧边红点提示信息")]
        public ContainerSideRedTipInfo sideRedTipInfo;
        [ALHeader("子窗口页面父节点")]
        public Transform pageParent;
        [ALHeader("页签列表")]
        public List<GGUICashGiftPackMainTabMono> monoTabList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6700); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6700); } }
    }
}
