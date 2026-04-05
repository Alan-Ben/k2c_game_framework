package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;
import GC2GS.p018_PlayerSkinOp.GC2GS_018_012_ReqUnsetCurPlayerSkin;
import NPEnum.ENPPlayerParam;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_012_ReqUnsetCurPlayerSkin extends NPUserMsgDealer<GC2GS_018_012_ReqUnsetCurPlayerSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_012_ReqUnsetCurPlayerSkin _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        userData.getPlayerComponent().setParam(ENPPlayerParam.PLAYER_SKIN, 0);
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_012_RetUnsetCurPlayerSkin());
    }
}