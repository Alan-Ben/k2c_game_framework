package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_009_ReqGetAllBusinessSkill;
import NPCommon.ErrMain.ConsortErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
public class  MsgDealer_GC2GS_015_009_ReqGetAllBusinessSkill extends NPUserMsgDealer<GC2GS_015_009_ReqGetAllBusinessSkill>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_009_ReqGetAllBusinessSkill _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ConsortInfo consort = userData.getConsortComponent().lookup(_msg.getConsortId());
        if(null == consort)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_GS2GC_009_RetGetAllBusinessSkill(consort));
    }
}