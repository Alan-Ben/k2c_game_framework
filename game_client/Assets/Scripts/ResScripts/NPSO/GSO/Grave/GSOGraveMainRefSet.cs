using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 杰出者大厅
	/// </summary>
	[Serializable]
	public class GraveMainRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id; //展厅id	
		public _NPPlayerConditionSerializeInfo show_condition; //展示条件	
		public List<long> type_id_list; //对应的杰出类型id list	
	}

	public class GSOGraveMainRefSet : _TALSOBasicRefSet<GraveMainRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/grave_refdata.unity3d"; } }
		public static string objName { get { return "grave_main"; } }
	}
}