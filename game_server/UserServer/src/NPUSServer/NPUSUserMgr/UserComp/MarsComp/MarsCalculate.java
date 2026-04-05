package NPUSServer.NPUSUserMgr.UserComp.MarsComp;

import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class MarsCalculate 
{
	/**
	 * 计算火星实力
	 * 
	 * 公式：火星基地总实力=建筑实力+科技实力+总队伍历史最高实力
	 * 
	 * @param _userData
	 */
	public static void calMarsPower(NPUSUserData _userData, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n------------------ cal mars power start ------------------");
		}
		
		long value = 0;
		NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_POWER_CHECK);
		
		//所有建筑的火星实力汇总
		long buildingSum = _userData.getMarsBuildingComponent().getMarsPowerSum(_sb);
		if(null != _sb)
		{
			_sb.append("\nbuildingSum:").append(buildingSum);
		}
		value += buildingSum;
		//更新建筑最高实力
		_userData.getRecordComponent().setGtRecord(ENPPlayerRecordParam.MARS_BUILDING_MAX_POWER, buildingSum, context);
		
		//所有科技的火星实力汇总
		long techSum = _userData.getMarsTechComponent().getMarsPowerSum(_sb);
		if(null != _sb)
		{
			_sb.append("\ntechSum:").append(techSum);
			_sb.append(_userData.getMarsTechComponent().toString());
		}
		value += techSum;
		//更新科技最高实力数据
		_userData.getRecordComponent().setGtRecord(ENPPlayerRecordParam.MARS_TECH_MAX_POWER, techSum, context);
		
		//所有队伍历史最高值汇总
		long queueAdd = _userData.getRecordComponent().getRecordCount(ENPPlayerRecordParam.MARS_TEAM_MAX_POWER);
		if(null != _sb)
		{
			_sb.append("\nqueueAdd:").append(queueAdd);
		}	
		value += queueAdd;
		
		if(null != _sb)
		{
			_sb.append("\nvalue:").append(value);
		}
		
		//检查火星实力是否变更
		if(_userData.getMarsComponent().getMarsPower() == value)
			return;
		
		//更新火星实力
		_userData.getMarsComponent().setMarsPower(value);
	}
	
	/**
	 * 计算行军时间
	 * @param _userData
	 * @param _posRef
	 * @return
	 */
	public static long calMarchTimeMS(NPUSUserData _userData, RefMarsExplorePos _posRef)
	{
		//配置基础值
		long timeMS = _posRef.march_time * 1000;
		
		//玩家属性加成
		//GOB-8288 修正所有涉及加速万分比的计算 https://www.teambition.com/task/69593ecb0bc2f864a6a42c82
		//time = time / ((10000 + per)/10000)
		long property = _userData.getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_MARCH_SPEED_PER);
		timeMS = timeMS * 10000 / (10000 + property);
		
		//最小值检查
		timeMS = Math.max(timeMS, 0);
		
		return timeMS;
	}
}
