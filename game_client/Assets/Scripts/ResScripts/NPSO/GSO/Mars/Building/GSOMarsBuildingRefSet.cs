using ALPackage;
using System;
using System.Collections.Generic;
using Common.MarsEnum;

namespace GOE
{
	/// <summary>
	/// 火星建筑
	/// </summary>
	[Serializable]
	public class MarsBuildingRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public EMarsBuildingType building_type;
		public long upgrade_group_id;
		public long settle_group_id;
		public string name;
		public List<string> name_args;
		public string desc;
		public int building_order;
		public _NPPlayerConditionSerializeInfo build_btn_show_condition;
		public List<long> build_condition_id_list;
		public List<NPCommonCostItem> build_cost_list;
		public long build_time_cost_sec;
		public NPGGoIndex unbuilt_res_index;
		public NPGGoIndex building_res_index;
		public NPGGoIndex built_res_index;
		public NPGTextureIndex building_icon;// 建筑图标
		public int name_ui_path_id;
		
		public long upgrade_sfx_id; // 升级特效 id
		
		[NonSerialized, ALAutoExportVariableAttr(true, true, true)]
		public int max_slot_num;
		[NonSerialized, ALAutoExportVariableAttr(true, true, true)]
		public List<int> slot_unlock_levels;
		[NonSerialized, ALAutoExportVariableAttr(true, true, true)]
		public int building_order_in_number;
    }

	public class GSOMarsBuildingRefSet : _TALSOBasicRefSet<MarsBuildingRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_building"; } }
	}
}
