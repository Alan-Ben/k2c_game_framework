using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 服务器状态
/// </summary>
public enum EServerOnlineState {
	[InspectorName("OPEN - idx[0] - 在线-所有人可访问")]
	OPEN, //0 ==== 在线-所有人可访问
	[InspectorName("CLOSED - idx[1] - 维护-白名单可以访问")]
	CLOSED, //1 ==== 维护-白名单可以访问
	[InspectorName("TEMP_CLOSED - idx[2] - 测试-白名单可以访问")]
	TEMP_CLOSED, //2 ==== 测试-白名单可以访问
}

public class EServerOnlineStateComparer : IEqualityComparer<EServerOnlineState>{
	public bool Equals(EServerOnlineState x, EServerOnlineState y) { return x == y; }
	public int GetHashCode(EServerOnlineState obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

