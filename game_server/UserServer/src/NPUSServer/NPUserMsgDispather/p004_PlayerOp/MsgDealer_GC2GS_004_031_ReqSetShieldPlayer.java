package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_031_ReqSetShieldPlayer;
import NPCommon.ErrMain.PlayerErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.FriendSystem.FriendSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;

public class MsgDealer_GC2GS_004_031_ReqSetShieldPlayer extends NPUserMsgDealer<GC2GS_004_031_ReqSetShieldPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_031_ReqSetShieldPlayer _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        ShieldCidInfo shield = userData.getUSServer().getShieldCidMgr().getShield(userData.getCid());
        
        //已经存在
        if(shield.hasShieldCid(_msg.getShieldCid()))
        {
        	_commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_031_RetSetShieldPlayer());
        	return;
        }
        
        //超过允许屏蔽的上限数量
        if(shield.getShieldCount() >= RefGeneral.Ref().shield_cid_limit)
        {
        	_commiter.commitFailRes(PlayerErr.SHIELD_CID_EXPEND_LIMIT.getCode());
        	return;
        }
        
        //增加屏蔽玩家
        shield.addShieldCid(_msg.getShieldCid());
        
    	_commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_031_RetSetShieldPlayer());

        //屏蔽好友则自动取消好友关系
    	//GOD-3458【优化-0】聊天-聊天频道增加屏蔽功能 https://www.teambition.com/task/66272546d52f89720d411a46
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.SHIELD_PLAYER_ADD);
    	
    	userData.getFriendComponent().getFriendMgr().removeFriend(_msg.getShieldCid(), context);
    	FriendSystem.sendRemoveFriend(getUSServer(), userData.getCid(), _msg.getShieldCid(), context);
    }
}
