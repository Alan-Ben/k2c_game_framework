using ALPackage;
using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 太空寻宝 - 矿石表
	/// </summary>
	[Serializable]
	public class TreasureHuntOreRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public long normal_skill_id;//矿石普通技能id
		public long advanced_skill_id;//矿石高级技能id
		public List<TreasureHuntOreMassRewardGradeInfo> mass_reward_grade_list;//矿石质量奖励列表(在配表初始化完成后会按照质量从小到大排序一次)
		public EQuality quality;//品质
		public string name;//名称
		public NPGTextureIndex icon;//图标
		public NPGTextureIndex advanced_ore_icon;//高级矿石图标
		public string desc;//描述
		public string not_get_desc;//未获得时描述
		public List<string> not_get_desc_args_list;//未获得时描述参数列表
		public List<long> catalog_tab_id_list;//所属图鉴页签id列表

#if NP_GAME
		/// <summary>
		/// 是否达到了高级矿石的质量
		/// </summary>
		/// <param name="_mass"></param>
		/// <returns></returns>
		public bool isReachAdvanceOreMass(int _mass)
		{
			int advanceOreNeedGradeIndex = GRefdataCoreMgr.instance.npGeneral.treasure_hunt_advanced_ore_min_grade;
			if (advanceOreNeedGradeIndex < 0)
				return true;

			if (mass_reward_grade_list == null || advanceOreNeedGradeIndex >= mass_reward_grade_list.Count)
				return false;
		
			TreasureHuntOreMassRewardGradeInfo gradeInfo = mass_reward_grade_list[advanceOreNeedGradeIndex];
			return gradeInfo != null && _mass >= gradeInfo.mass;
		}
#endif
		
	}

	public class GSOTreasureHuntOreRefSet : _TALSOBasicRefSet<TreasureHuntOreRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/treasure_hunt_refdata.unity3d"; } }
		public static string objName { get { return "treasure_hunt_ore"; } }
	}
}