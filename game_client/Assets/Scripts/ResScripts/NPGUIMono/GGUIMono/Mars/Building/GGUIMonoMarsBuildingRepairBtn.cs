using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 队伍带兵量占比显示物体
	/// </summary>
	[Serializable]
	public class MarsBuildingRepairTeamSoldierRatioConfig
	{
		[ALHeader("队伍带兵比例")]
		public float soldierRatio;
		
		[ALHeader("大于等于该比例时显示的物体列表")]
		public List<GameObject> greaterOrEqualShow;
	}
	
    public class GGUIMonoMarsBuildingRepairBtn : ALGGUIMonoCommonFollowItem
    {
		[ALHeader("修复按钮")]
		public GameObject btnRepair;

		[ALHeader("有未编队队伍时显示的物体列表(teamSoldierRatioConfigs配置显隐互斥)")]
		public List<GameObject> hasEmptyTeamShow;
		
		[ALHeader("队伍带兵量占比显示配置(与hasEmptyTeamShow配置显隐互斥)")]
		public List<MarsBuildingRepairTeamSoldierRatioConfig> teamSoldierRatioConfigList;
    }
} 