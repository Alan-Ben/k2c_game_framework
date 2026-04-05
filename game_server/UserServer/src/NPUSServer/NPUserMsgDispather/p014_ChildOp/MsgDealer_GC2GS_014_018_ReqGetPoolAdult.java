package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_018_ReqGetPoolAdult;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
public class  MsgDealer_GC2GS_014_018_ReqGetPoolAdult extends NPUserMsgDealer<GC2GS_014_018_ReqGetPoolAdult>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_018_ReqGetPoolAdult _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        AdultMarrySystem.GetPoolAdult(getUSServer(), _msg.getCid(), _msg.getAdultId(), (_errCode, _poolAdult) -> 
        {
        	if(_errCode > 0)
        	{
        		_commiter.commitFailRes(_errCode);
        	}
        	else
        	{
        		_commiter.commitSucRes(US2GCWriter_014_ChildOp.make_018_RetGetPoolAdult(_poolAdult));
        	}
        });
    }
}