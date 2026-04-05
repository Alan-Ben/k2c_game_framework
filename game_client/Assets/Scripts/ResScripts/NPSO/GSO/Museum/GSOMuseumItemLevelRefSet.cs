using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 博物馆表
	/// </summary>
	[Serializable]
	public class MuseumItemLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long museum_id;
		public int level;
		public _UnionBonusSerializeInfo bonus;
		public string bonus_desc_key;
		public List<string> bonus_desc_params;
		public string next_bonus_desc_key;
		public List<string> next_bonus_desc_params;
	}

	public class GSOMuseumItemLevelRefSet : _TALSOBasicRefSet<MuseumItemLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/museum_refdata.unity3d"; } }
		public static string objName { get { return "museum_item_level"; } }
	}
}