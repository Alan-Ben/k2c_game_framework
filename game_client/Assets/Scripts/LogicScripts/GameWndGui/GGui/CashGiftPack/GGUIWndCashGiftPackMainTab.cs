using NPEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 现金礼包主页面页签
    /// </summary>
    public class GGUIWndCashGiftPackMainTab : _ATNPGGUIWndCommonTab<ECashGiftPackMainTabType, GGUIWndCashGiftPackMainTab>, _IContainerSideRedTipItemInfo
    {
        public GGUIWndCashGiftPackMainTab(NPGGUIMonoCommonTab _wnd, ECashGiftPackMainTabType _tabType) : base(_wnd, _tabType)
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
                    case ECashGiftPackMainTabType.ACTIVITY:
                        _ARedTipNode activityRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_CASH_GIFT_PACK_ACTIVITY_TAB);
                        return activityRedTipNode != null && activityRedTipNode.needShow();
                    case ECashGiftPackMainTabType.PERMANENT:
                        _ARedTipNode permanentRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_CASH_GIFT_PACK_PERMANENT_TAB);
                        return permanentRedTipNode != null && permanentRedTipNode.needShow();
                    case ECashGiftPackMainTabType.RECHARGE_REBATE:
                        List<RechargeRebateInfo> infoList = NPPlayer.instance.rechargeRebateComp.getRechargeRebateInfoList();
                        bool haveRechargeRebate = infoList != null && infoList.Count > 0;
                        _ARedTipNode rechargeRebateRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_RECHARGE_REBATE);
                        return haveRechargeRebate && rechargeRebateRedTipNode != null && rechargeRebateRedTipNode.needShow();
                    case ECashGiftPackMainTabType.PRIVILEGE_CARD:
                        _ARedTipNode privilegeCardRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_PRIVILEGE_CARD);
                        return privilegeCardRedTipNode != null && privilegeCardRedTipNode.needShow();
                    case ECashGiftPackMainTabType.FUND:
                        bool haveFund = GCommon.isFuncUnlock(ENPFunctionType.ACTIVITY_FUND);
                        _ARedTipNode fundRedTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_FUND);
                        return haveFund && fundRedTipNode != null && fundRedTipNode.needShow();
                    default:
                        return false;
                }
                return false;
            }
        }
    }
}
