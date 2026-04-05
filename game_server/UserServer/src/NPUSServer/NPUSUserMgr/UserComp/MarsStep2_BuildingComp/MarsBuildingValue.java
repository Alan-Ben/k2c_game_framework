package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import NPCommon.Util.Pair.WCGPairInt;
import NPGameRes.Refs.Mars.*;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class MarsBuildingValue 
{
	//建筑数据
	private MarsBuildingInfo _m_biBuildingInfo;
	
	//----------------------------- 基础属性 ----------------------------- //
	//主基地-氧气值
	private long _m_lHomeOxygenYield;
	public long getHomeOxygenYield() {return _m_lHomeOxygenYield;}
	public void setHomeOxygenYield(long _value) {_m_lHomeOxygenYield = _value;}
	//主基地-资源消耗（单位：分钟）
	private long _m_lHomeEnergyConsumePerMin;
	public long getHomeEnergyConsumePerMin() {return _m_lHomeEnergyConsumePerMin;}
	public void setHomeEnergyConsumePerMin(long _value) {_m_lHomeEnergyConsumePerMin = _value;}
	//能源厂基础产出/分
	private long _m_lOutputValuePerMin;
	public long getOutputValuePerMin() {return _m_lOutputValuePerMin;}
	public void setOutputValuePerMin(long _value) {_m_lOutputValuePerMin = _value;}
	//居民派遣基础产出/分
	private long _m_lPeopleOutputValuePerMin;
	//public long getPeopleOutputValuePerMin() {return _m_lPeopleOutputValuePerMin;}
	//GOB-8369 资源建筑，人口加成不能放在部件内计算，直接按人口数量，放在外围计算
	//https://www.teambition.com/task/695b8e38c4c6ca13f45bb100
	public long getPeopleOutputValuePerMin() {return 1;}
	public void setPeopleOutputValuePerMin(long _value) {_m_lPeopleOutputValuePerMin = _value;}
	//最大资源储存量
	private long _m_lMaxStorage;
	public long getMaxStorage() {return _m_lMaxStorage;}
	public void setMaxStorage(long _value) {_m_lMaxStorage = _value;}
	//资源消耗（单位：分钟）
	private long _m_lEnergyConsumePerMin;
	public long getEnergyConsumePerMin() {return _m_lEnergyConsumePerMin;}
	public void setEnergyConsumePerMin(long _value) {_m_lEnergyConsumePerMin = _value;}
	//饱腹值
	private long _m_lSatietyYield;
	public long getSatietyYield() {return _m_lSatietyYield;}
	public void setSatietyYield(long _value) {_m_lSatietyYield = _value;}
	//治愈率
	private long _m_lCureRate;
	public long getCureRate() {return _m_lCureRate;}
	public void setCureRate(long _value) {_m_lCureRate = _value;}
	//舒适值
	private long _m_lComfortYield;
	public long getComfortYield() {return _m_lComfortYield;}
	public void setComfortYield(long _value) {_m_lComfortYield = _value;}
    //心情值
	private long _m_lMoodYield;
	public long getMoodYield() {return _m_lMoodYield;}
	public void setMoodYield(long _value) {_m_lMoodYield = _value;}
    //睡眠值
	private long _m_lSleepYield;
	public long getSleepYield() {return _m_lSleepYield;}
	public void setSleepYield(long _value) {_m_lSleepYield = _value;}
    //居民上限(覆盖值)
	private long _m_lPeopleNumLimit;
	public long getPeopleNumLimit() {return _m_lPeopleNumLimit;}
	public void setPeopleNumLimit(long _value) {_m_lPeopleNumLimit = _value;}
	
	//容纳治疗居民的范围
	private WCGPairInt _m_piCureNum;
	public WCGPairInt getCureNum() {return _m_piCureNum;}
	
	//----------------------------- 需要计算加成属性 ----------------------------- //
	//计算后最终的饱腹值
	private long _m_lFinalSatietyValue;
	public long getFinalSatietyValue() {return _m_lFinalSatietyValue;}
	//计算后最终的舒适值
	private long _m_lFinalComfortValue;
	public long getFinalComfortValue() {return _m_lFinalComfortValue;}
	//计算后最终的心情值
	private long _m_lFinalMoodValue;
	public long getFinalMoodValue() {return _m_lFinalMoodValue;}
	//计算后最终的睡眠值
	private long _m_lFinalSleepValue;
	public long getFinalSleepValue() {return _m_lFinalSleepValue;}
	
	public MarsBuildingValue(MarsBuildingInfo _info)
	{
		_m_biBuildingInfo = _info;
		
		_m_piCureNum = new WCGPairInt();
	}

	public MarsBuildingInfo getBuilding() {return _m_biBuildingInfo;}
	public NPUSUserData getUserData() {return _m_biBuildingInfo.getUserData();}
	
	/**
	 * 重置数据
	 */
	protected void _resetAll()
	{
		_m_lOutputValuePerMin = 0;
		_m_lPeopleOutputValuePerMin = 0;
		_m_lMaxStorage = 0;
		_m_lEnergyConsumePerMin = 0;
		_m_lSatietyYield = 0;
		_m_lCureRate = 0;
		_m_lComfortYield = 0;
		_m_lMoodYield = 0;
		_m_lSleepYield = 0;
		_m_lPeopleNumLimit = 0;
		
		_m_lFinalSatietyValue = 0;
		_m_lFinalComfortValue = 0;
		_m_lFinalMoodValue = 0;
		_m_lFinalSleepValue = 0;
		
		_m_piCureNum.clear();
	}
	
	/*
	 * 累计部件属性
	 */
	protected void _calAddEquipment(MarsBuildingEquipmentInfo _info)
	{
	    RefMarsEquipmentFoodLevel foodLvlRef = _info.getFoolLvl();
	    if(null != foodLvlRef && _info.getBuilding().isFoodPowerOn())
	    {
	    	_m_lEnergyConsumePerMin += foodLvlRef.energy_consume_per_min;
	    	_m_lSatietyYield += foodLvlRef.satiety_yield;
	    }
	    
		RefMarsEquipmentEnergyLevel energyLvlRef = _info.getEnergyLvl();
		if(null != energyLvlRef)
		{
			_m_lOutputValuePerMin += energyLvlRef.output_value_per_min;
			_m_lMaxStorage += energyLvlRef.max_storage;
			_m_lPeopleOutputValuePerMin += energyLvlRef.people_output_value_per_min;
		}
		
		RefMarsEquipmentHospitalLevel hospitalLvlRef = _info.getHospitalLvl();
		if(null != hospitalLvlRef)
		{
			_m_lCureRate += hospitalLvlRef.cure_rate;
			
			_m_piCureNum.add(hospitalLvlRef.cure_num_range);
		}
		
		RefMarsEquipmentLivingLevel livingLvlRef = _info.getLivingLvl();
		if(null != livingLvlRef)
		{
			_m_lComfortYield += livingLvlRef.comfort_yield;
			_m_lMoodYield += livingLvlRef.mood_yield;
			_m_lSleepYield += livingLvlRef.sleep_yield;
			_m_lPeopleNumLimit += livingLvlRef.people_num_limit;
		}
	}

	/**
	 * 计算加成数值
	 */
	protected void _calAddPer()
	{
		RefMarsBuildingLevel lvlRef = _m_biBuildingInfo.getLvlRef();
		if(null == lvlRef)
			return;
		
		_m_lFinalSatietyValue = _m_lSatietyYield * lvlRef.building_level_cofficient;
		_m_lFinalComfortValue = _m_lComfortYield * lvlRef.building_level_cofficient;
		_m_lFinalMoodValue = _m_lMoodYield * lvlRef.building_level_cofficient;
		_m_lFinalSleepValue = _m_lSleepYield * lvlRef.building_level_cofficient;
	}
	
	/**
	 * 更新主基地氧气产出值
	 * @param _value
	 */
	public void updateHomeOxygenYield(long _value) 
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_lHomeOxygenYield == _value)
				return;
			
			_m_lHomeOxygenYield = _value;
			
			//计算所有建筑属性
			getUserData().getMarsBuildingComponent().doLazyCalAllBuildingValue();
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 更新主基能量消耗值
	 * @param _value
	 */
	public void updateHomeEnergyConsumePerMin(long _value) 
	{
		getUserData().lockUser();
		
		try
		{
			if(_m_lHomeEnergyConsumePerMin == _value)
				return;
			
			_m_lHomeEnergyConsumePerMin = _value;
			
			//计算所有建筑属性
			getUserData().getMarsBuildingComponent().doLazyCalAllBuildingValue();
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	@Override
	public String toString()
	{
		StringBuilder sb = new StringBuilder();
		sb.append("\nHomeOxygenYield:").append(_m_lHomeOxygenYield);
		sb.append("\nHomeEnergyConsumePerMin:").append(_m_lHomeEnergyConsumePerMin);
		sb.append("\nOutputValuePerMin:").append(_m_lOutputValuePerMin);
		sb.append("\nPeopleOutputValuePerMin:").append(_m_lPeopleOutputValuePerMin);
		sb.append("\nMaxStorage:").append(_m_lMaxStorage);
		sb.append("\nEnergyConsumePerMin:").append(_m_lEnergyConsumePerMin);
		sb.append("\nSatietyYield:").append(_m_lSatietyYield);
		sb.append("\nCureRate:").append(_m_lCureRate);
		sb.append("\nComfortYield:").append(_m_lComfortYield);
		sb.append("\nMoodYield:").append(_m_lMoodYield);
		sb.append("\nSleepYield:").append(_m_lSleepYield);
		sb.append("\nPeopleNumLimit:").append(_m_lPeopleNumLimit);
		sb.append("\nCureNum:").append(_m_piCureNum.first()).append(" - ").append(_m_piCureNum.second());
		sb.append("\nFinalSatietyValue:").append(_m_lFinalSatietyValue);
		sb.append("\nFinalComfortValue:").append(_m_lFinalComfortValue);
		sb.append("\nFinalMoodValue:").append(_m_lFinalMoodValue);
		sb.append("\nFinalSleepValue:").append(_m_lFinalSleepValue);
		
		return sb.toString();
	}
}
