using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 订单发货主城推送弹窗
    /// </summary>
    public class NoticeDealer_OrderDelivery : _AMainCityCanJumpPushNotice
    {
        private List<NPCommon.NPCommon_ItemInfo> _m_RewardItemList;
        private Action _m_OnDealerDone;
        
        public NoticeDealer_OrderDelivery(NPCommon_ItemList _rewardData, EMainCityPushNoticeTriggerType _pushNoticeTriggerType, Action _onDealerDone) : base(_pushNoticeTriggerType)
        {
            _m_RewardItemList = _rewardData?.getItemList();
            _m_OnDealerDone = _onDealerDone;
        }

        public NoticeDealer_OrderDelivery(List<NPCommon.NPCommon_ItemInfo> _rewardItemList, EMainCityPushNoticeTriggerType _pushNoticeTriggerType, Action _onDealerDone) : base(_pushNoticeTriggerType)
        {
            _m_RewardItemList = _rewardItemList;
            _m_OnDealerDone = _onDealerDone;
        }

        protected override bool _isEnable { get { return _m_RewardItemList != null; } }
        protected override bool _canCurShow { get { return true; } }
        protected override string _noticeTag { get { return NoticeTagConst.ORDER_DELIVERY; } }
        
        public override void dealShowNotice()
        {
            GCommon.dealGainItemDontUseNotice(_m_RewardItemList, TransKeyConst.common_getreward_tip,
                () =>
                {
                    setDealerDone();
                });
        }

        public override void dealHideNotice()
        {
        }
        
        protected override void __onDealerDone()
        {
            _m_OnDealerDone?.Invoke();
            _m_OnDealerDone = null;
        }
        
        // 临时加上, 防止不能IF修改
        public override void showNotice()
        {
            base.showNotice();
        }

        // 临时加上, 防止不能IF修改
        protected override void _onGotoOtherMainViewNode()
        {
        }
    }
}