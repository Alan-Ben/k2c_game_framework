package NPUSServer.NPUserMsgDispather.p021_PlayerInfo;

import GC2GS.p021_PlayerInfo.GC2GS_021_031_ReqDealFriendApply;
import NPCommon.ErrMain.FriendErr;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.EPlayerCounterEnum;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.ApplyMgr.FriendApplyInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;


public class MsgDealer_GC2GS_021_031_ReqDealFriendApply extends NPUserMsgDealer<GC2GS_021_031_ReqDealFriendApply>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_021_031_ReqDealFriendApply _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        if (_msg.getIsAgree()) //同意申请
        {
            //屏蔽玩家不能加好友
            ShieldCidInfo shield = userData.getUSServer().getShieldCidMgr().lookup(userData.getCid());
            if(null != shield && shield.hasShieldCid(_msg.getApplyCid()))
            {
                _commiter.commitFailRes(PlayerErr.SHIELD_PLAYER_BAN.getCode());
                return;
            }
        	
            //检查好友上限
            int friendLimit = (int) userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.FRIEND_NUM);
            if (getUSServer().getUserCounterMgr().getPlayerCounter(userData.getCid(), EPlayerCounterEnum.FRIEND) >= friendLimit)
            {
                _commiter.commitFailRes(FriendErr.FRIEND_LIMIT_ERROR.getCode());
                return;
            }

            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.AGREE_FRIEND_APPLY);
            //检查申请
            FriendApplyInfo apply = userData.getFriendComponent().getFriendApplyMgr().lookupApply(_msg.getApplyCid());
            if (null == apply)
            {
                _commiter.commitFailRes(FriendErr.FRIEND_APPLY_NOT_EXIST.getCode());
                return;
            }
            
            //先增加玩家计数
            getUSServer().getUserCounterMgr().incrPlayerCounter(userData.getCid(), EPlayerCounterEnum.FRIEND);
            
            FriendSystem.sendAgreeApply(getUSServer(), apply, context, (isSuc, errCode)->
            {
            	if(isSuc) //成功处理
            	{
            		//加入好友
            		userData.getFriendComponent().getFriendMgr().addFriend(apply.getApplyCid(), false, context);
            		//移除请求
            		userData.getFriendComponent().getFriendApplyMgr().removeApply(apply.getApplyCid(), context);
            		
            		//回包数据
                    _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_031_RetDealFriendApply());

                    //增加玩家添加好友的计数
                    userData.getRecordComponent().addRecord(ENPPlayerRecordParam.ADD_FRIEND, 1, context);
            	}
            	else //失败处理
            	{
            		_commiter.commitFailRes(errCode);
            	}
            	
                //更新玩家计数
                getUSServer().getUserCounterMgr().ensure(userData.getCid()).setCounter(EPlayerCounterEnum.FRIEND, userData.getFriendComponent().getFriendMgr().getFriendCount());
            });
        } 
        else //拒绝申请，只需要移除自身的好友申请即可
        {
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REFUSE_FRIEND_APPLY);
        	//获取并移除申请
        	FriendApplyInfo apply = userData.getFriendComponent().getFriendApplyMgr().removeApply(_msg.getApplyCid(), context);
            if (null == apply)
            {
                _commiter.commitFailRes(FriendErr.FRIEND_APPLY_NOT_EXIST.getCode());
                return;
            }
            
    		//回包数据
            _commiter.commitSucRes(US2GCWriter_021_PlayerInfo.make_031_RetDealFriendApply());
        }
    }
}
