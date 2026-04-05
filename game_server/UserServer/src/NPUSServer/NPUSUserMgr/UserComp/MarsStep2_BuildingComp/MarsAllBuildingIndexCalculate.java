package NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp;

import Common.MarsEnum.EMarsBuildingType;
import Common.MarsEnum.EMarsPropertyType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 火星系统各项指数计算公式
 * @author mj
 *
 */
public class MarsAllBuildingIndexCalculate 
{
	/**
	 * 健康指数计算
	 * 
	 * 健康指数=（氧气指数*氧气系数+饱腹指数*氧气系数+睡眠指数*睡眠系数）*（1+修正系数(暂不用)+火星属性_健康指数万分比加成）
	 * 
	 * @param _userData
	 * @param _sb
	 */
	public static void calHealthIndex(NPUSUserData _userData, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n------------------ cal health index start ------------------");
		}
		
		long value = 0;
		
		do
		{	
			//人口数
			long peopleSum = _userData.getMarsPeopleComponent().getNumInfo().getPeopleSum();
			if(null != _sb)
			{
				_sb.append("\npeopleSum:").append(peopleSum);
			}
			
			//氧气指数*氧气系数
			long oxygen = 0;
			//(主基地当前功率*主基地氧气值等级修正系数)
			long oxygenAllBuildingValue = _userData.getMarsBuildingComponent().getAllBuildingValue().getOxygenValue();
			if(null != _sb)
			{
				_sb.append("\noxygenAllBuildingValueSum:").append(oxygenAllBuildingValue);
			}
			//(每人口消耗氧气值*人口数)
			long oxygenPeopleValue = peopleSum * RefGeneral.Ref().mars_building_oxygen_yield_per_consume;
			if(null != _sb)
			{
				_sb.append("\noxygenPeopleValue:").append(oxygenPeopleValue);
			}
			if(oxygenAllBuildingValue > 0 && oxygenPeopleValue > 0)
			{
				//火星属性_氧气指数万分比加成
				long oxygenPropertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.OXYGEN_ADD_PER);
				//*氧气指数=[(主基地当前功率*主基地氧气值等级修正系数)/(每人口消耗氧气值*人口数)]*(1+火星属性_氧气指数万分比加成)
				long oxygenIndex = (long) ((1.0f * oxygenAllBuildingValue / oxygenPeopleValue) * (1 + 1.0f * oxygenPropertyPer / 10000f));
				long finalOxygenIndex = Math.min(oxygenIndex, RefGeneral.Ref().mars_building_oxygen_index_max);//计算结果受上限值限制
				//氧气指数*氧气系数
				oxygen = (long) (finalOxygenIndex * (1.0f * RefGeneral.Ref().mars_building_oxygen_coefficient / 10000f));
				if(null != _sb)
				{
					_sb.append("\noxygenPropertyPer:").append(oxygenPropertyPer);
					_sb.append("\noxygenIndex:").append(oxygenIndex);
					_sb.append("\nfinalOxygenIndex:").append(finalOxygenIndex);
					_sb.append("\noxygen:").append(oxygen);
				}
			}

			//饱腹指数*饱腹系数
			long satiety = 0;
			//（当前生态园产出饱腹值*生态园饱腹值等级修正系数）
			long satietyAllBuildingValue = _userData.getMarsBuildingComponent().getAllBuildingValue().getSatietyValue();
			if(null != _sb)
			{
				_sb.append("\nsatietyAllBuildingValueSum:").append(satietyAllBuildingValue);
			}
			//（每人口消耗饱腹值*人口数）
			long satietyPeopleValue = peopleSum * RefGeneral.Ref().mars_building_satiety_yield_per_consume;
			if(null != _sb)
			{
				_sb.append("\nsatietyPeopleValue:").append(satietyPeopleValue);
			}
			if(satietyAllBuildingValue > 0 && satietyPeopleValue > 0)
			{
				//火星属性_饱腹指数万分比加成
				long satietyPropertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.SATIETY_ADD_PER);
				//*饱腹指数=[（当前生态园产出饱腹值*生态园饱腹值等级修正系数）/（每人口消耗饱腹值*人口数）]* （1+火星属性_饱腹指数万分比加成）
				long satietyIndex = (long) ((1.0f * satietyAllBuildingValue / satietyPeopleValue) * (1 + 1.0f * satietyPropertyPer / 10000f));
				long finalSatietyIndex = Math.min(satietyIndex, RefGeneral.Ref().mars_building_satiety_index_max);//计算结果受上限值限制
				//饱腹指数*饱腹系数
				satiety = (long) (finalSatietyIndex * (1.0f * RefGeneral.Ref().mars_building_satiety_coefficient / 10000f));
				if(null != _sb)
				{
					_sb.append("\nsatietyPropertyPer:").append(satietyPropertyPer);
					_sb.append("\nsatietyIndex:").append(satietyIndex);
					_sb.append("\nfinalSatietyIndex:").append(finalSatietyIndex);
					_sb.append("\nsatiety:").append(satiety);
				}
			}
			
			//睡眠指数*睡眠系数
			long sleep = 0;
			//（当前居民舱A产出睡眠值*居民舱A睡眠值等级修正系数+当前居民舱B产出睡眠值*居民舱B睡眠值等级修正系数+...）
			long sleepAllBuildingValue = _userData.getMarsBuildingComponent().getAllBuildingValue().getSleepValue();
			if(null != _sb)
			{
				_sb.append("\nsleepAllBuildingValueSum:").append(sleepAllBuildingValue);
			}
			//（每人口消耗睡眠值*人口数）
			long sleepPeopleValue = peopleSum * RefGeneral.Ref().mars_building_sleep_yield_per_consume;
			if(null != _sb)
			{
				_sb.append("\nsleepPeopleValue:").append(sleepPeopleValue);
			}
			if(sleepPeopleValue > 0 && sleepPeopleValue > 0)
			{
				long sleepPropertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.SLEEP_ADD_PER);
				//*睡眠指数=[（当前居民舱A产出睡眠值*居民舱A睡眠值等级修正系数+当前居民舱B产出睡眠值*居民舱B睡眠值等级修正系数+...）/（每人口消耗睡眠值*人口数）]* （1+火星属性_睡眠指数万分比加成）
				long sleepIndex = (long) ((1.0f * sleepAllBuildingValue / sleepPeopleValue) * (1 + 1.0f * sleepPropertyPer / 10000f));
				long finalSleepIndex = Math.min(sleepIndex, RefGeneral.Ref().mars_building_sleep_index_max);//计算结果受上限值限制
				//睡眠指数*睡眠系数
				sleep = (long) (finalSleepIndex * (1.0f * RefGeneral.Ref().mars_building_sleep_coefficient / 10000f));
				if(null != _sb)
				{
					_sb.append("\nsleepPropertyPer:").append(sleepPropertyPer);
					_sb.append("\nsleepIndex:").append(sleepIndex);
					_sb.append("\nfinalSleepIndex:").append(finalSleepIndex);
					_sb.append("\nsleep:").append(sleep);
				}
			}
			
			//火星属性_健康指数万分比加成
			long propertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.HEALTH_ADD_PER);
			//健康指数=（氧气指数*氧气系数+饱腹指数*氧气系数+睡眠指数*睡眠系数）*（1+修正系数(暂不用)+火星属性_健康指数万分比加成）
			value = (long) ((oxygen + satiety + sleep) * (1 + 1.0f * propertyPer / 10000f));
			if(null != _sb)
			{
				_sb.append("\npropertyPer:").append(propertyPer);
				_sb.append("\nvalue:").append(value);
			}
			
		} while(false);
		
		_userData.getMarsBuildingComponent().setHealthIndex(value);
	}
	
	/**
	 * 幸福指数计算
	 * 
	 * 幸福指数=（舒适指数*舒适系数+心情指数*心情系数）*(1+火星属性_幸福指数万分比加成)
	 * 
	 * @param _userData
	 * @param _sb
	 */
	public static void calHappyIndex(NPUSUserData _userData, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n------------------ cal happy index start ------------------");
		}
		
		long value = 0;
		
		do
		{
			//每人口消耗数
			long peopleSum = _userData.getMarsPeopleComponent().getNumInfo().getPeopleSum();
			if(null != _sb)
			{
				_sb.append("\npeopleSum:").append(peopleSum);
			}
			
			//舒适指数*舒适系数
			long comfort = 0;
			//（当前居民舱A产出舒适值*居民舱A舒适值等级修正系数+当前居民舱B产出舒适值*居民舱B舒适值等级修正系数+...）
			long comfortAllBuildingValue = _userData.getMarsBuildingComponent().getAllBuildingValue().getComfortValue();
			if(null != _sb)
			{
				_sb.append("\ncomfortAllBuildingValueSum:").append(comfortAllBuildingValue);
			}
			//（每人口消耗睡眠值*人口数）
			long comfortPeopleValue = peopleSum * RefGeneral.Ref().mars_building_comfort_yield_per_consume;
			if(null != _sb)
			{
				_sb.append("\ncomfortPeopleValue:").append(comfortAllBuildingValue);
			}
			if(comfortAllBuildingValue > 0 && comfortPeopleValue > 0)
			{
				//火星属性_舒适指数万分比加成
				long comfortPropertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.COMFORT_ADD_PER);
				//*舒适指数=[（当前居民舱A产出舒适值*居民舱A舒适值等级修正系数+当前居民舱B产出舒适值*居民舱B舒适值等级修正系数+...）/（每人口消耗舒适值*人口数）] *（1+火星属性_舒适指数万分比加成）
				long comfortIndex = (long) ((1.0f * comfortAllBuildingValue / comfortPeopleValue) * (1 + 1.0f * comfortPropertyPer / 10000f));
				long finalComfortIndex = Math.min(comfortIndex, RefGeneral.Ref().mars_building_comfort_index_max);//计算结果受上限值限制
				//舒适指数*舒适系数
				comfort = (long) (finalComfortIndex * (1.0f * RefGeneral.Ref().mars_building_comfort_coefficient / 10000f));
				if(null != _sb)
				{
					_sb.append("\ncomfortPropertyPer:").append(comfortPropertyPer);
					_sb.append("\ncomfortIndex:").append(comfortIndex);
					_sb.append("\nfinalComfortIndex:").append(finalComfortIndex);
					_sb.append("\ncomfort:").append(comfort);
				}
			}
			
			//心情指数*心情系数
			long mood = 0;
			//（当前居民舱A产出心情值*居民舱A心情值等级修正系数+当前居民舱B产出心情值*居民舱B心情值等级修正系数+...）
			long moodAllBuildingValue = _userData.getMarsBuildingComponent().getAllBuildingValue().getMoodValue();
			if(null != _sb)
			{
				_sb.append("\nmoodAllBuildingValueSum:").append(moodAllBuildingValue);
			}
			//（每人口消耗舒适值*人口数）
			long moodPeopleValue = peopleSum * RefGeneral.Ref().mars_building_mood_yield_per_consume;
			if(null != _sb)
			{
				_sb.append("\nmoodPeopleValue:").append(moodPeopleValue);
			}
			//额外心情值（来自效果）
			long moodIndex = 0;
			long extMoodIndex = _userData.getMarsBuildingComponent().getCompInfo().getExtMoodIndex();
			if(null != _sb)
			{
				_sb.append("\nextMoodIndex:").append(extMoodIndex);
			}
			moodIndex += extMoodIndex;
			//计算建筑获取的心情值
			if(moodAllBuildingValue > 0 && moodPeopleValue > 0)
			{
				//火星属性_心情指数万分比加成
				long moodPropertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.MOOD_ADD_PER);
				//*心情指数=[（当前居民舱A产出心情值*居民舱A心情值等级修正系数+当前居民舱B产出心情值*居民舱B心情值等级修正系数+...）/（每人口消耗心情值*人口数）]* （1+火星属性_心情指数万分比加成）
				moodIndex += (long) ((1.0f * moodAllBuildingValue / moodPeopleValue) * (1 + 1.0f * moodPropertyPer / 10000f));
				if(null != _sb)
				{
					_sb.append("\nmoodPropertyPer:").append(moodPropertyPer);
				}
			}
			long finalMoodIndex = Math.min(moodIndex, RefGeneral.Ref().mars_building_mood_index_max);//计算结果受上限值限制
			//心情指数*心情系数
			mood = (long) (finalMoodIndex * (1.0f * RefGeneral.Ref().mars_building_mood_coefficient / 10000f));
			if(null != _sb)
			{
				_sb.append("\nmoodIndex:").append(moodIndex);
				_sb.append("\nfinalMoodIndex:").append(finalMoodIndex);
				_sb.append("\nmood:").append(mood);
			}
			
			//火星属性_幸福指数万分比加成
			long propertyPer = _userData.getMarsComponent().getMarsPropertyMgr().getValue(EMarsPropertyType.HAPPY_ADD_PER);
			//幸福指数=（舒适指数*舒适系数+心情指数*心情系数）*(1+火星属性_幸福指数万分比加成)
			value = (long) ((comfort + mood) * (1 + 1.0f * propertyPer / 10000f));
			if(null != _sb)
			{
				_sb.append("\npropertyPer:").append(propertyPer);
				_sb.append("\nvalue:").append(value);
			}
			
		} while(false);
		
		_userData.getMarsBuildingComponent().setHappyIndex(value);
	}
	
	/**
	 * 医疗室治愈率计算
	 * 
		根据同时治愈的居民数量范围(在范围内纯随机),按治愈间隔时间,以医疗室总治愈率,去计算每个生病居民是否治愈		
		医疗室总治愈率=医疗室所有部件提供的治愈率之和+医疗室派遣居民数量*每居民治愈率		
	 	*部件提供的治愈率,在mars_equipment_hospital_level表中配置	
	 	*每居民治愈率,在general表中配置	
		同时治愈的数量范围=所有部件提供的数量范围总和,部件提供的治愈数量范围在mars_equipment_hospital_level表中配置		
		治愈间隔时间,在general表中配置
	 * 
	 * @param _userData
	 * @return
	 */
	public static void calCureRate(NPUSUserData _userData, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n------------------ cal cure rate start ------------------");
		}
		
		//医疗室总治愈率=医疗室所有部件提供的治愈率之和+医疗室派遣居民数量*每居民治愈率
		long buildingCureSum = _userData.getMarsBuildingComponent().getAllBuildingValue().getCureRate();
		if(null != _sb)
		{
			_sb.append("\nbuildingCureSum:").append(buildingCureSum);
		}
		long hospitalNum = _userData.getMarsBuildingComponent().getPeopleBuildingFuncMgr().dispatchedSum(EMarsBuildingType.HOSPITAL);
		if(null != _sb)
		{
			_sb.append("\nhospitalNum:").append(hospitalNum);
		}
		long buildingCureRate = (long) (buildingCureSum + hospitalNum * RefGeneral.Ref().mars_building_people_cure_rate);
		if(null != _sb)
		{
			_sb.append("\nbuildingCurePer:").append(buildingCureRate);
		}
		
		_userData.getMarsBuildingComponent().setCureRate(buildingCureRate);
	}
}
