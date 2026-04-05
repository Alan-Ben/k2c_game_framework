package NPUSServer.NPUserMsgDispather.p030_ShopOp;

import GC2GS.p030_ShopOp.GC2GS_030_003_ReqRefreshShop;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ShopErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Shop.RefShop;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopComponent;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;

public class MsgDealer_GC2GS_030_003_ReqRefreshShop extends NPUserMsgDealer<GC2GS_030_003_ReqRefreshShop>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_030_003_ReqRefreshShop _msg)
    {
        PlayerShopComponent shopComponent = _commiter.getUserData().getShopComponent();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SHOP_FREE_REFRESH);

        RefShop refShop = RefShop.getMgr().get(_msg.getShopRefId());
        if (refShop == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //查找商店数据
        PlayerShopInfo shop = shopComponent.lookup(_msg.getShopRefId());
        if (shop == null)
        {
            _commiter.commitFailRes(ShopErr.SHOP_ITEM_NOT_EXIST.getCode());
            return;
        }

        //刷新次数限制
        if (shop.getRefreshNum() >= refShop.pay_refresh_limit)
        {
            _commiter.commitFailRes(ShopErr.SHOP_REFRESH_LIMIT.getCode());
            return;
        }

        //计算刷新消耗
        NPCommonCostItem item = UsFunc.calCostPrice(_commiter.getUserData(), refShop.pay_refresh_times_price_id, shop.getRefreshNum());
        if (item == null)
        {
            _commiter.commitFailRes(CommErr.TIME_PRICE_CAL_ERR.getCode());
            return;
        }

        //消耗道具
        if (!_commiter.getUserData().spendItem(item, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        shop.addFreshNum(1);
        //强制刷新商店
        shopComponent.refreshGoods(_msg.getShopRefId());

        _commiter.commitSucRes(US2GCWriter_030_ShopOp.make_003_RetRefreshShop());
    }
}
