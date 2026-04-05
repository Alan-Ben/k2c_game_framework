package NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineDealer;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_CommonReward;
import Common.ServerObj.ServerObj_OrderPaySimpleInfo;
import CommonEnum.ESpecialItemType;
import MJLog.MJLog;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_ItemList;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardInfo;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardTakeResult;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_PaidVoucher;
import NPUSServer.USLog;

/**
 * 订单发货逻辑
 * @author mj
 */
public class OfflineDealer_OrderDelivery extends _AOfflineDataDealer
{
    @Override
    public EOfflineRewardEnum getEnum()
    {
        return EOfflineRewardEnum.C_ORDER_DELIVERY;
    }

    @Override
    public boolean isValid()
    {
        return true;
    }

    @Override
    public boolean syncToClient()
    {
        return true;
    }

    @Override
    public void _preDeal(OfflineRewardInfo _info, NPPlayerContext _context)
    {

    }

    @Override
    public void takeReward(OfflineRewardInfo _info, NPPlayerContext _context, OfflineRewardTakeResult _result)
    {
        Offline_CommonReward rewardItemListObj = _info.getItemList();
        if (null == rewardItemListObj)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ORDER_DELIVERY);
        context.setGuid(_context.getGuid());

        if (null == _info.getOfflineData())
        {
            //领取奖励
            _info.getUserData().gainItemListP(rewardItemListObj.getItemList(), context);
            
            //回包数据展示
        	NPCommon_ItemList gainItemListProto = new NPCommon_ItemList();
        	context.getCollector().fillProtoList(gainItemListProto.getItemList());
        	_result.setExtData(gainItemListProto);
        } else
        {
            SpecialItemDealer_PaidVoucher dealer =
                    _info.getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.PAID_VOUCHER, SpecialItemDealer_PaidVoucher.class);

            long oldVoucherCount = _info.getUserData().getItemCount(ENPItemType.BAG_ITEM, RefGeneral.Ref().voucher_item_bag_item_id);
            long oldPaidVoucherCount = dealer == null ? 0 : dealer.getItemCount();

            //领取奖励
            _info.getUserData().gainItemListP(rewardItemListObj.getItemList(), context);

            //回包数据展示
        	NPCommon_ItemList gainItemListProto = new NPCommon_ItemList();
        	context.getCollector().fillProtoList(gainItemListProto.getItemList());
        	_result.setExtData(gainItemListProto);

            //处理付费代金券特殊物品逻辑
            if (dealer != null)
            {
                for (NPCommon_ItemInfo itemInfo : rewardItemListObj.getItemList())
                {
                    if (itemInfo.getItemType() == ENPItemType.BAG_ITEM.ordinal() && itemInfo.getSubId() == RefGeneral.Ref().voucher_item_bag_item_id)
                    {
                        dealer.gainItem(itemInfo.getCount(), context);
                    }
                }
            }

            try
            {
                long newVoucherCount = _info.getUserData().getItemCount(ENPItemType.BAG_ITEM, RefGeneral.Ref().voucher_item_bag_item_id);
                long newPaidVoucherCount = dealer == null ? 0 : dealer.getItemCount();

                ServerObj_OrderPaySimpleInfo obj = new ServerObj_OrderPaySimpleInfo();
                obj.readPackage(_info.getOfflineData());

                MJLog.logVoucher(
                        _info.getUserData(),
                        context.getContextId(),  // 事件ID
                        oldVoucherCount,// 旧值
                        newVoucherCount,// 新值
                        newVoucherCount - oldVoucherCount,// 改变值
                        null,// 商品id（获得时传null）
                        0,// 商品数量（获得时传0）
                        1,// action: 1=获得
                        0,// cn_money
                        obj.getMoney(),// money
                        obj.getOrderId(),// 订单号（如果有）
                        obj.getSdkPayId(),// sdk_pay_id
                        oldPaidVoucherCount,// 付费代金券旧值
                        newPaidVoucherCount,// 付费代金券新值
                        newPaidVoucherCount - oldPaidVoucherCount// 付费代金券改变值
                );
            } catch (Exception ex)
            {
                USLog.error(_info.getUserData().getUSServer(), "", ex);
            }
        }
    }
}
