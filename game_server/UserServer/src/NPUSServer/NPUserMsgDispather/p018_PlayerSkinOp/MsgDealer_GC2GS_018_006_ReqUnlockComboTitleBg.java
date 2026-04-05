package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;

import GC2GS.p018_PlayerSkinOp.GC2GS_018_006_ReqUnlockComboTitleBg;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerSkinErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerComboTitleType;
import NPGameRes.Refs.Title.RefPlayerTitleBg;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_006_ReqUnlockComboTitleBg extends NPUserMsgDealer<GC2GS_018_006_ReqUnlockComboTitleBg>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_006_ReqUnlockComboTitleBg _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        if(userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.BG).hasItem(_msg.getBgId(), 1))
        {
        	_commiter.commitFailRes(PlayerSkinErr.PLAYER_TITLE_EXISTED.getCode());
        	return;
        }
        
        RefPlayerTitleBg ref = RefPlayerTitleBg.getMgr().get(_msg.getBgId());
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
        userData.getPlayerComboTitleComp().getUnitMgr(ENPPlayerComboTitleType.BG).gainItem(_msg.getBgId(), 1, false, context);
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_006_RetUnlockComboTitleBg());
    }
}