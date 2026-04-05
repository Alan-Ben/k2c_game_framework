package NPUSServer.NPUserMsgDispather.p038_MarsOp;

import Common.MarsObj.Mars_GoRoute_StageMsg;
import GC2GS.p038_MarsOp.GC2GS_038_005_ReqGetStageMsgList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_038_MarsOp;

import java.util.List;

public class MsgDealer_GC2GS_038_005_ReqGetStageMsgList extends NPUserMsgDealer<GC2GS_038_005_ReqGetStageMsgList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_038_005_ReqGetStageMsgList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        int stage = _msg.getStage();
        List<Mars_GoRoute_StageMsg> msgList = getUSServer().getMarsGoRouteMsgMgr().getStageMsgProtoList(stage);
        _commiter.commitSucRes(US2GCWriter_038_MarsOp.make_005_RetGetStageMsgList(stage, msgList));
    }
}
