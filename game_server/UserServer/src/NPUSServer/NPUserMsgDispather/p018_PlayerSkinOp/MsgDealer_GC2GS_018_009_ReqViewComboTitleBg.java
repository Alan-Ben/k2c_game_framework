package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;
import GC2GS.p018_PlayerSkinOp.GC2GS_018_009_ReqViewComboTitleBg;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPPlayerComboTitleType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_009_ReqViewComboTitleBg extends NPUserMsgDealer<GC2GS_018_009_ReqViewComboTitleBg>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_009_ReqViewComboTitleBg _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        Result result = userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.BG).setViewed(_msg.getBgId());
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_009_RetViewComboTitleBg());
    }
}