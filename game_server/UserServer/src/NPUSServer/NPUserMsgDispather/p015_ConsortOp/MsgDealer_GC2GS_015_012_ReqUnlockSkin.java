package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;
import GC2GS.p015_ConsortOp.GC2GS_015_012_ReqUnlockSkin;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Consort.RefConsortSkin;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
public class  MsgDealer_GC2GS_015_012_ReqUnlockSkin extends NPUserMsgDealer<GC2GS_015_012_ReqUnlockSkin>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_012_ReqUnlockSkin _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        RefConsortSkin skinRef = RefConsortSkin.getMgr().get(_msg.getSkinId());
        if(null == skinRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }

        //检查家人数据
        ConsortInfo consort = userData.getConsortComponent().lookup(skinRef.consort_id);
        if(null == consort)
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查皮肤数据
        if(consort.getSkinMgr().hasSkin(_msg.getSkinId()))
        {
        	_commiter.commitFailRes(ConsortErr.CONSORT_SKIN_HAD_UNLOCKED.getCode());
        	return;
        }
        
        //检查消耗物品
        if(!userData.hasItem(skinRef.unlock_cost_item))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_UNLOCK_SKIN);
        //消耗物品
        if(!userData.spendItem(skinRef.unlock_cost_item, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        //解锁皮肤
        consort.getSkinMgr().unlockSkin(_msg.getSkinId(), context);
        //获取解锁奖励
        userData.gainItemList(skinRef.unlock_gain_item_list, context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_012_RetUnlockSkin());
    }
}