package  NPUSServer.NPUserMsgDispather.p014_ChildOp;
import GC2GS.p014_ChildOp.GC2GS_014_006_ReqGetUnMarriedAdult;
import NPCommon.ErrMain.ChildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
public class  MsgDealer_GC2GS_014_006_ReqGetUnMarriedAdult extends NPUserMsgDealer<GC2GS_014_006_ReqGetUnMarriedAdult>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_006_ReqGetUnMarriedAdult _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        UnmarryAdultInfo adult = userData.getChildComponent().getAdultMgr().lookupUnmarryAdult(_msg.getAdultId());
        if(null == adult)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_006_RetGetUnMarriedAdult(adult));
    }
}