package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;

import GC2GS.p019_DinnerOp.GC2GS_019_014_ReqCheckDinnerInvite;
import NPUSServer.NPUSUserMgr.GameSystem.DinnerSystem.DinnerSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_014_ReqCheckDinnerInvite extends NPUserMsgDealer<GC2GS_019_014_ReqCheckDinnerInvite>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_014_ReqCheckDinnerInvite _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        DinnerSystem.GetDinnerIdx(userData, _msg.getInstanceId(), (_getDinnerErr, _dinnerIdx) -> 
        {
        	if(_getDinnerErr > 0)
        	{
        		_commiter.commitFailRes(_getDinnerErr);
        		return;
        	}
        	
        	_commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_014_RetCheckDinnerInvite(_dinnerIdx));
        });
    }
}