package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;
import GC2GS.p018_PlayerSkinOp.GC2GS_018_010_ReqUnlockPlayerSkin;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerSkinErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.PlayerSkin.RefPlayerSkin;
import NPGameRes.Refs.PlayerSkin.RefPlayerSkinLevel;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_010_ReqUnlockPlayerSkin extends NPUserMsgDealer<GC2GS_018_010_ReqUnlockPlayerSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_010_ReqUnlockPlayerSkin _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //已经解锁皮肤
        if(userData.getPlayerSkinComp().hasItem(_msg.getSkinId(), 1))
        {
        	_commiter.commitFailRes(PlayerSkinErr.PLAYER_SKIN_EXISTED.getCode());
        	return;
        }
        
        //检查皮肤配表
        RefPlayerSkin ref = RefPlayerSkin.getMgr().get(_msg.getSkinId());
        if(null == ref)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        RefPlayerSkinLevel lvlRef = ref.getLevelMapMgr().getLevelData(1);
        if(null == lvlRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //检查解锁消耗物品
        if(!userData.hasItem(ref.unlock_item))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_SKIN_UNLOCK);
        
        if(!userData.spendItem(ref.unlock_item, context)) 
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        
        userData.getPlayerSkinComp().gainItem(_msg.getSkinId(), 1, false, context);

        //获得解锁奖励物品
        if (!ref.unlock_gain_item_list.isEmpty())
            userData.gainItemList(ref.unlock_gain_item_list, context);

        //推送通用获得物品协议
        if(!context.getCollector().isEmpty())
        {
        	userData.sendMsgToGC(context.getCollector().toProto());
        }
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_010_RetUnlockPlayerSkin());
    }
}