using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 游历妃子表
	/// </summary>
	[Serializable]
	public class TravelConsortRefObj : _IALBasicRefObj
	{
		public long _refId { get { return consort_id; } }
		public long consort_id;
		public int marry_need_like;//迎娶所需好感度
		public List<WCGPairIntLong> like_step_dialog_list;//好感度阶段对话列表(好感度:对话id), 在配表初始化完成后会按照好感度升序排序
		public List<long> normal_like_dialog_list;//普通好感度对话列表
		public List<long> bar_intimacy_dialog_list;//酒馆亲密度对话列表
	}

	public class GSOTravelConsortRefSet : _TALSOBasicRefSet<TravelConsortRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/travel_refdata.unity3d"; } }
		public static string objName { get { return "travel_consort"; } }
	}
}