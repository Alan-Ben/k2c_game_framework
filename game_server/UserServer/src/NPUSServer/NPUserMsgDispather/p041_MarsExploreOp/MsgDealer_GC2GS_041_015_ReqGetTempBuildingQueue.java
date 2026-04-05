package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_015_ReqGetTempBuildingQueue;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041015MarsBuildingTeampQueueBO;

public class MsgDealer_GC2GS_041_015_ReqGetTempBuildingQueue extends NPUserMsgDealer<GC2GS_041_015_ReqGetTempBuildingQueue>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_015_ReqGetTempBuildingQueue _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //获取次数，用于计算本次的消费
        long times = userData.getRecordComponent().getRecordCount(ENPPlayerRecordParam.MARS_BUILDING_TEMP_QUEUE_TIMES);
        
        NPCommonCostItem cost = UsFunc.calCostPrice(userData, RefGeneral.Ref().mars_building_temp_queue_time_price_type, (int)times);
        if(null == cost)
        {
        	_commiter.commitFailRes(CommErr.REF_ERROR.getCode());
        	return;
        }

        //检查消耗物品
        if(!userData.hasItem(cost))
        {
    		_commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
    		return;
        }
        
        //扣除消耗
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_ADD_TEMP_BUILD_QUEUE);
        if(!userData.spendItem(cost, context))
        {
    		_commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
    		return;
        }
        
        //增加创建计数
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.MARS_BUILDING_TEMP_QUEUE_TIMES, 1, context);
        
        //增加火星建筑临时队列buff
        userData.gainItem(RefGeneral.Ref().mars_temp_building_queue_gain_buff_item, context);
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_015_RetGetTempBuildingQueue());

        //日志数据
        Opt041015MarsBuildingTeampQueueBO optBo = new Opt041015MarsBuildingTeampQueueBO();
        _commiter.getUserData().logEvent(optBo, context);
    }
}
