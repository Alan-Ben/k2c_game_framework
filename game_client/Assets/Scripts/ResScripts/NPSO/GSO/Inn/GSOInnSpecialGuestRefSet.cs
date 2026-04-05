using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 旅店特殊客人表
	/// </summary>
	[Serializable]
	public class InnSpecialGuestRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public string name;
		public string desc;
		public NPGTextureIndex icon;
		public NPGTextureIndex small_icon;
		public List<_NPPlayerConditionSerializeInfo> unlock_condition_list; // 解锁条件
		public List<string> unlock_condition_desc_list; // 解锁条件描述
		public List<CommonStringList> unlock_condition_desc_params_list;
		public NPGGoIndex td_res_index;
		public List<NPCommonCostItem> handbook_reward;
		public long first_gain_gift_id;
		public long serve_dialogue_id;
		public long serve_done_dialogue_id;
		public long serve_choice_id;
	}

	public class GSOInnSpecialGuestRefSet : _TALSOBasicRefSet<InnSpecialGuestRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_special_guest"; } }
	}
}