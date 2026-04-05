using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 火星探索等级表
	/// </summary>
	[Serializable]
	public class MarsExploreLvlRefObj : _IALBasicRefObj
	{
		public long _refId { get { return explore_level; } }
		public int explore_level;
		public NPPlayerPropertyModifier player_property;
		public int upgrade_need_explore_num;
		public List<NPCommonCostItem> upgrade_gain_item_list;
		public int explore_event_exist_limit;
		public long map_scene_id;
		public List<long> pos_list;
		public List<CommonQualityWeight> refresh_event_quality_list;
        public List<long> battle_event_quality_solider_power_list;
        public List<long> battle_event_quality_solider_num_list;
        public List<CommonCostItemList> battle_event_quality_reward_list;
	}

	public class GSOMarsExploreLvlRefSet : _TALSOBasicRefSet<MarsExploreLvlRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_lvl"; } }
	}
}
