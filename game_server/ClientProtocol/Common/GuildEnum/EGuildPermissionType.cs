using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟权限类型
/// </summary>
public enum EGuildPermissionType {
	NONE, //0 ==== 
	[InspectorName("TRANSFER_LEADER - idx[1] - 盟主转让")]
	TRANSFER_LEADER, //1 ==== 盟主转让
	[InspectorName("DISSOLVE_GUILD - idx[2] - 解散联盟")]
	DISSOLVE_GUILD, //2 ==== 解散联盟
	[InspectorName("CHANGE_GUILD_INFO - idx[3] - 更改联盟信息")]
	CHANGE_GUILD_INFO, //3 ==== 更改联盟信息
	[InspectorName("POSITION_APPOINT - idx[4] - 职位任命")]
	POSITION_APPOINT, //4 ==== 职位任命
	[InspectorName("TOGGLE_FREE_JOIN - idx[5] - 开启/关闭自由加入")]
	TOGGLE_FREE_JOIN, //5 ==== 开启/关闭自由加入
	[InspectorName("PROCESS_JOIN_REQUEST - idx[6] - 处理入盟申请")]
	PROCESS_JOIN_REQUEST, //6 ==== 处理入盟申请
	[InspectorName("KICK_OUT - idx[7] - 踢出联盟")]
	KICK_OUT, //7 ==== 踢出联盟
	[InspectorName("BROADCAST_MESSAGE - idx[8] - 群发消息")]
	BROADCAST_MESSAGE, //8 ==== 群发消息
	[InspectorName("LEAVE_GUILD - idx[9] - 退出联盟")]
	LEAVE_GUILD, //9 ==== 退出联盟
	[InspectorName("GUILD_MANAGEMENT - idx[10] - 联盟管理")]
	GUILD_MANAGEMENT, //10 ==== 联盟管理
	[InspectorName("DAILY_CONSTRUCTION - idx[11] - 每日建设")]
	DAILY_CONSTRUCTION, //11 ==== 每日建设
	[InspectorName("VIEW_MEMBERS - idx[12] - 查看成员")]
	VIEW_MEMBERS, //12 ==== 查看成员
	[InspectorName("STORE_EXCHANGE - idx[13] - 商店兑换")]
	STORE_EXCHANGE, //13 ==== 商店兑换
	[InspectorName("LEADERBOARD - idx[14] - 排行榜")]
	LEADERBOARD, //14 ==== 排行榜
	[InspectorName("OPEN_RECRUITMENT - idx[15] - 开启招募")]
	OPEN_RECRUITMENT, //15 ==== 开启招募
	[InspectorName("SET_PVE_AUTO_STAR - idx[16] - 设置公会副本自动开启")]
	SET_PVE_AUTO_STAR, //16 ==== 设置公会副本自动开启
	[InspectorName("USE_GUILD_WEALTH - idx[17] - 使用联盟财富")]
	USE_GUILD_WEALTH, //17 ==== 使用联盟财富
	[InspectorName("USE_ITEM_START_PVE - idx[18] - 使用物品开启公会副本")]
	USE_ITEM_START_PVE, //18 ==== 使用物品开启公会副本
	[InspectorName("SET_PVE_MONSTER_TAG - idx[19] - 设置公会怪物标签")]
	SET_PVE_MONSTER_TAG, //19 ==== 设置公会怪物标签
	[InspectorName("SET_GUILD_COOPERATE_RECOMMEND_REWARD_POINT - idx[20] - 设置联盟协作推荐奖励据点")]
	SET_GUILD_COOPERATE_RECOMMEND_REWARD_POINT, //20 ==== 设置联盟协作推荐奖励据点
}

public class EGuildPermissionTypeComparer : IEqualityComparer<EGuildPermissionType>{
	public bool Equals(EGuildPermissionType x, EGuildPermissionType y) { return x == y; }
	public int GetHashCode(EGuildPermissionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 21;
}
}

