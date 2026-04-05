using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 购买按钮显示类型
    /// </summary>
    public enum ECommonBuyButtonType
    {
        [InspectorName("CASH（现金）")]
        CASH,
        [InspectorName("ITEM（道具）")]
        ITEM,
        [InspectorName("FREE（免费）")]
        FREE,
        [InspectorName("SELL_OUT（售罄）")]
        SELL_OUT,
    }

    /// <summary>
    /// 显示状态
    /// </summary>
    [System.Serializable]
    public class GGUIBuyButtonShowState
    {
        [ALHeader("显示类型")]
        public ECommonBuyButtonType type;
        [ALHeader("需要显示的列表")]
        public List<GameObject> goShowList;
    }

    /// <summary>
    /// 通用购买点击按钮，支持现金、道具购买、免费
    /// </summary>
    public class GGUIMonoCommonBuyButton : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
        [ALHeader("支付现金价格")]
        public Text txtCashPrice;
        [ALHeader("增加VIP经验")]
        public Text txtVIPExp;
        [ALHeader("售罄需要置灰的列表")]
        public List<MaskableGraphic> sellOutGrayList;
        [ALHeader("显示状态列表")]
        public List<GGUIBuyButtonShowState> buttonShowStateList;
        [ALHeader("售罄时点击是否需要弹售罄提示")]
        public bool isShowSellOutTip = true;
    }
}

