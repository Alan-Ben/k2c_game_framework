package  NPUSServer.NPUserMsgDispather.p019_DinnerOp;

import GC2GS.p019_DinnerOp.GC2GS_019_011_ReqGetPreDinnerInfo;
import NPUSServer.NPUSUserMgr.GameSystem.DinnerSystem.DinnerSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_019_DinnerOp;
public class  MsgDealer_GC2GS_019_011_ReqGetPreDinnerInfo extends NPUserMsgDealer<GC2GS_019_011_ReqGetPreDinnerInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_019_011_ReqGetPreDinnerInfo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        DinnerSystem.GetPreDinnerInfoBySort(userData, _msg.getInstanceId(), _msg.getIdx(), (_errCode, _dinnerResult) -> 
        {
        	if(_errCode > 0)
        	{
        		_commiter.commitFailRes(_errCode);
        	}
        	else
        	{
        		_commiter.commitSucRes(US2GCWriter_019_DinnerOp.make_011_RetGetPreDinnerInfo(_dinnerResult));
        	}
        });
    }
}