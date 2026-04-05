using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 游戏登录方式，与文档账号类型相同 https\:\/\/thoughts.teambition.com\/share\/647e8f5e90a37b003eb87cf4
/// </summary>
public enum ENPLoginWayType {
	NONE, //0 ==== 
	[InspectorName("GUEST - idx[1] - 游客")]
	GUEST, //1 ==== 游客
	[InspectorName("GAMECENTER - idx[2] - 游戏圈账号登录  -- iOS支持 android不支持")]
	GAMECENTER, //2 ==== 游戏圈账号登录  -- iOS支持 android不支持
	[InspectorName("IOS - idx[3] - Apple账号登录    -- iOS支持 android不支持")]
	IOS, //3 ==== Apple账号登录    -- iOS支持 android不支持
	[InspectorName("GOOGLE - idx[4] - 谷歌账号登录    -- iOS不支持 android支持")]
	GOOGLE, //4 ==== 谷歌账号登录    -- iOS不支持 android支持
	[InspectorName("FACEBOOK - idx[5] - facebook账号登录")]
	FACEBOOK, //5 ==== facebook账号登录
	[InspectorName("VK - idx[6] - vk账号登录")]
	VK, //6 ==== vk账号登录
	[InspectorName("LINE - idx[7] - line账号登录")]
	LINE, //7 ==== line账号登录
	[InspectorName("TWITTER - idx[8] - twitter账号登录")]
	TWITTER, //8 ==== twitter账号登录
	[InspectorName("MJ - idx[9] - 梦加账号登录")]
	MJ, //9 ==== 梦加账号登录
}

public class ENPLoginWayTypeComparer : IEqualityComparer<ENPLoginWayType>{
	public bool Equals(ENPLoginWayType x, ENPLoginWayType y) { return x == y; }
	public int GetHashCode(ENPLoginWayType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 10;
}
}

