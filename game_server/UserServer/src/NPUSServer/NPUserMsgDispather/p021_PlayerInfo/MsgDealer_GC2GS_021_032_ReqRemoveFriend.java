package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_032_ReqRemoveFriend;
import NPCommon.ErrMain.FriendErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;


public class MsgDealer_GC2GS_021_032_ReqRemoveFriend extends NPUserMsgDealer<GC2GS_021_032_ReqRemoveFriend>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_032_ReqRemoveFriend _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REMOVE_FRIEND);
        //移除好友
        boolean res = userData.getFriendComponent().getFriendMgr().removeFriend(_msg.getTargetCid(), context);
        if(!res)
        {
        	_commiter.commitFailRes(FriendErr.FRIEND_NOT_EXISTED_ERROR.getCode());
        	return;
        }
        
        //移除对方好友数据
        FriendSystem.sendRemoveFriend(getUSServer(), userData.getCid(), _msg.getTargetCid(), context);

        //回包消息
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_032_RetRemoveFriend());
    }
}
