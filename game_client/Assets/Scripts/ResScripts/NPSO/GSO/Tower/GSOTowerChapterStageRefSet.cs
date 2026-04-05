using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 爬塔关卡阶段表
	/// </summary>
	[Serializable]
	public class TowerChapterStageRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long next_stage_id;					//下一个stage_id
		public int level_count_initial_value;		//关卡数初始值
		public int levels_in_range_count;			//区间内关卡数量
		public long diamond_reward_per_level;		//到达每关的钻石奖励
		public long tower_coin_reward_ratio_per_level;// 到达每关的迷宫币奖励万分比系数
		public long base_power_per_level;			//关卡实力基础值
		public long power_growth_ratio;				//实力增长万分比
		public long daily_tower_coin_output_base;	//	每日迷宫币产出基础值
		public long tower_coin_growth_value;		//	迷宫币增长值
		public long initial_gold_income_bonus_per_level;//	关卡初始金币收益加成
		public long average_gold_income_bonus_growth;	//	金币收益加成平均增长值

		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public int total_level_start; // 当前Stage 的总关卡数初始值
		
		// 到达每一关卡时会获得奖励:
		// 钻石奖励: stage内配固定值
		// 	每日迷宫币产出提升: 每一关的产出值= 每日迷宫币基础值+增长值*(当前关卡-关卡初始值)
		// 迷宫币奖励=到达当前关卡的每日迷宫币产出值*迷宫币奖励万分比系数
		//
		// 每关的实力值=实力基础值*实力增长万分比^(当前关卡-关卡初始值)
		public long getBossPower(long _level)
		{
			if (_level < 0)
				return 0;
			return (long)(base_power_per_level * Mathf.Pow((1 + power_growth_ratio / 10000f), Mathf.Max(_level, 0)));
		}

		// 每关的金币收益加成=关卡初始金币收益加成+金币收益增长值*(当前关卡-关卡初始值)
		public long getCoinBonus(long _level)
		{
			if (_level < 0)
				return 0;
			return (initial_gold_income_bonus_per_level + average_gold_income_bonus_growth * Math.Max(_level, 0));
		}
		
		/// <summary>
		/// 每日迷宫币产出值 每日迷宫币产出提升: 每一关的产出值= 每日迷宫币基础值+增长值*(当前关卡-关卡初始值)
		/// </summary>
		/// <returns></returns>
		public long getTowerCoinCount(long _level)
		{
			if (_level < 0)
				return 0;
			return daily_tower_coin_output_base + tower_coin_growth_value * Math.Max(_level, 0);
		}
	}

	public class GSOTowerChapterStageRefSet : _TALSOBasicRefSet<TowerChapterStageRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/tower_refdata.unity3d"; } }
		public static string objName { get { return "tower_chapter_stage"; } }
	}
}