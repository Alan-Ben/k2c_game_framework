using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 平台参数类型
/// </summary>
public enum EPlatParamType {
	[InspectorName("CLIENT_VERSION - idx[0] - 客户端资源版本")]
	CLIENT_VERSION, //0 ==== 客户端资源版本
	[InspectorName("HOT_REF_URL_BASE - idx[1] - 热更配表的CDN地址")]
	HOT_REF_URL_BASE, //1 ==== 热更配表的CDN地址
}

public class EPlatParamTypeComparer : IEqualityComparer<EPlatParamType>{
	public bool Equals(EPlatParamType x, EPlatParamType y) { return x == y; }
	public int GetHashCode(EPlatParamType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

