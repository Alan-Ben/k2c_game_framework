package  NPUSServer.NPUserMsgDispather.p014_ChildOp;
import GC2GS.p014_ChildOp.GC2GS_014_010_ReqAkeyRefuseToMeApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
public class  MsgDealer_GC2GS_014_010_ReqAkeyRefuseToMeApply extends NPUserMsgDealer<GC2GS_014_010_ReqAkeyRefuseToMeApply>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_010_ReqAkeyRefuseToMeApply _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_REFUSE_ALL_PLAYER_APPLY);
        
        userData.getChildComponent().getToMeMarryApplyMgr().allRefuse(context);
        
        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_010_RetAkeyRefuseToMeApply());
    }
}