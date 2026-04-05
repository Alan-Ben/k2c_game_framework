using Common.OfflineRewardObj;
using System;
using CommonEnum;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 充值完成订单发货
    /// </summary>
    public class OfflineRewardInfo_OrderDelivery : _AOfflineRewardInfoWithData<Offline_OrderDelivery, NPCommon_ItemList>
    {
        public OfflineRewardInfo_OrderDelivery(OfflineReward_Info _info) : base(_info)
        {
        }
        
        protected override void _onDealReward(bool _isInit, Action _reqTakeReward)
        {
            if (offlineData == null)
                return;

            //发送埋点-开始处理Offline订单
            GCommon.sendStepReport(TraceConst.START_DEAL_OFFLINE_ORDER_DELIVERY.setMarkParam(
                offlineData.getOrderId(),
                offlineData.getGiftPackId(),
                offlineData.getSdkOrderId(),
                offlineData.getPayMoney(),
                offlineData.getPayCurrency(),
                offlineData.getOrderType()));

            Action afterShowReward = () =>
            {
                //发送埋点-Offline订单发货
                GCommon.sendStepReport(TraceConst.OFFLINE_ORDER_DELIVERY.setMarkParam(
                    offlineData.getOrderId(),
                    offlineData.getGiftPackId(),
                    offlineData.getSdkOrderId(),
                    offlineData.getPayMoney(),
                    offlineData.getPayCurrency(),
                    offlineData.getOrderType()));

                //发送第三方支付埋点
                GCommon.sendAllThirdPurchaseEvent(offlineData);

                //请求领取奖励
                _reqTakeReward?.Invoke();
            };
            
            if (_isInit)//若是初始化领取，全部使用主城推送弹窗弹出
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_OrderDelivery(offlineData.getItemList(), EMainCityPushNoticeTriggerType.LOGIN, afterShowReward));
            }
            else
            {
                GiftPackRefObj giftPackRefObj = offlineData == null ? null : GRefdataCoreMgr.instance.giftPackRefCore.getRef(offlineData.getGiftPackId());
                if (giftPackRefObj == null)
                {
                    GCommon.dealGainItem(offlineData.getItemList(), TransKeyConst.common_getreward_tip, afterShowReward);
                }
                else
                {
                    switch (giftPackRefObj.type)
                    {
                        case EGiftPackType.PUSH_GIFT://非初始化的现金礼包购买奖励展示不使用Notice(因为推送礼包可能是使用Notice展示的, 若奖励还使用Notice, 就无法弹出奖励弹窗)
                            GCommon.dealGainItemDontUseNotice(offlineData.getItemList(), TransKeyConst.common_getreward_tip,
                                () =>
                                {
                                    afterShowReward();
                                });
                            break;
                        
                        default:
                            GCommon.dealGainItem(offlineData.getItemList(), TransKeyConst.common_getreward_tip, afterShowReward);
                            break;
                    }
                }
            }
        }

        protected override void _onTakeReward(bool _isSuc, bool _isInit, NPCommon_ItemList _rewardData)
        {
            if(!_isSuc || _rewardData == null)
                return;

        }
    }
}