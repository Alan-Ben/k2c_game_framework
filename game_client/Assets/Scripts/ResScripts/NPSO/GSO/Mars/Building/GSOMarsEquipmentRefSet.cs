using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星部件
	/// </summary>
	[Serializable]
	public class MarsEquipmentRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public string name;
		public string desc;
		public NPGTextureIndex icon;
		public long upgrade_group_id;
		public int unlock_level;
		public float level_limit_ratio;

		public int getMaxLevel(int _buildingLevel)
		{
			int maxLevel = (int)(_buildingLevel * level_limit_ratio);
			if (maxLevel < 1) maxLevel = 1;
			return maxLevel;
		}
	}

	public class GSOMarsEquipmentRefSet : _TALSOBasicRefSet<MarsEquipmentRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_equipment"; } }
	}
}
