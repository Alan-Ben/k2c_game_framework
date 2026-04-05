using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 爬塔关卡表
	/// </summary>
	[Serializable]
	public class TowerChapterRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public string name;//章节名称
		public string chapter_stage_name;//关卡名称
		public string chapter_stage_top_name;//关卡顶层名称
		public List<NPCommonCostItem> gain_item_list;//到达该章节的奖励物品列表
		public long initial_stage_id;//起始stage_id
		public NPGTextureIndex banner_tex;//banner背景
		public NPGTextureIndex boss_icon;//	boss形象Icon	
		public string boss_name;//boss名称	
		public bool if_pve_chapter;//是否单机章节
		public NPGGoIndex boss_go_index;//boss 场景 形象
		public List<GVideoClipIndex> pve_go_index;//pve 战斗视频
		public List<GVideoClipIndex> pvp_boss_go_index;//pvp 敌方 战斗视频
		public string chapter_unlock_desc;//章节解锁描述
		public List<string> chapter_unlock_desc_args;// 章节解锁描述参数

		public NPGGoIndex chapter_scene_go_index;//章节场景Go
		public List<NPCommonCostItem> research_finish_reward;//到达该章节的研究奖励物品列表

		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public List<TowerChapterStageRefObj> stage_list;//关卡id列表
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public int level_count; //关卡总数
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		public int total_level_start; // 当前Stage 的总关卡数初始值


#if NP_GAME
	
		[NonSerialized][ALAutoExportVariableAttr(true, true, true)]
		private List<TowerResearchRefObj> _m_research_list = null;//研究列表
		public List<TowerResearchRefObj> research_list 
		{
			get
			{
				if (_m_research_list == null)
				{
					_initResearchList();
				}
				return _m_research_list;
			}
		}
		
		private void _initResearchList()
		{
			_m_research_list = new List<TowerResearchRefObj>();
			foreach (TowerResearchRefObj refObj in GRefdataCoreMgr.instance.towerResearchRefCore.refList)
			{
				if (refObj != null && refObj.chapter_id == id)
				{
					_m_research_list.Add(refObj);
				}
			}

			// 按照level从小到大排序
			_m_research_list.Sort((a, b) => a.level.CompareTo(b.level));
		}
		
#endif
		
		/// <summary>
		/// 获取关卡名字
		/// </summary>
		/// <param name="_level"></param>
		/// <returns></returns>
		public string getLevelName(int _level)
		{
			// if(_level == level_count)
			// 	return TextTranslate.instance.getLanguage(chapter_stage_top_name);
			return TextTranslate.instance.getLanguage(chapter_stage_name, getTotalLevel(_level));
		}
		
		// 每关的实力值=实力基础值*实力增长万分比^(当前关卡-关卡初始值)
		public long getBossPower(int _level)
		{
			TowerChapterStageRefObj stageRef = getStageByLevel(_level);
			if (stageRef != null) 
				return stageRef.getBossPower(_level - stageRef.level_count_initial_value);
			return 0;
		}
		
		/// <summary>
		/// 每日迷宫币产出值 每日迷宫币产出提升: 每一关的产出值= 每日迷宫币基础值+增长值*(当前关卡-关卡初始值)
		/// </summary>
		/// <returns></returns>
		public long getTowerCoinCount(int _level)
		{
			TowerChapterStageRefObj stageRef = getStageByLevel(_level);
			if (stageRef != null) 
				return stageRef.getTowerCoinCount(_level - stageRef.level_count_initial_value);
			return 0;
		}

		/// <summary>
		/// 获取总关卡的关卡数值
		/// </summary>
		/// <returns></returns>
		public long getTotalLevel(long _level)
		{
			return total_level_start + _level;
		}
		private TowerChapterStageRefObj getStageByLevel(int _level)
		{
			if (stage_list == null) return null;
			int count = 0;
			foreach (var stage in stage_list)
			{
				if(stage == null) continue;
				int lastCount = count;
				count += stage.levels_in_range_count;
				if (lastCount < _level && _level <= count)
				{
					return stage;
				}
			}
			return null;
		}
	}

	public class GSOTowerChapterRefSet : _TALSOBasicRefSet<TowerChapterRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/tower_refdata.unity3d"; } }
		public static string objName { get { return "tower_chapter"; } }
	}
}