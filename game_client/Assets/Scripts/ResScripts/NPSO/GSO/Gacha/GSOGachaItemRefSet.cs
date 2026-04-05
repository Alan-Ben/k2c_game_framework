using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 抽卡道具表
	/// </summary>
	[Serializable]
	public class GachaItemRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public long pool_id;//所属卡池id
		public NPCommonCostItem item;//道具

		public bool if_show;//抽到是否在公屏展示
	}

	public class GSOGachaItemRefSet : _TALSOBasicRefSet<GachaItemRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/gacha_refdata.unity3d"; } }
		public static string objName { get { return "gacha_item"; } }
	}
}