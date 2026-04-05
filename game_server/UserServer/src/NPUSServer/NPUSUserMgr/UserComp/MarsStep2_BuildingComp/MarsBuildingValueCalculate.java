package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import NPEnum.ENPPlayerPropertyType;

public class MarsBuildingValueCalculate 
{
	/**
	 * 计算每分钟能量产出
	 * 
	 * 能源总产出/分=( 能源厂基础产出/分+居民派遣基础产出/分*派遣居民人数 ) *(1+火星属性_能源产出万分比加成)
	 * 
	 * 能源厂基础产速/分,居民派遣基础产出/分, 在mars_equipment_energy_level表中配置								
		(字段名:能源厂基础产速/分output_value_per_min; 居民派遣基础产出/分people_output_value_per_min)							
	 * 
	 * @param _func
	 * @param _sb
	 * @return
	 */
	public static long calEnergyOutputValuePerMin(MarsPeopleBuildingFunc _func, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n------------------ cal energy start ------------------");
		}
		
		//能源厂基础产出/分
		long ouputValueMin = _func.getBuildingInfo().getBuildingValue().getOutputValuePerMin();
		if(null != _sb)
		{
			_sb.append("\nouputValueMin:").append(ouputValueMin);
		}
		//居民派遣基础产出/分
		long peopleOutputValuePerMin = _func.getBuildingInfo().getBuildingValue().getPeopleOutputValuePerMin();
		if(null != _sb)
		{
			_sb.append("\npeopleOutputValuePerMin:").append(peopleOutputValuePerMin);
		}
		//派遣居民人数
		int peopleSum = _func.getDispatchedNum();
		if(null != _sb)
		{
			_sb.append("\npeopleSum:").append(peopleSum);
		}
		//火星属性_能源产出万分比加成
		long propertyPer = _func.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_ENERGY_OUTPUT_PER);
		if(null != _sb)
		{
			_sb.append("\npropertyPer:").append(propertyPer);
		}
		//能源总产出/分=( 能源厂基础产出/分+居民派遣基础产出/分*派遣居民人数 ) *(1+火星属性_能源产出万分比加成)
		long value = (long) ((ouputValueMin + peopleOutputValuePerMin * peopleSum) * (1 + 1.0f * propertyPer / 10000f));
		if(null != _sb)
		{
			_sb.append("\nvalue:").append(value);
		}
		
		if(null != _sb)
		{
			_sb.append("\n------------------ cal energy end ------------------");
		}
		
		return value;
	}
}
