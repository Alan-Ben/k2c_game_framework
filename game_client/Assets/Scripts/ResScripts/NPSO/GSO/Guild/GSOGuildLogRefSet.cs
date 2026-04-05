using ALPackage;
using System;
using Common.GuildEnum;

namespace GOE
{
	/// <summary>
	/// 联盟日志表
	/// </summary>
	[Serializable]
	public class GuildLogRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)type; } }
        public EGuildLogType type;//操作行为类型
        public string name;//操作行为名
        public string desc;//描述
    }

	public class GSOGuildLogRefSet : _TALSOBasicRefSet<GuildLogRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_log"; } }
	}
}