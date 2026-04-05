using Common.ActivityEnum;
using GC2GS.p017_ActivityOp;
using GS2GC.p017_ActivityOp;
using System;
using System.Collections.Generic;

namespace GOE
{
    public partial class CommonActivityComponent
    {
        /// <summary>
        /// 刷新钻石礼包商店红点
        /// </summary>
        public void refreshGemGiftPackRedTip()
        {
            long count = 0;
            if (_m_lCommonActivityInfoList != null)
            {
                for (int i = 0; i < _m_lCommonActivityInfoList.Count; i++)
                {
                    if (_m_lCommonActivityInfoList[i] != null &&
                        _m_lCommonActivityInfoList[i].isPlaying &&
                        _m_lCommonActivityInfoList[i].crystalGiftPackInfo != null &&
                        _m_lCommonActivityInfoList[i].crystalGiftPackInfo.haveFreeGiftPackCanBuy())
                        count++;
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GEM_GIFT_PACK_FREE, count);
        }

        /// <summary>
        /// 刷新钻石礼包商店每日红点
        /// </summary>
        public void refreshGemGiftPackDialyRedTip()
        {
            bool haveActivity = false;
            for (int i = 0; i < _m_validActivityInfoList.Count; i++)
            {
                if (_m_validActivityInfoList[i] != null &&
                    _m_validActivityInfoList[i].crystalGiftPackInfo != null &&
                    _m_validActivityInfoList[i].crystalGiftPackInfo.giftPackGroupRef != null &&
                    _m_validActivityInfoList[i].crystalGiftPackInfo.giftPackGroupRef.page_ui_res_id > 0 &&
                    _m_validActivityInfoList[i].isPlaying)
                {
                    haveActivity = true;
                    break;
                }
            }
            //钻石礼包入口每日红点
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_CRYSTAL_GIFT_PACK,
                AccountSettingMgr.instance.dailyTagSaver.isNewDay(DailyTagConst.CRYSTAL_GIFT_PACK_RED_TIP) && haveActivity ? 1 : 0);
        }

        #region S2C

        /// <summary>
        /// 活动商店变更刷新
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityShopRefresh(GS2GC_017_058_OnActivityShopRefresh _msg)
        {
            if (_msg == null)
                return;

            _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_msg.getInstanceId());
            if (activityInfo != null)
            {
                activityInfo.updateShopInfo(_msg.getShopInfo());
                WinMsg.SendMsg(WinMsgType.ON_ACTIVITY_SHOP_REFRESH, _msg.getInstanceId());
            }
        }

        /// <summary>
        /// 活动钻石礼包变更刷新
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityCrystalGiftPackRefresh(GS2GC_017_059_OnActivityCrystalGiftPackRefresh _msg)
        {
            if (_msg == null)
                return;

            _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_msg.getInstanceId());
            if (activityInfo != null)
            {
                activityInfo.updateCrystalGiftPackInfo(_msg.getCrystalGiftPackInfo());
                //刷新红点
                refreshGemGiftPackRedTip();
                WinMsg.SendMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH, _msg.getInstanceId());
            }
        }

        /// <summary>
        /// 活动商店商品购买变更
        /// </summary>
        public void onActivityShopItemBuy(GS2GC_017_060_OnActivityShopItemBuy _msg)
        {
            if (_msg == null)
                return;

            _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_msg.getInstanceId());
            if (activityInfo != null && activityInfo.exchangeActivityShopInfo != null && activityInfo.exchangeActivityShopInfo.activityShopId == _msg.getShopId())
            {
                activityInfo.exchangeActivityShopInfo.updateBuyRecord(_msg.getBuyRecord());
                WinMsg.SendMsg(WinMsgType.ON_ACTIVITY_SHOP_BUY_RECORD_CHG, _msg.getInstanceId(), _msg.getShopId(), _msg.getBuyRecord().getItemId());
            }
        }

        /// <summary>
        /// 活动钻石礼包商品购买变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityCrystalGiftPackItemBuy(GS2GC_017_061_OnActivityCrystalGiftPackItemBuy _msg)
        {
            if (_msg == null)
                return;

            _ABaseActivityInfo activityInfo = getActivityInfoByInstanceId(_msg.getInstanceId());
            if (activityInfo != null && activityInfo.crystalGiftPackInfo != null && activityInfo.crystalGiftPackInfo.giftPackGroupId == _msg.getGroupId())
            {
                activityInfo.crystalGiftPackInfo.updateBuyRecord(_msg.getBuyRecord());
                //刷新红点
                refreshGemGiftPackRedTip();
                WinMsg.SendMsg(WinMsgType.ON_ACTIVITY_CRYSTAL_GIFT_PACK_BUY_RECORD_CHG, _msg.getInstanceId(), _msg.getGroupId(), _msg.getBuyRecord().getGiftPackId());
            }
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求刷新活动商店
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_shopId"></param>
        /// <param name="_callback"></param>
        public void reqRefreshActivityShop(long _instanceId, long _shopId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_014_ReqRefreshActivityShop(_instanceId, _shopId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_014_RetRefreshActivityShop>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求购买活动商店商品
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_shopId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_callback"></param>
        public void reqBuyActivityShopItem(long _instanceId, long _shopId, long _itemId, int _num, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_015_ReqBuyActivityShopItem(_instanceId, _shopId, _itemId, _num),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_015_RetBuyActivityShopItem>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求刷新活动钻石礼包
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_groupId"></param>
        /// <param name="_callback"></param>
        public void reqRefreshActivityCrystalGiftPack(long _instanceId, long _groupId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_016_ReqRefreshActivityCrystalGiftPack(_instanceId, _groupId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_016_RetRefreshActivityCrystalGiftPack>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        /// <summary>
        /// 请求购买活动钻石礼包
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_groupId"></param>
        /// <param name="_packId"></param>
        /// <param name="_callback"></param>
        public void reqBuyActivityCrystalGiftPack(long _instanceId, long _groupId, long _packId, int _num, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_017_017_ReqBuyActivityCrystalGiftPack(_instanceId, _groupId, _packId, _num),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_017_017_RetBuyActivityCrystalGiftPack>((info) =>
                {
                    if (null != _callback)
                        _callback();
                }));
        }

        #endregion
    }
}