using System;

namespace GOE
{
    /// <summary>
    /// 现金礼包主页面页签
    /// </summary>
    public class GGUIWndMarsCashGiftPackMainTab : _ATNPGGUIWndCommonTab<EMarsCashGiftPackMainTabType, GGUIWndMarsCashGiftPackMainTab>, _IContainerSideRedTipItemInfo
    {
        public GGUIWndMarsCashGiftPackMainTab(NPGGUIMonoCommonTab _wnd, EMarsCashGiftPackMainTabType _tabType) : base(_wnd, _tabType)
        {
        }

        /// <summary>
        /// 设置点击tab
        /// </summary>
        public void setClickTab()
        {
            _onClickSelectButton(null);
        }

        /// <summary>
        /// 是否有红点
        /// </summary>
        public bool haveRedTip
        {
            get
            {
                switch (tabType)
                {
                    case EMarsCashGiftPackMainTabType.PERMANENT:
                        _ARedTipNode permanentRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_MARS_CASH_GIFT_PACK_PERMANENT_TAB);
                        return permanentRedTipNode != null && permanentRedTipNode.needShow();
                    default:
                        return false;
                }
                return false;
            }
        }
    }
}
