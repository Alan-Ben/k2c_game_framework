using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 旅店设施等级表
	/// </summary>
	[Serializable]
	public class InnStationLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }

		public long id;
		public long station_id; // 设施 id
		public int level; // 等级
		public NPCommonCostItem upgrade_cost; // 升级消耗
		public long popularity_add; // 做菜人气加成
		public long affection_add; // 做菜心意值加成
		public long finesse_add; // 做菜熟练度加成
		public string level_up_tip; // 升级提示
		public List<string> level_up_tip_params; // 升级提示
		
		
#if NP_GAME
		
		public string levelUpTipTranslated { get { return TextTranslate.instance.getLanguage(level_up_tip, level_up_tip_params); } }
		
#endif
	}

	public class GSOInnStationLevelRefSet : _TALSOBasicRefSet<InnStationLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_station_level"; } }
	}
}