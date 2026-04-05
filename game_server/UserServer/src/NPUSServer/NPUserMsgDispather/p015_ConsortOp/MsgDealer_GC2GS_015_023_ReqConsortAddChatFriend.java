package NPUSServer.NPUserMsgDispather.p015_ConsortOp;

import GC2GS.p015_ConsortOp.GC2GS_015_023_ReqConsortAddChatFriend;
import NPCommon.ErrMain.ConsortErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortChatComp.ConsortChatInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;

public class MsgDealer_GC2GS_015_023_ReqConsortAddChatFriend extends NPUserMsgDealer<GC2GS_015_023_ReqConsortAddChatFriend>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_023_ReqConsortAddChatFriend _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        ConsortChatInfo chatInfo = userData.getConsortChatComponent().lookupChatInfo(_msg.getConsortId());
        if (chatInfo == null)
        {
            _commiter.commitFailRes(ConsortErr.CHAT_DIALOGUE_NOT_TRIGGERED.getCode());
            return;
        }

        ConsortInfo consortInfo = userData.getConsortComponent().lookup(_msg.getConsortId());
        if (consortInfo == null)
        {
            _commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
            return;
        }

        consortInfo.markHasAddChatFriend();

        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_023_RetConsortAddChatFriend());
    }
}