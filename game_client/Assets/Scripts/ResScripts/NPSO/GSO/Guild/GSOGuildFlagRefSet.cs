using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 联盟旗帜表
	/// </summary>
	[Serializable]
	public class GuildFlagRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public NPGTextureIndex icon;//图标
    }

	public class GSOGuildFlagRefSet : _TALSOBasicRefSet<GuildFlagRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_flag"; } }
	}
}