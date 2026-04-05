using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 游历妃子酒馆事件消耗表
	/// </summary>
	[Serializable]
	public class TravelEventConsortBarCostRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//唯一id

		public NPGTextureIndex icon;//选项的icon
		public string name;//选项的名字
		public NPCommonCostItem cost;//消耗道具
		public int add_like;//增加的好感度
		public int add_intimacy;
	}

	public class GSOTravelEventConsortBarCostRefSet : _TALSOBasicRefSet<TravelEventConsortBarCostRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_event_consort_bar_cost"; } }
	}
}