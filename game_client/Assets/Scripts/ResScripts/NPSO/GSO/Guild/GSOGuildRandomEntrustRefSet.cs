using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 联盟杂物委托表
	/// </summary>
	[Serializable]
	public class GuildRandomEntrustRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;//杂物委托事件id

		public NPGTextureIndex banner_tex;//banner图片
		public string name;//事件名称
		public string desc;//事件描述
	}

	public class GSOGuildRandomEntrustRefSet : _TALSOBasicRefSet<GuildRandomEntrustRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_random_entrust"; } }
	}
}