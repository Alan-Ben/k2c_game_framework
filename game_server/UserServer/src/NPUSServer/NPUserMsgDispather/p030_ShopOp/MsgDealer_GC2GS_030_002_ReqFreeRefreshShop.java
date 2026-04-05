package NPUSServer.NPUserMsgDispather.p030_ShopOp;

import GC2GS.p030_ShopOp.GC2GS_030_002_ReqFreeRefreshShop;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Shop.RefShop;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopComponent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;

public class MsgDealer_GC2GS_030_002_ReqFreeRefreshShop extends NPUserMsgDealer<GC2GS_030_002_ReqFreeRefreshShop>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_030_002_ReqFreeRefreshShop _msg)
    {
        PlayerShopComponent shopComponent = _commiter.getUserData().getShopComponent();

        RefShop refShop = RefShop.getMgr().get(_msg.getShopRefId());
        if (refShop == null)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SHOP_FREE_REFRESH);

        if (!_commiter.getUserData().spendItem(refShop.free_refresh_cd.getItemType(), refShop.free_refresh_cd.getItemId(), 1, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //强制刷新商店
        shopComponent.refreshGoods(_msg.getShopRefId());

        _commiter.commitSucRes(US2GCWriter_030_ShopOp.make_002_RetFreeRefreshShop());
    }
}
