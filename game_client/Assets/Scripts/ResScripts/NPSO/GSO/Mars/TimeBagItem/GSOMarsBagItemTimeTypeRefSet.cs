using ALPackage;
using System;
using Common.MarsEnum;

namespace GOE
{
	/// <summary>
	/// 火星道具时间类型表
	/// </summary>
	[Serializable]
	public class MarsBagItemTimeTypeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long) time_type; } }
		public EMarsBagItemUseTimeType time_type;

		public string name;//时间类型名称
		public NPCommonItem lack_show_gain_way_item;//缺少时显示的获取途径道具
	}

	public class GSOMarsBagItemTimeTypeRefSet : _TALSOBasicRefSet<MarsBagItemTimeTypeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_bag_item_time_type"; } }
	}
}
