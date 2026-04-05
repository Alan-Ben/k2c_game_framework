package  NPUSServer.NPUserMsgDispather.p015_ConsortOp;


import GC2GS.p015_ConsortOp.GC2GS_015_010_ReqUpgradeHaloLvl;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.ConsortErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.Consort.RefConsortHaloLvl;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import USLOGDB.OptBo.Opt015010ConsortUpHaloLvlBO;
public class  MsgDealer_GC2GS_015_010_ReqUpgradeHaloLvl extends NPUserMsgDealer<GC2GS_015_010_ReqUpgradeHaloLvl>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_015_010_ReqUpgradeHaloLvl _msg)
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
        //检查当前星辉配表数据
        if(null == consort.getHaloInfo().getRef())
        {
        	_commiter.commitFailRes(CommErr.DATA_STATE_ERR.getCode());
        	return;
        }
        
        int preLvl = consort.getHaloInfo().getLvl();
        //获取下一等级配置
        int nextLvl = consort.getHaloInfo().getLvl() + 1;
        RefConsortHaloLvl nextLvlRef = consort.getRef().getHaloLevelMapMgr().getLevelData(nextLvl);
        if(null == nextLvlRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        //检查消耗
        if(!userData.hasItem(consort.getHaloInfo().getRef().cost_item))
        {
        	_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_UP_HALO);
        //消耗物品
        if(!userData.spendItem(consort.getHaloInfo().getRef().cost_item, context))
        {
        	_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
        	return;
        }
        
        //升级星辉
        consort.getHaloInfo().setLvl(nextLvlRef, context);
        //获取升级奖励
        userData.gainItemList(nextLvlRef.reward_item_list, context);
        
        _commiter.commitSucRes(US2GCWriter_015_ConsortOp.make_010_RetUpgradeHaloLvl());
        
        //日志
        Opt015010ConsortUpHaloLvlBO optBo = new Opt015010ConsortUpHaloLvlBO();
        optBo.setConsortId(getUSServer().getBM(), _msg.getConsortId());
        optBo.setPreLvl(getUSServer().getBM(), preLvl);
        optBo.setNewLvl(getUSServer().getBM(), consort.getHaloInfo().getLvl());
        userData.logEvent(optBo, context);
    }
}