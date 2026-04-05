using ALPackage;
using System;
using System.Collections.Generic;
using Common.GuildEnum;

namespace GOE
{
	/// <summary>
	/// 联盟职位表
	/// </summary>
	[Serializable]
	public class GuildPositionRefObj : _IALBasicRefObj
	{
		public long _refId { get { return (long)type; } }
		public EGuildPositionType type;
        public string name;//名称
        public NPGTextureIndex icon;//图标
        public NPGSpriteIndex banner_spt;//职位banner图
		public EGuildPositionType pre_position;//前一个职位
        public List<EGuildPermissionType> permission_list;//权限列表
        public List<EGuildPositionType> can_appoint_position_list;//可任命的职位列表
        public int leader_passive_transfer_priority;//盟主被动转让优先级
        public long trans_need_historical_contributions;//任命所需历史贡献
	}

	public class GSOGuildPositionRefSet : _TALSOBasicRefSet<GuildPositionRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_position"; } }
	}
}