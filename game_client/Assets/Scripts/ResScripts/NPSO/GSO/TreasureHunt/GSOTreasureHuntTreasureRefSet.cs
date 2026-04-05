using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 奇物表
	/// </summary>
	[Serializable]
	public class TreasureHuntTreasureRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public long skill_id;//奇物技能id
		public NPCommonItem upgrade_cost;//升级消耗的物品
		public string name;//名称
		public NPGTextureIndex icon;//图标
		public string desc;//描述
		public string not_get_desc;//未获得时描述
		public List<string> not_get_desc_args_list;//未获取时描述列表
		public EQuality quality;//品质
		public List<long> catalog_tab_id_list;//所属图鉴页签id列表
		public _NPPlayerConditionSerializeInfo unlock_condition;//解锁条件
		public long related_lab_id;//对应研究实验室id
		public NPGGoIndex td_go_index;//3D模型GoIndex
	}

	public class GSOTreasureHuntTreasureRefSet : _TALSOBasicRefSet<TreasureHuntTreasureRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_treasure"; } }
	}
}