package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_033_ReqFriendApplyExpired;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;


public class MsgDealer_GC2GS_021_033_ReqFriendApplyExpired extends NPUserMsgDealer<GC2GS_021_033_ReqFriendApplyExpired>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_033_ReqFriendApplyExpired _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.FRIEND_APPLY_EXPIRED);
        userData.getFriendComponent().getFriendApplyMgr().removeApply(_msg.getApplyCid(), context);

        //回包消息
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_033_RetFriendApplyExpired());
    }
}
