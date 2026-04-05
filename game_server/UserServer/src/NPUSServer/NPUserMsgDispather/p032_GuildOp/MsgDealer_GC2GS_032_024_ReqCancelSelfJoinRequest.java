package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_024_ReqCancelSelfJoinRequest;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

public class MsgDealer_GC2GS_032_024_ReqCancelSelfJoinRequest extends NPUserMsgDealer<GC2GS_032_024_ReqCancelSelfJoinRequest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_024_ReqCancelSelfJoinRequest _msg)
    {
        Result result = _committer.getUserData().getUSServer().getGuildMgr().getJoinRequestMgr()
                .cancelJoinRequest(_committer.getUserData(), _msg.getGuildId());
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_024_RetCancelSelfJoinRequest());
    }
}
