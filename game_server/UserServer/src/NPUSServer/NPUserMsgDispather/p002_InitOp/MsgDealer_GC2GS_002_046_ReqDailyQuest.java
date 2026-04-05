package NPUSServer.NPUserMsgDispather.p002_InitOp;

import GC2GS.p002_InitOp.GC2GS_002_046_ReqDailyQuest;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;


public class MsgDealer_GC2GS_002_046_ReqDailyQuest extends NPUserMsgDealer<GC2GS_002_046_ReqDailyQuest>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_046_ReqDailyQuest _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        //返回数据
        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_046_RetDailyQuest(userData));
    }
}
