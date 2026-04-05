using ALPackage;
using System;
using Common.MarsEnum;

namespace GOE
{
	/// <summary>
	/// 火星所有建筑表
	/// </summary>
	[Serializable]
	public class MarsAllBuildingRefObj : _IALBasicRefObj
	{
		public long _refId { get { return building_id; } }
		public long building_id;
		
		public EMarsBuildingType building_type;
		
		public long guide_hand_ui_res_id; // 引导手指资源 id
	}

	public class GSOMarsAllBuildingRefSet : _TALSOBasicRefSet<MarsAllBuildingRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_all_building"; } }
	}
}
