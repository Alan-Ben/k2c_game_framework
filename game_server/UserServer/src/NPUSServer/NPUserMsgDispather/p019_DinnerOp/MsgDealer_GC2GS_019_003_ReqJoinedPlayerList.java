package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;
import GC2GS.p019_DinnerOp.GC2GS_019_003_ReqJoinedPlayerList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_003_ReqJoinedPlayerList extends NPUserMsgDealer<GC2GS_019_003_ReqJoinedPlayerList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_003_ReqJoinedPlayerList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        _commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_003_RetJoinedPlayerList(userData));
    }
}