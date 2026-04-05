using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
	/// <summary>
	/// 公会协作区域表
	/// </summary>
	[Serializable]
	public class GuildCooperateAreaRefObj : _IALBasicRefObj
	{
		public long _refId { get { return area_id; } }
		public long area_id;
        public string area_name;//区域名称
        public long bg_ui_res_id;//区域背景资源id
        public long reward_point_num;//奖励据点数量
        public List<long> property_point_hp;//属性据点血量
        public long reward_point_guild_wealth_reward;//奖励据点工会财富奖励值(公会奖励)
        public List<NPCommonCostItem> reward_point_reward_list;//奖励据点奖励列表(固定)
        public List<NPCommonCostItem> reward_point_random_reward_list;//奖励据点奖励列表(随机)
        public List<long> pos_id_list;//需要随机的据点id列表，按奖励据点数量随机
    }

	public class GSOGuildCooperateAreaRefSet : _TALSOBasicRefSet<GuildCooperateAreaRefObj> 
	{
		/************
		 * 资源加载路径
		 **/
		public static string assetPath { get { return "refdata/guild_cooperate_refdata.unity3d"; } }
		public static string objName { get { return "guild_cooperate_area"; } }
	}
}