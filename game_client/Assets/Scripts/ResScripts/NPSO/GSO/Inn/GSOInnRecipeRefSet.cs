using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 旅店表
	/// </summary>
	[Serializable]
	public class InnRecipeRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
	}

	public class GSOInnRecipeRefSet : _TALSOBasicRefSet<InnRecipeRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/inn_refdata.unity3d"; } }
		public static string objName { get { return "inn_recipe"; } }
	}
}