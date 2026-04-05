using ALPackage;
using System;
using System.Collections.Generic;
using Common.GuildEnum;

namespace GOE
{
	/// <summary>
	/// 联盟等级表
	/// </summary>
	[Serializable]
	public class GuildLevelRefObj : _IALBasicRefObj
	{
		public long _refId { get { return level; } }
		public long level;//联盟等级
        public long need_exp;//升级所需经验
        public long member_limit;//联盟人员总数量上限
        public List<_NNPCommonEnumLongInfo<EGuildPositionType>> position_limit_list;//职位数量限制列表
        public List<NPCommonCostItem> mail_reward_list;//升级邮件奖励列表
        public List<string> preview_desc_list;//等级预览描述列表
        public List<string> preview_desc_value_list;//等级预览描述参数列表
        public long gain_box_need_active_point;  //获取活跃宝箱所需活跃点
        public long gain_guild_box_id; //活跃宝箱id
        public long construct_gain_guild_exp_limit;//金币建设获得联盟经验上限
        public long construct_gain_guild_wealth_limit;//金币建设获得联盟财富上限
    }

	public class GSOGuildLevelRefSet : _TALSOBasicRefSet<GuildLevelRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
		public static string objName { get { return "guild_level"; } }
	}
}