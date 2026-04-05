package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_036_ReqChgBelongFriendGroup;
import NPCommon.ErrMain.FriendErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

public class MsgDealer_GC2GS_021_036_ReqChgBelongFriendGroup extends NPUserMsgDealer<GC2GS_021_036_ReqChgBelongFriendGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_036_ReqChgBelongFriendGroup _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //检查是否传入的好友都是好友
        for (Long cid : _msg.getCidList())
        {
            if (userData.getFriendComponent().getFriendMgr().lookupByFCid(cid) == null)
            {
                _commiter.commitFailRes(FriendErr.FRIEND_EXISTED_ERROR.getCode());
                return;
            }
        }

        //把好友加入到分组
        Result result = userData.getFriendComponent().getFriendGroupMgr().changeGroup(_msg.getGroupDbId(), _msg.getCidList());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //回包消息
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_036_RetChgBelongFriendGroup());
    }
}
