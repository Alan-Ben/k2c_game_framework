package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_013_ReqUnlockHalo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
public class  MsgDealer_GC2GS_015_013_ReqUnlockHalo extends NPUserMsgDealer<GC2GS_015_013_ReqUnlockHalo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_013_ReqUnlockHalo _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查家人数据
        ConsortInfo consort = userData.getConsortComponent().lookup(_msg.getConsortId());
        if(null == consort)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查星辉等级配表
        if(null == consort.getHaloInfo().getRef())
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //已经解锁
        if(consort.getHaloInfo().isUnlock())
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_HALO_HAD_UNLOCKED.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_UNLOCK_HALO);
        //解锁星辉
        consort.getHaloInfo().unlock(context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_013_RetUnlockHalo());
    }
}