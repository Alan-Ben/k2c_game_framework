using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 旅店客人表
	/// </summary>
	[Serializable]
	public class InnGuestRefObj : _IALBasicRefObj
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
		public long finesse_add; // 熟练度加成
		public List<NPCommonCostItem> handbook_reward; // 图鉴奖励
		public NPGGoIndex td_res_index; // 场景里得资源
		public long unlock_dialogue_id;
	}

	public class GSOInnGuestRefSet : _TALSOBasicRefSet<InnGuestRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_guest"; } }
	}
}