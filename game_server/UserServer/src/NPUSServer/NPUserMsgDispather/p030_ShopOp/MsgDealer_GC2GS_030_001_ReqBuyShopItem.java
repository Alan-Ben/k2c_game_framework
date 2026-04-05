package NPUSServer.NPUserMsgDispather.p030_ShopOp;

import GC2GS.p030_ShopOp.GC2GS_030_001_ReqBuyShopItem;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ShopErr;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPair;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_SHOP_BUY_ITEM;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopComponent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;
import USLOGDB.OptBo.Opt030001BuyShopItemBO;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_030_001_ReqBuyShopItem extends NPUserMsgDealer<GC2GS_030_001_ReqBuyShopItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_030_001_ReqBuyShopItem _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        PlayerShopComponent shopComponent = userData.getShopComponent();

        //GOB-7267 【优化-0】检查商店购买协议数量上限，客户端UI上限999，但是超过99个都会购买失败
        //https://www.teambition.com/task/692e92beae577919da6715fe
        //对数量进行合法性判断
        if (_msg.getCount() < 0 || _msg.getCount() > 1000)
        {
            _commiter.commitFailRes(CommErr.NUM_REACH_LIMIT.getCode());
            return;
        }

        //计算开销
        List<NPCommonCostItem> itemList = shopComponent.calCost(_msg.getShopRefId(), _msg.getInstanceId(), (int) _msg.getCount());
        if (itemList == null)
        {
            _commiter.commitFailRes(ShopErr.SHOP_ITEM_NOT_EXIST.getCode());
            return;
        }

        if (!userData.hasCostItemList(itemList))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //日志所需信息
        WCGPair<Long, Long> logInfoPair = new WCGPair<>(0L, 0L);

        //实际购买
        ArrayList<NPCommonCostItem> goodsItemList = new ArrayList<>();
        if (!shopComponent.buyGoods(_msg.getShopRefId(), _msg.getInstanceId(), (int) _msg.getCount(), goodsItemList, logInfoPair))
        {
            _commiter.commitFailRes(ShopErr.SHOP_BUY_FAIL.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SHOP_BUY);
        //消耗购买商品的开销
        if (!userData.spendCostItemList(itemList, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        userData.gainItemList(goodsItemList, context);
        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_030_ShopOp.make_001_RetBuyShopItem());

        userData.onLogicEvent(new Event_P_SHOP_BUY_ITEM(context, _msg.getShopRefId()));

        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.SHOP_BUY_COUNT, _msg.getCount(), context);

        BM bmObj = getUSServer().getBM();

        //购买日志
        Opt030001BuyShopItemBO optLog = new Opt030001BuyShopItemBO();
        optLog.setShopId(bmObj, _msg.getShopRefId());
        optLog.setGoodsInstanceId(bmObj, _msg.getInstanceId());
        optLog.setGoodsRefId(bmObj, logInfoPair.first);
        optLog.setBuyNum(bmObj, _msg.getCount());
        optLog.setDiscount(bmObj, logInfoPair.second);
        optLog.setTotalConsume(bmObj, CommonFunc.list2String(itemList, ';'));
        userData.logEvent(optLog, context);
    }
}
