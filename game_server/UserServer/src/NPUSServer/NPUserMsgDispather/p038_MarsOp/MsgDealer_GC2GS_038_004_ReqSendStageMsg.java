package NPUSServer.NPUserMsgDispather.p038_MarsOp;

import GC2GS.p038_MarsOp.GC2GS_038_004_ReqSendStageMsg;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_038_MarsOp;

public class MsgDealer_GC2GS_038_004_ReqSendStageMsg extends NPUserMsgDealer<GC2GS_038_004_ReqSendStageMsg>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_038_004_ReqSendStageMsg _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        Result result = userData.getMarsGoRouteComponent().sendStageMsg(_msg.getStage(), _msg.getContent());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_038_MarsOp.make_004_RetSendStageMsg());
    }
}
