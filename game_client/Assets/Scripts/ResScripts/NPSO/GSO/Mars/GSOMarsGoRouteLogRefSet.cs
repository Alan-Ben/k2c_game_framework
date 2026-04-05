using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 火星航行日志表
	/// </summary>
	[Serializable]
	public class MarsGoRouteLogRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//日志id
        public string log_title;//日志标题
		public string log_title_args;//日志标题参数
        public string log_content;//日志内容
		public string log_content_args;//日志内容参数
    }

	public class GSOMarsGoRouteLogRefSet : _TALSOBasicRefSet<MarsGoRouteLogRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_go_route_log"; } }
	}
}