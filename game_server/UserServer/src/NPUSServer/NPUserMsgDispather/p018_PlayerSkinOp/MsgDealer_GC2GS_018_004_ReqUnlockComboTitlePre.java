package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;

import GC2GS.p018_PlayerSkinOp.GC2GS_018_004_ReqUnlockComboTitlePre;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerSkinErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerComboTitleType;
import NPGameRes.Refs.Title.RefPlayerTitlePrefix;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_004_ReqUnlockComboTitlePre extends NPUserMsgDealer<GC2GS_018_004_ReqUnlockComboTitlePre>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_004_ReqUnlockComboTitlePre _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        if(userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.PRE).hasItem(_msg.getPreId(), 1))
        {
        	_commiter.commitFailRes(PlayerSkinErr.PLAYER_TITLE_EXISTED.getCode());
        	return;
        }
        
        RefPlayerTitlePrefix ref = RefPlayerTitlePrefix.getMgr().get(_msg.getPreId());
        if(null == ref)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        if(!NPPlayerConditionDealerMgr.IsEnable(ref.unlock_condition, userData, null))
        {
        	_commiter.commitFailRes(CommErr.CONDITION_NOT_ENABLE.getCode());
        	return;
        }
        
        //解锁组合称号前缀
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.COMBO_TITLE_UNLOCK);
        userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.PRE).gainItem(_msg.getPreId(), 1, false, context);
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_004_RetUnlockComboTitlePre());
    }
}