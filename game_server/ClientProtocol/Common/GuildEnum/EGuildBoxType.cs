using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.GuildEnum
{

/// <summary>
/// 联盟宝箱类型
/// </summary>
public enum EGuildBoxType {
	NONE, //0 ==== 
	[InspectorName("GUILD_ACTIVE_BOX - idx[1] - 联盟辉煌宝箱")]
	GUILD_ACTIVE_BOX, //1 ==== 联盟辉煌宝箱
	[InspectorName("GUILD_FREE_BOX - idx[2] - 免费赠礼宝箱")]
	GUILD_FREE_BOX, //2 ==== 免费赠礼宝箱
	[InspectorName("GUILD_GIFT_BOX - idx[3] - 盟友等级宝箱")]
	GUILD_GIFT_BOX, //3 ==== 盟友等级宝箱
}

public class EGuildBoxTypeComparer : IEqualityComparer<EGuildBoxType>{
	public bool Equals(EGuildBoxType x, EGuildBoxType y) { return x == y; }
	public int GetHashCode(EGuildBoxType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

