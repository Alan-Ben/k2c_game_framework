package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;
import GC2GS.p018_PlayerSkinOp.GC2GS_018_001_ReqSetCommTitle;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_001_ReqSetCommTitle extends NPUserMsgDealer<GC2GS_018_001_ReqSetCommTitle>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_001_ReqSetCommTitle _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.TITLE_SET_CUR);
        
        Result result = userData.getPlayerComponent().getCurTitleMgr().setCommTitle(_msg.getTitle(), context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_001_RetSetCommTitle());
    }
}