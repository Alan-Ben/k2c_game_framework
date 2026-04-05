package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_038_ReqChgFriendGroupOrderList;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

public class MsgDealer_GC2GS_021_038_ReqChgFriendGroupOrderList extends NPUserMsgDealer<GC2GS_021_038_ReqChgFriendGroupOrderList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_038_ReqChgFriendGroupOrderList _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //变更分组顺序
        Result result = userData.getFriendComponent().getFriendGroupMgr().chgGroupOrderList(_msg.getGroupIdList());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //回包消息
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_038_RetChgFriendGroupOrderList());
    }
}
