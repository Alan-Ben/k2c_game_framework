
using System;
using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseObj;
using Common.ConsortEnum;
using CommonEnum;
using GS2GC.p004_PlayerOp;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 周卡自动处理结算弹窗
    /// </summary>
    public class NoticeDealer_WeekCardGetSettleInfo : _AMainCityCanJumpPushNotice
    {
        private readonly GS2GC_004_015_RetWeekCardSettleInfo _m_info;
        private readonly long _m_offlineMs;

        public NoticeDealer_WeekCardGetSettleInfo(GS2GC_004_015_RetWeekCardSettleInfo _info, long _offlineMs, EMainCityPushNoticeTriggerType _pushNoticeTriggerType) : base(_pushNoticeTriggerType)
        {
            _m_info = _info;
            _m_offlineMs = _offlineMs;
        }
        
        protected override bool _isEnable { get { return true; } }
        protected override bool _canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool needTransBk { get { return true; } }
        public override bool isNoticeFullScreen { get { return false; } }
        public override bool isOnlyUINode { get { return true; } }
        protected override string _noticeTag { get { return NoticeTagConst.WEEK_CARD_GET_SETTLE_INFO; } }

        public override void dealShowNotice()
        {
            //征收数据塞进去
            if (null != _m_info.getFoodOfflineInfo() &&
                _m_info.getFoodOfflineInfo().getCount() > 0)
            {
                NPCommon_ItemInfo itemInfo = new NPCommon_ItemInfo();
                itemInfo.setCount(_m_info.getFoodOfflineInfo().getCount());
                itemInfo.setItemType((int)ENPItemType.CURRENCY);
                // itemInfo.setSubId((int)ECurrency.FOOD);
                _m_info.getSettleInfo().addItemList(itemInfo);
            }
                    
            GGUIWndWeekCardSettle.instance.onCloseWnd += setDealerDone;
            GUISceneMain.instance.showAddWnd(GGUIWndWeekCardSettle.instance, () =>
            {
                GGUIWndWeekCardSettle.instance.setInfo(_m_info.getSettleInfo(), _m_offlineMs);
            });
        }

        public override void dealHideNotice()
        {
        }

        protected override void __onDealerDone()
        {
            GGUIWndWeekCardSettle.instance.discard();
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
