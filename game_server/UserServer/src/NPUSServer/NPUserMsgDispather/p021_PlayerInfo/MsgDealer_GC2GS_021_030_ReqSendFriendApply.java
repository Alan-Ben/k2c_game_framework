package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_030_ReqSendFriendApply;
import NPCommon.ErrMain.FriendErr;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;


public class MsgDealer_GC2GS_021_030_ReqSendFriendApply extends NPUserMsgDealer<GC2GS_021_030_ReqSendFriendApply>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_030_ReqSendFriendApply _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        //不能对自身发起申请
        if (userData.getCid() == _msg.getTargetCid())
        {
            _commiter.commitFailRes(FriendErr.FRIEND_SYS_ERROR.getCode());
            return;
        }
        
        //屏蔽玩家不能加好友
        ShieldCidInfo shield = userData.getUSServer().getShieldCidMgr().lookup(userData.getCid());
        if(null != shield && shield.hasShieldCid(_msg.getTargetCid()))
        {
            _commiter.commitFailRes(PlayerErr.SHIELD_PLAYER_BAN.getCode());
            return;
        }

        //检查当前是否好友
        if (null != userData.getFriendComponent().getFriendMgr().lookupByFCid(_msg.getTargetCid()))
        {
            _commiter.commitFailRes(FriendErr.FRIEND_EXISTED_ERROR.getCode());
            return;
        }

        //检查好友上限
        int friendLimit = (int) userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.FRIEND_NUM);
        if (userData.getFriendComponent().getFriendMgr().getFriendCount() >= friendLimit)
        {
            _commiter.commitFailRes(FriendErr.FRIEND_LIMIT_ERROR.getCode());
            return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SEND_FRIEND_APPLY);
        //发起申请
        FriendSystem.sendApply(getUSServer(), userData.getCid(), _msg.getTargetCid(), context, (isSuc, errCode)->
        {
        	if(isSuc)
        	{
                //回包数据
                _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_030_RetSendFriendApply());

                //增加玩家添加好友的计数
                userData.getRecordComponent().addRecord(ENPPlayerRecordParam.ADD_FRIEND, 1, context);
        	}
        	else
        	{
        		_commiter.commitFailRes(errCode);
        	}
        });
    }
}
