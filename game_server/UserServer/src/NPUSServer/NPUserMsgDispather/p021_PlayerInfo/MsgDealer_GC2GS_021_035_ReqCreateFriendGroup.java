package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_035_ReqCreateFriendGroup;
import NPCommon.ErrMain.FriendErr;
import NPEnum.ENPPlayerPropertyType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.GroupMgr.FriendGroupInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;

public class MsgDealer_GC2GS_021_035_ReqCreateFriendGroup extends NPUserMsgDealer<GC2GS_021_035_ReqCreateFriendGroup>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_035_ReqCreateFriendGroup _msg)
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

        //检查自定义分组数量
        int groupLimit = (int) userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.CUSTOM_FRIEND_GROUP_NUM);
        if (userData.getFriendComponent().getFriendGroupMgr().getCustomGroupNum() >= groupLimit)
        {
            _commiter.commitFailRes(FriendErr.FRIEND_GROUP_REACH_LIMIT.getCode());
            return;
        }

        //创建分组
        FriendGroupInfo friendGroupInfo = userData.getFriendComponent().getFriendGroupMgr().addGroup(_msg.getGroupName());

        //把好友加入到分组
        userData.getFriendComponent().getFriendGroupMgr().changeGroup(friendGroupInfo.getDbId(), _msg.getCidList());

        //回包消息
        _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_035_RetCreateFriendGroup());
    }
}
