using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 杰出者类型
	/// </summary>
	[Serializable]
	public class GraveTypeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public int id; //杰出者类型id
		public string type_name; //名称
		public long ui_path_id; //UI预制体id
		public List<long> player_title_id_list; //对应的玩家称号id列表
	}

	public class GSOGraveTypeRefSet : _TALSOBasicRefSet<GraveTypeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/grave_refdata.unity3d"; } }
		public static string objName { get { return "grave_type"; } }
	}
}