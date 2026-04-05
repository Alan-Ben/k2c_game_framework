package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_007_ReqSetCurSkin;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortSkin.ConsortSkinInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
public class  MsgDealer_GC2GS_015_007_ReqSetCurSkin extends NPUserMsgDealer<GC2GS_015_007_ReqSetCurSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_007_ReqSetCurSkin _msg)
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
        
        ConsortSkinInfo skin = consort.getSkinMgr().lookup(_msg.getSkinId());
        if(null == skin)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_SKIN_NOT_EXISTS.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_SET_CUR_SKIN);
        consort.setCurSkin(_msg.getSkinId(), context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_007_RetSetCurSkin());
    }
}