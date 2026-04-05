package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_032_ReqUnsetShieldPlayer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.ShieldCidMgr.ShieldCidInfo;

public class MsgDealer_GC2GS_004_032_ReqUnsetShieldPlayer extends NPUserMsgDealer<GC2GS_004_032_ReqUnsetShieldPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_032_ReqUnsetShieldPlayer _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        ShieldCidInfo shield = userData.getUSServer().getShieldCidMgr().lookup(userData.getCid());
        if(null != shield)
        {
        	shield.removeShieldCid(_msg.getShieldCid());
        }
        
    	_commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_032_RetUnsetShieldPlayer());
    }
}
