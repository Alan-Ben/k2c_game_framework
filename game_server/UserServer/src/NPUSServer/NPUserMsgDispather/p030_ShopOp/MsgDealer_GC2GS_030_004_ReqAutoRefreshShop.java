package NPUSServer.NPUserMsgDispather.p030_ShopOp;

import GC2GS.p030_ShopOp.GC2GS_030_004_ReqAutoRefreshShop;
import NPUSServer.NPUSUserMgr.UserComp.ShopComp.PlayerShopComponent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;

public class MsgDealer_GC2GS_030_004_ReqAutoRefreshShop extends NPUserMsgDealer<GC2GS_030_004_ReqAutoRefreshShop>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_030_004_ReqAutoRefreshShop _msg)
    {
        PlayerShopComponent shopComponent = _commiter.getUserData().getShopComponent();

        //尝试自动刷新商店数据
        shopComponent.tryRefreshShop(_msg.getShopRefId());

        _commiter.commitSucRes(US2GCWriter_030_ShopOp.make_004_RetAutoRefreshShop());
    }
}
