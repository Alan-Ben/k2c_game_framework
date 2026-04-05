package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import GC2GS.p006_BagItemOp.GC2GS_006_001_ReqSellBagItem;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.BagItemErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BagItemComp.BagItemInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;

public class MsgDealer_GC2GS_006_001_ReqSellBagItem extends NPUserMsgDealer<GC2GS_006_001_ReqSellBagItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_001_ReqSellBagItem _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //参数物品数量检查
        if(!userData.checkItemCount(_msg.getCount()))
        {
            _commiter.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }

        //背包物品检查
        BagItemInfo info = userData.getBagItemComponent().getBagItem(_msg.getItemId());
        if (null == info)
        {
            _commiter.commitFailRes(BagItemErr.BAG_ITEM_NOT_EXISTS_ERROR.getCode());
            return;
        }
        if (!info.getRef().can_sell)
        {
            _commiter.commitFailRes(BagItemErr.BAG_ITEM_CAN_NOT_SELL_ERROR.getCode());
            return;
        }

        //背包数量检查
        if (!userData.hasItem(ENPItemType.BAG_ITEM, _msg.getItemId(), _msg.getCount()))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //消耗背包物品
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SELL_BAG_ITEM);
        if (!userData.spendItem(ENPItemType.BAG_ITEM, _msg.getItemId(), _msg.getCount(), context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //记录获取的物品
        for (int i = 0; i < info.getRef().sell_price.getItemTypeObjList().size(); i++)
        {
            NPCommonCostItem obj = info.getRef().sell_price.getItemTypeObjList().get(i);
            if (null == obj)
                continue;

            userData.gainItem(obj.getItemType(), obj.getItemId(), obj.getCount() * _msg.getCount(), context);
        }

        //推送获取物品协议
        userData.pushMsgToGC(context.getCollector().toProto());

        //返回出售成功
        _commiter.commitSucRes(US2GCWriter_006_BagItemOp.make_001_RetSellBagItemSucc());
    }
}
