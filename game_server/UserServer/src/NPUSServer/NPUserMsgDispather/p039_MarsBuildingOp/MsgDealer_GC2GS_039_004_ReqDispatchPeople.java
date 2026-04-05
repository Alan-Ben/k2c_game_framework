package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_004_ReqDispatchPeople;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsPeopleBuildingFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039004MarsBuildingDispatchPeopleBO;

public class MsgDealer_GC2GS_039_004_ReqDispatchPeople extends NPUserMsgDealer<GC2GS_039_004_ReqDispatchPeople>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_004_ReqDispatchPeople _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        if(_msg.getPeopleNum() <= 0)
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }
        
        //检查空闲人口
        if(_msg.getPeopleNum() > userData.getMarsPeopleComponent().getNumInfo().getIdleNum())
        {
        	_commiter.commitFailRes(CommErr.PARAM_ERROR.getCode());
        	return;
        }
        
        //检查建筑
        MarsPeopleBuildingFunc info = userData.getMarsBuildingComponent().getPeopleBuildingFuncMgr().lookup(_msg.getBuildingId());
        if(null == info)
        {
        	_commiter.commitFailRes(MarsErr.MARS_BUILDING_NOT_FOUND.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_BUILDING_PEOPLE_DISPATCH);
        //执行派遣
        Result result = info.dispatchPeople(_msg.getPeopleNum(), context);
        if(!result.isSucc())
        {
        	_commiter.commitFailRes(result.getCode());
        	return;
        }

        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_004_RetDispatchPeople());

        //日志数据
        Opt039004MarsBuildingDispatchPeopleBO optBo = new Opt039004MarsBuildingDispatchPeopleBO();
        optBo.setBuildingId(getUSServer().getBM(), _msg.getBuildingId());
        optBo.setPeopleNum(getUSServer().getBM(), _msg.getPeopleNum());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
