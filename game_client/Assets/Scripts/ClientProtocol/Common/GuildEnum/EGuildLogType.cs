using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟日志类型
/// </summary>
public enum EGuildLogType {
	NONE, //0 ==== 
	[InspectorName("GUILD_CREATION - idx[1] - 联盟创建")]
	GUILD_CREATION, //1 ==== 联盟创建
	[InspectorName("MEMBER_JOIN - idx[2] - 新成员加入")]
	MEMBER_JOIN, //2 ==== 新成员加入
	[InspectorName("MEMBER_LEAVE - idx[3] - 老成员离开")]
	MEMBER_LEAVE, //3 ==== 老成员离开
	[InspectorName("GUILD_CONSTRUCTION - idx[4] - 联盟建设")]
	GUILD_CONSTRUCTION, //4 ==== 联盟建设
	[InspectorName("KICK_OUT_MEMBER - idx[5] - 踢出成员")]
	KICK_OUT_MEMBER, //5 ==== 踢出成员
	[InspectorName("POSITION_CHANGE - idx[6] - 职位变更")]
	POSITION_CHANGE, //6 ==== 职位变更
	[InspectorName("ANNOUNCEMENT_CHANGE - idx[7] - 公告变更")]
	ANNOUNCEMENT_CHANGE, //7 ==== 公告变更
	[InspectorName("GUILD_RENAME - idx[8] - 联盟改名")]
	GUILD_RENAME, //8 ==== 联盟改名
	[InspectorName("GUILD_FLAG_CHANGE - idx[9] - 联盟旗帜变更")]
	GUILD_FLAG_CHANGE, //9 ==== 联盟旗帜变更
	[InspectorName("ENABLE_FREE_JOIN - idx[10] - 开启自由加入")]
	ENABLE_FREE_JOIN, //10 ==== 开启自由加入
	[InspectorName("DISABLE_FREE_JOIN - idx[11] - 关闭自由加入")]
	DISABLE_FREE_JOIN, //11 ==== 关闭自由加入
	[InspectorName("GUILD_UPGRADE - idx[12] - 联盟升级")]
	GUILD_UPGRADE, //12 ==== 联盟升级
	[InspectorName("LEADER_TRANSFER - idx[13] - 盟主转让")]
	LEADER_TRANSFER, //13 ==== 盟主转让
}

public class EGuildLogTypeComparer : IEqualityComparer<EGuildLogType>{
	public bool Equals(EGuildLogType x, EGuildLogType y) { return x == y; }
	public int GetHashCode(EGuildLogType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 14;
}
}

