using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/**
	 * 章节金币消耗倍率配表
	 *
	 * 根据战力比例区间配置金币消耗的倍率
	 * 战力比例 = (玩家战力 - 关卡战力) / 关卡战力 * 10000
	 */
	[Serializable]
	public class ChapterCostRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;// id
		public int power_rate_start;//战力比例区间(万分比)，格式：min:max
		public int power_rate_end;//战力比例区间(万分比)，格式：min:max
			
		public int cost_multiple_rate;// 金币消耗倍率(万分比)
	}

	public class GSOChapterCostRefSet : _TALSOBasicRefSet<ChapterCostRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/chapter_refdata.unity3d"; } }
		public static string objName { get { return "chapter_cost"; } }
	}
}