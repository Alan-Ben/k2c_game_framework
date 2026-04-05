package  NPUSServer.NPUserMsgDispather.p018_PlayerSkinOp;
import GC2GS.p018_PlayerSkinOp.GC2GS_018_013_ReqUpgradePlayerSkin;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PlayerSkinErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerSkinComp.PlayerSkinInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
public class  MsgDealer_GC2GS_018_013_ReqUpgradePlayerSkin extends NPUserMsgDealer<GC2GS_018_013_ReqUpgradePlayerSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_018_013_ReqUpgradePlayerSkin _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //已经解锁皮肤
        PlayerSkinInfo skin = userData.getPlayerSkinComp().lookup(_msg.getSkinId());
        if(null == skin)
        {
        	_commiter.commitFailRes(PlayerSkinErr.PLAYER_SKIN_NOT_FOUND.getCode());
        	return;
        }
        
        //检查皮肤配表
        if(null == skin.getSkinRef() || null == skin.getSkinLvlRef())
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //检查下一个等级的配表
        if(null == skin.getSkinRef().getLevelMapMgr().getLevelData(skin.getSkinLvl() + 1))
        {
        	_commiter.commitFailRes(PlayerSkinErr.PLAYER_SKIN_LVL_FULLL.getCode());
        	return;
        }
        
        //检查解锁消耗物品
        if(!userData.hasItem(skin.getSkinLvlRef().upgrade_cost))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.PLAYER_SKIN_UP_LVL);
        
        if(!userData.spendItem(skin.getSkinLvlRef().upgrade_cost, context)) 
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        
        skin.setLvl(skin.getSkinLvl() + 1, context);
        
        _commiter.commitSucRes(US2GCWriter_018_PlayerSkinOp.make_013_RetUpgradePlayerSkin());
    }
}