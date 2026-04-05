using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
	/// <summary>
	/// 火星主基地等级
	/// </summary>
	[Serializable]
	public class MarsBuildingHomeLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return level; } }
		public int level;
		public int energy_consume_per_min;
		public int overdrive_energy_consume_per_min;
		public int on_oxygen_yield;
		public int overdrive_oxygen_yield;
		public int off_oxygen_yield;
		public int oxygen_yield_adjust_coefficient;
		public NPGTextureIndex icon;
        public List<NPCommonCostItem> output_per_min;
    }

	public class GSOMarsBuildingHomeLevelRefSet : _TALSOBasicRefSet<MarsBuildingHomeLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_building_home_level"; } }
	}
}
