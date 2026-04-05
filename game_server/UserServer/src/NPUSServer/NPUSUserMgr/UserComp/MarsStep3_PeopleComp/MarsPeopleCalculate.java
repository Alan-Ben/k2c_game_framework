package NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp;

import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class MarsPeopleCalculate 
{
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
	public static int calCureNum(NPUSUserData _userData)
	{
		//医疗室总治愈率=医疗室所有部件提供的治愈率之和+医疗室派遣居民数量*每居民治愈率
		long buildingCurePer = _userData.getMarsBuildingComponent().getCureRate();
		//检查概率
		int rand = CommonFunc.randomInt(10000);
		if(rand > buildingCurePer)
			return 0;
		
		//计算治愈居民数区间
		return _userData.getMarsBuildingComponent().getAllBuildingValue().getCureNum().rand();
	}
}
