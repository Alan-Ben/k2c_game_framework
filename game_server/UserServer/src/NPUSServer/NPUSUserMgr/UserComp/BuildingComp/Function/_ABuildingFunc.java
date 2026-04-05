package NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function;

import Common.BuildingEnum.EBuildingFuncEnum;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;

public abstract class _ABuildingFunc
{
	private BuildingInfo _m_buildingInfo;

	public _ABuildingFunc(BuildingInfo _buildingInfo)
	{
		_m_buildingInfo = _buildingInfo;
	}

	public BuildingInfo getBuildingInfo()
	{
		return _m_buildingInfo;
	}
	
	public NPUSUserData getUserdata()
	{
		return getBuildingInfo().getUserData();
	}

	/***
	 * 建筑功能类型（农田建筑，经营建筑，...）
	 * @return
	 */
	public abstract EBuildingFuncEnum getFuncEnum();

	/**
	 * 建筑功能建造完成
	 */
	public abstract void onBuilt();

	/**
	 * 初始化计算
	 */
	public abstract void _initCalFunc(boolean _isInit);
}
