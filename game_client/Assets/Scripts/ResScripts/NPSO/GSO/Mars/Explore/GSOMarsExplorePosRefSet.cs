using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星探索位置表
	/// </summary>
	[Serializable]
	public class MarsExplorePosRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		public long march_time;
		public long distance;
	}

	public class GSOMarsExplorePosRefSet : _TALSOBasicRefSet<MarsExplorePosRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_explore_pos"; } }
	}
}
