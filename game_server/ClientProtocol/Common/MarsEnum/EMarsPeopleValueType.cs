using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星居民指数类型
/// </summary>
public enum EMarsPeopleValueType {
	NONE, //0 ==== 
	[InspectorName("HEALTH - idx[1] - 健康指数")]
	HEALTH, //1 ==== 健康指数
	[InspectorName("HAPPY - idx[2] - 幸福指数")]
	HAPPY, //2 ==== 幸福指数
}

public class EMarsPeopleValueTypeComparer : IEqualityComparer<EMarsPeopleValueType>{
	public bool Equals(EMarsPeopleValueType x, EMarsPeopleValueType y) { return x == y; }
	public int GetHashCode(EMarsPeopleValueType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

