using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 藏品表
	/// </summary>
	[Serializable]
	public class EquipRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long sort_id;//排序id
        public long num_limit;//数量上限
        public long level_limit;//等级上限
        public long initial_talent;//初始资质
        public long initial_skill_num;//初始技能数
        public NPCommonCostItem cost_item;//升级消耗
        public long upgrade_increase_talent;//升级增加资质点数
        public long addition_quality_level;//额外品质等级（RED品质才有）
        public NPGTextureIndex quality_lvl_icon;//藏品品质等级图标（RED品质才有）
		public List<NPCommonCostItem> disassemble_get_item_list;//分解基础返还道具列表
	}

	public class GSOEquipRefSet : _TALSOBasicRefSet<EquipRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/equip_refdata.unity3d"; } }
		public static string objName { get { return "equip"; } }
	}
}