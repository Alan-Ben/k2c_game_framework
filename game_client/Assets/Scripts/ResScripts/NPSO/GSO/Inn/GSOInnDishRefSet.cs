using ALPackage;
using System;
using System.Collections.Generic;
using CommonEnum;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 旅店菜品表
	/// </summary>
	[Serializable]
	public class InnDishRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public int num;
		public _NPPlayerConditionSerializeInfo unlock_condition; // 解锁条件
		public List<long> need_station_id_list; // 所需的设施 id
		public long station_id; // 所属设施 id（决定菜品显示在哪个设施的菜品列表中）
		public long upgrade_group_id; // 升级组 id
		public ESpecAttrType basic_attr; // 菜品相性
		public PlayerBonusPropertyModifier bonus_prop_modifier_per_level; // 菜品每集相性收益加成
		public string name; // 名字
		public string desc;
		public NPGTextureIndex icon; // 图标
		public string unlock_tip;
		public List<string> unlock_tip_params;
		public NPGTextureIndex unlock_small_icon;
		public string unlock_small_tip;
		public List<string> unlock_small_tip_params;
		public _NPPlayerEffectSerializeInfo unlock_go_to;//跳转效果
		public NPCommonCostItem unlock_cost;
	}

	public class GSOInnDishRefSet : _TALSOBasicRefSet<InnDishRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_dish"; } }
	}
}