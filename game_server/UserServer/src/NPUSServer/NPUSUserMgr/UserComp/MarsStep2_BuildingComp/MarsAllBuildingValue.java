package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import NPCommon.Util.Pair.WCGPairInt;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 火星系统所有建筑的各个数指数之和
 * 
 * @author mj
 *
 */
public class MarsAllBuildingValue 
{
	//组件数据
	private MarsBuildingComponent _m_comp;

	//氧气值
    private long _m_lOxygenValue;
    public long getOxygenValue() {return _m_lOxygenValue;}
	public void setOxygenValue(long _value) {_m_lOxygenValue = _value;}
	//能源消耗（单位：分钟）
	private long _m_lEnergyConsumePerMin;
    public long getEnergyConsumePerMin() {return _m_lEnergyConsumePerMin;}
	public void setEnergyConsumePerMin(long _value) {_m_lEnergyConsumePerMin = _value;}
	//饱腹值
	private long _m_lSatietyValue;
	public long getSatietyValue() {return _m_lSatietyValue;}
	public void setSatietyValue(long _value) {_m_lSatietyValue = _value;}
	//治愈率
	private long _m_lCureRate;
	public long getCureRate() {return _m_lCureRate;}
	public void setCureRate(long _value) {_m_lCureRate = _value;}
	//舒适值
	private long _m_lComfortValue;
	public long getComfortValue() {return _m_lComfortValue;}
	public void setComfortValue(long _value) {_m_lComfortValue = _value;}
    //心情值
	private long _m_lMoodValue;
	public long getMoodValue() {return _m_lMoodValue;}
	public void setMoodValue(long _value) {_m_lMoodValue = _value;}
    //睡眠值
	private long _m_lSleepValue;
	public long getSleepValue() {return _m_lSleepValue;}
	public void setSleepValue(long _value) {_m_lSleepValue = _value;}
    //居民上限(覆盖指数)
	private long _m_lPeopleNumLimit;
	public long getPeopleNumLimit() {return _m_lPeopleNumLimit;}
	public void setPeopleNumLimit(long _value) {_m_lPeopleNumLimit = _value;}

	//能源厂基础产出/分
	private long _m_lOutputValuePerMin;
	public long getOutputValuePerMin() {return _m_lOutputValuePerMin;}
	public void setOutputValuePerMin(long _value) {_m_lOutputValuePerMin = _value;}

	//容纳治疗居民的范围
	private WCGPairInt _m_piCureNum;
	public WCGPairInt getCureNum() {return _m_piCureNum;}
	
    public MarsAllBuildingValue(MarsBuildingComponent _comp)
    {
    	_m_comp = _comp;
    	
    	_m_piCureNum = new WCGPairInt();
    }
    
    public MarsBuildingComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return _m_comp.getUserData();}
    
    /**
     * 修改氧气值
     * @param _value
     */
    public void updateOxygenValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	if(_m_lOxygenValue == _value)
        		return;
        	
        	_m_lOxygenValue = _value;

        	//触发健康指数计算
        	_m_comp.doLazyCalHealthIndex();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 修改消耗时长（单位：分钟）
     * @param _value
     */
    public void updateEnergyConsumePerMin(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	if(_m_lEnergyConsumePerMin == _value)
        		return;
        	
        	_m_lEnergyConsumePerMin = _value;

			//触发延迟计算火星能量消耗速度
			_m_comp.getMarsEnergyDealer().doLazyCalCostSpeed();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
	
    /**
     * 修改治愈率
     * @param _value
     */
    public void updateCureRate(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	if(_m_lCureRate == _value)
        		return;
        	
        	_m_lCureRate = _value;

			//触发延迟计算火星治愈率
			_m_comp.doLazyCalCureRate();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 修改舒适值
     * @param _value
     */
    public void updateComfortValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	if(_m_lComfortValue == _value)
        		return;
        	
        	_m_lComfortValue = _value;
        	
        	//触发幸福指数计算
			_m_comp.doLazyCalHappyIndex();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 更新心情值
     * @param _value
     */
    public void updateMoodValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	if(_m_lMoodValue == _value)
        		return;
        	
        	_m_lMoodValue = _value;
        	
        	//触发幸福指数计算
			_m_comp.doLazyCalHappyIndex();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 调整心情值
     * @param _chgValue
     */
    public void chgMoodValue(long _chgValue)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		long newValue = Math.max(_m_lMoodValue - _chgValue, 0);
    		
    		updateMoodValue(newValue);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 更新睡眠值
     * @param _value
     */
    public void updateSleepValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
        	if(_m_lSleepValue == _value)
        		return;
        	
        	_m_lSleepValue = _value;
        	
        	//触发健康指数计算
			_m_comp.doLazyCalHealthIndex();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 更新饱腹值
     * @param _value
     */
    public void updateSatietyValue(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(_m_lSatietyValue == _value)
        		return;
        	
        	_m_lSatietyValue = _value;

        	//触发健康指数计算
			_m_comp.doLazyCalHealthIndex();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 更新居民数量上限
     * @param _value
     */
    public void updatePeopleNumLimit(long _value)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if(_m_lPeopleNumLimit == _value)
        		return;
        	
        	_m_lPeopleNumLimit = _value;
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
		sb.append("\nOxygenValue:").append(_m_lOxygenValue);
		sb.append("\nEnergyConsumePerMin:").append(_m_lEnergyConsumePerMin);
		sb.append("\nSatietyValue:").append(_m_lSatietyValue);
		sb.append("\nCureRate:").append(_m_lCureRate);
		sb.append("\nComfortValue:").append(_m_lComfortValue);
		sb.append("\nMoodValue:").append(_m_lMoodValue);
		sb.append("\nSleepValue:").append(_m_lSleepValue);
		sb.append("\nPeopleNumLimit:").append(_m_lPeopleNumLimit);
		sb.append("\nOutputValuePerMin:").append(_m_lOutputValuePerMin);
		sb.append("\nCureNum:").append(_m_piCureNum.first()).append(" - ").append(_m_piCureNum.second());
		
		return sb.toString();
    }
}
