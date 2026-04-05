package NPUSServer.NPUserMsgDispather.p030_ShopOp;

import GC2GS.p030_ShopOp.GC2GS_030_022_ReqBuyGiftPack;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_030_ShopOp;

/********
 * 购买礼包请求处理
 */
public class MsgDealer_GC2GS_030_022_ReqBuyGiftPack extends NPUserMsgDealer<GC2GS_030_022_ReqBuyGiftPack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_030_022_ReqBuyGiftPack _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GIFT_PACK_BUY);

        Result result = userData.getOrderComponent().getMgr().itemBuy(_msg.getPackId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        _commiter.commitSucRes(US2GCWriter_030_ShopOp.make_022_RetBuyGiftPack());
    }
}