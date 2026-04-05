using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 火星基地事件表
	/// </summary>
	[Serializable]
	public class MarsEventRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
		
		public string name;// 事件名
		public long buff_id;//buff id
		public NPGTextureIndex banner_img;//事件banner图
		public string desc;// 事件描述
		public List<long> access_id_list;//前往途径id列表
		public _NPPlayerConditionSerializeInfo need_show_tip_cond;//需要显示侧边tip条件
		public bool is_singleton_tip;//是否单例tip(同事件tip只存在一个)
	}

	public class GSOMarsEventRefSet : _TALSOBasicRefSet<MarsEventRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_event"; } }
	}
}
