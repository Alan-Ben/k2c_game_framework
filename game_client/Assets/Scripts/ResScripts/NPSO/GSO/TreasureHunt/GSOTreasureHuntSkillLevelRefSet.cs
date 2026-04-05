using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 技能等级表
	/// </summary>
	[Serializable]
	public class TreasureHuntSkillLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public long skill_id;//技能ID
		public int level; //技能等级
		public long upgrade_cost_num;//升级需要的消耗数量
		public NPPlayerPropertyModifier add_player;//玩家属性加成（ENPPlayerPropertyType）
		public _UnionBonusSerializeInfo union_bonus;//全局属性加成（EBonusPropertyType）
		public List<string> skill_desc_args_list;//技能描述参数列表
		public List<string> next_level_add_value_desc;//下一级加成数值描述
	}

	public class GSOTreasureHuntSkillLevelRefSet : _TALSOBasicRefSet<TreasureHuntSkillLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_skill_level"; } }
	}
}