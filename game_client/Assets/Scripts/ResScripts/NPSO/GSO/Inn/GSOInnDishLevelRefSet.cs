using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 旅店菜品等级表
	/// </summary>
	[Serializable]
	public class InnDishLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long group_id; // 菜品组 id
		public int level; // 等级
		public long up_need_finesse; // 升级所需熟练度
	}

	public class GSOInnDishLevelRefSet : _TALSOBasicRefSet<InnDishLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_dish_level"; } }
	}
}