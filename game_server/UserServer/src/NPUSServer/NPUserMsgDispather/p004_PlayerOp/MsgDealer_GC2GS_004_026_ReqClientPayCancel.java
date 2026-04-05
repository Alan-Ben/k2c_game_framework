package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_026_ReqClientPayCancel;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/********
 * 客户端支付取消通知处理
 */
public class MsgDealer_GC2GS_004_026_ReqClientPayCancel extends NPUserMsgDealer<GC2GS_004_026_ReqClientPayCancel>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_026_ReqClientPayCancel _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CLIENT_CANCEL_ORDER);

        Result result = userData.getOrderComponent().cancelOrderByOrderId(_msg.getOrderId(), context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_026_RetClientPayCancel());
    }
}