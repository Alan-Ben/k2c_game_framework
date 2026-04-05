package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_028_ReqRecruit;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_028_ReqRecruit extends NPUserMsgDealer<GC2GS_007_028_ReqRecruit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_007_028_ReqRecruit _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData ==null)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RECRUIT);

        Result result = userData.getRecruitComponent().tryRecruit(_msg.getId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }
        
        userData.sendMsgToGC(context.getCollector().toProto());

        _committer.commitSucRes(US2GCWriter_007_CommOp.make_028_RetRecruit());
    }
}
