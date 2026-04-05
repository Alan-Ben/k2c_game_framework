package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_021_ReqClientPayDone;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/********
 * 客户端支付完成通知处理
 */
public class MsgDealer_GC2GS_004_021_ReqClientPayDone extends NPUserMsgDealer<GC2GS_004_021_ReqClientPayDone>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_021_ReqClientPayDone _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CLIENT_NOTIFY_ORDER_PAY);

        Result result = userData.getOrderComponent().clientNotifyPay(_msg.getOrderId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_021_RetClientPayDone());
    }
}