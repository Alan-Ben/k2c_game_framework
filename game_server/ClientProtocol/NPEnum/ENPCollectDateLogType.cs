using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 日志收集数据类型
/// </summary>
public enum ENPCollectDateLogType {
	NONE, //0 ==== 
	[InspectorName("LOGIN - idx[1] - 登录日志")]
	LOGIN, //1 ==== 登录日志
	[InspectorName("CREATE - idx[2] - 创角日志")]
	CREATE, //2 ==== 创角日志
	[InspectorName("ONLINE - idx[3] - 在线日志")]
	ONLINE, //3 ==== 在线日志
	[InspectorName("RECHARGE - idx[4] - 充值日志")]
	RECHARGE, //4 ==== 充值日志
	[InspectorName("ITEM - idx[5] - 物品日志")]
	ITEM, //5 ==== 物品日志
	[InspectorName("ADMOB - idx[6] - 广告日志")]
	ADMOB, //6 ==== 广告日志
}

public class ENPCollectDateLogTypeComparer : IEqualityComparer<ENPCollectDateLogType>{
	public bool Equals(ENPCollectDateLogType x, ENPCollectDateLogType y) { return x == y; }
	public int GetHashCode(ENPCollectDateLogType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 7;
}
}

