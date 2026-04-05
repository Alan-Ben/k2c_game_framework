using ALPackage;
using System;
using System.Collections.Generic;
using Common.GuildEnum;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 联盟建设表
	/// </summary>
	[Serializable]
	public class GuildConstructRefObj : _IALBasicRefObj
	{
		public long _refId { get { return id; } }
		public long id;
        public EGuildConstructType type;//类型
        public string name;//名称
        public NPGTextureIndex icon;//图标（没配置读取cost的图标）
        public EQuality quality;//品质（用于获取品质框，没有配置读取cost的品质框）
		public long fix_cd_id;//fixCd消耗
        public _NPPlayerConditionSerializeInfo free_condition;//免建设花费条件
        public string free_condition_desc;//免建设花费条件描述
        public NPCommonCostItem cost;//建设花费
        public long add_guild_exp;//增加联盟经验
        public long add_guild_wealth;//增加联盟财富
        public long add_devote;//增加个人贡献
        public long add_personal_guild_coin;//个人联盟币
        public int add_reward_point;//增加奖励进度
	}

	public class GSOGuildConstructRefSet : _TALSOBasicRefSet<GuildConstructRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_construct"; } }
	}
}