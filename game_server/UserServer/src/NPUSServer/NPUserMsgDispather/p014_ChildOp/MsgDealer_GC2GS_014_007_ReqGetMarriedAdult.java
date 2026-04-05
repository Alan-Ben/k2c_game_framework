package  NPUSServer.NPUserMsgDispather.p014_ChildOp;
import GC2GS.p014_ChildOp.GC2GS_014_007_ReqGetMarriedAdult;
import NPCommon.ErrMain.ChildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.MarriedAdultInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
public class  MsgDealer_GC2GS_014_007_ReqGetMarriedAdult extends NPUserMsgDealer<GC2GS_014_007_ReqGetMarriedAdult>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_007_ReqGetMarriedAdult _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        MarriedAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupMarriedAdult(_msg.getAdultId());
        if(null == adult)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_007_RetGetMarriedAdult(adult));
    }
}