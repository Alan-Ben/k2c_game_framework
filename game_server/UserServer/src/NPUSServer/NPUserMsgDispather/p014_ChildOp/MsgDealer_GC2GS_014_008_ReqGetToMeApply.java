package  NPUSServer.NPUserMsgDispather.p014_ChildOp;
import GC2GS.p014_ChildOp.GC2GS_014_008_ReqGetToMeApply;
import NPCommon.ErrMain.ChildErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr.ToMeMarryApplyInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
public class  MsgDealer_GC2GS_014_008_ReqGetToMeApply extends NPUserMsgDealer<GC2GS_014_008_ReqGetToMeApply>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_008_ReqGetToMeApply _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        ToMeMarryApplyInfo toMeApply = userData.getChildComponent().getToMeMarryApplyMgr().checkExpiredAndLookup(_msg.getApplyAdultId());
        if(null == toMeApply)
        {
        	_commiter.commitFailRes(ChildErr.TO_ME_APPLY_NOT_EXISTS.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_008_RetGetToMeApply(toMeApply));
    }
}