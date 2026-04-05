using ALPackage;
using System;
using Common.MarsEnum;

namespace GOE
{
	/// <summary>
	/// 火星居民求助表
	/// </summary>
	[Serializable]
	public class MarsPeopleHelpRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;

		public EMarsPeopleHelpType type;//求助类型

		public NPCommonAssetPathInfo item_prefab_asset_path;//item预制加载路径
		public string desc;//求助描述
	}

	public class GSOMarsPeopleHelpRefSet : _TALSOBasicRefSet<MarsPeopleHelpRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/mars_refdata.unity3d"; } }
		public static string objName { get { return "mars_people_help"; } }
	}
}
