using ALPackage;
using System;

namespace GOE
{
	/// <summary>
	/// 公会协作据点表
	/// </summary>
	[Serializable]
	public class GuildCooperateAreaPosRefObj : _IALBasicRefObj
	{
		public long _refId { get { return pos_id; } }
		public long pos_id;//奖励据点id
        public NPGTextureIndex icon;//图标
		public string name;//名称
		public string desc;//描述
        public string desc_args;//描述参数
    }

	public class GSOGuildCooperateAreaPosRefSet : _TALSOBasicRefSet<GuildCooperateAreaPosRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_cooperate_refdata.unity3d"; } }
		public static string objName { get { return "guild_cooperate_area_pos"; } }
	}
}