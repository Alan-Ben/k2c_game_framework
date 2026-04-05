package  NPUSServer.NPUserMsgDispather.p010_BuildingOp;

import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import GC2GS.p010_BuildingOp.GC2GS_010_003_ReqFarmClickOutput;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_FARM_COLLECT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_FarmMutiple;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;
public class  MsgDealer_GC2GS_010_003_ReqFarmClickOutput extends NPUserMsgDealer<GC2GS_010_003_ReqFarmClickOutput>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_003_ReqFarmClickOutput _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        
        //检查配表-点击时间间隔不能为0
        if(RefGeneral.Ref().farming_building_click_spaceMS <= 0)
        {
        	_commiter.commitFailRes(CommErr.REF_ERROR.getCode());
        	return;
        }
        
        //获取建筑数据
        BuildingInfo info = userData.getBuildingComponent().lookupBuilding(_msg.getBuildingId());
        if(null == info)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NOT_EXIST.getCode());
        	return;
        }
        
        //检查是否农场建筑
        if(null == info.getFarm())
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NO_TYPE.getCode());
        	return;
        }

        //更新最后一次时间间隔
        Result result = info.getFarm().recordClick(CommonFunc.getNowTimeMS());
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }

        //获取农场暴击处理器
        SpecialItemDealer_FarmMutiple dealer = userData.getSpecialItemComponent().getDealer(ESpecialItemType.FARM_MULTIPLE, SpecialItemDealer_FarmMutiple.class);
        if (dealer == null)
        {
            _commiter.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        //检查暴击次数
        ResultOne<Long> calResult = dealer.calFarmMultipleClickOutput(info.getFarm().getLvlRef().multiple_weight_list,
                _msg.getClickNum(), _msg.getClientOpTimeMs());
        if (!calResult.isSucc())
        {
            _commiter.commitFailRes(calResult.getCode());
            return;
        }

        //获取暴击倍数
        long multiple = calResult.getData();
        if (multiple > 1)
        {
            dealer.addHadMultipleTimes();
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.BUILDING_FARM_CLICK);
        //获取收益
        long gainCount = info.getFarm().getLvlRef().tap_to_collect_num * multiple;
        userData.gainItem(ENPItemType.CURRENCY, ECurrency.SILVER.ordinal(), gainCount, context);
        
        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_003_RetFarmClickOutput());

        //领取次数
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.FRAM_COLLECT, 1, context);
        //领取数量
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.FRAM_COLLECT_SUM, gainCount, context);

        //触发事件
        userData.onLogicEvent(new Event_P_FARM_COLLECT(context));
    }
}