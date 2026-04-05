package NPUSServer.NPUSUserMgr.UserComp.BuildingComp;

import ALBasicServer.ALTask._IALSynTask;
import Common.BuildingEnum.EBuildingFuncEnum;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function._ABuildingFunc;

public class BuildingSpeedCalTask implements _IALSynTask
{
	//建筑数据
	private final BuildingInfo _m_biBuildingInfo;
	
	public BuildingSpeedCalTask(BuildingInfo _building)
	{
		_m_biBuildingInfo = _building;
	}
	
	@Override
	public void run() 
	{
		_ABuildingFunc buildingFunc = _m_biBuildingInfo.getBuildingFunc(EBuildingFuncEnum.BUSINESS);
		if (buildingFunc== null)
			return;

		BuildingBusinessFunc businessFunc = buildingFunc instanceof BuildingBusinessFunc ? ((BuildingBusinessFunc) buildingFunc) : null;
		if (businessFunc == null)
			return;

		_m_biBuildingInfo.getUserData().lockUser();
		try{
		    //当前建筑产出速度，用于后面比对是否与 新产出速度 一致，如果不一致，需要继续触发后续的task
    		long preOutputSpeed = businessFunc.getSpeed();

    		//重新计算并更新数值
    		long outputSpeed = businessFunc.calOutputSpeed(null);

    		//没有变化，不需要继续触发
    		if(preOutputSpeed == outputSpeed)
    			return;

    		//更新产出速度
    		businessFunc.setSpeed(outputSpeed);

    		//替换玩家产出速度
    		_m_biBuildingInfo.getComp().replaceEarnings(preOutputSpeed, outputSpeed, false);
		}finally
		{
			_m_biBuildingInfo.getUserData().unlockUser();
		}
	}
}
