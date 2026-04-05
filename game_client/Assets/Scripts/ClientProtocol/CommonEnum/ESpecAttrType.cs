using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 特长属性类型
/// </summary>
public enum ESpecAttrType {
	NONE, //0 ==== 
	[InspectorName("TYPE_A - idx[1] - 属性A")]
	TYPE_A, //1 ==== 属性A
	[InspectorName("TYPE_B - idx[2] - 属性B")]
	TYPE_B, //2 ==== 属性B
	[InspectorName("TYPE_C - idx[3] - 属性C")]
	TYPE_C, //3 ==== 属性C
	[InspectorName("TYPE_D - idx[4] - 属性D")]
	TYPE_D, //4 ==== 属性D
	[InspectorName("TYPE_E - idx[5] - 属性E")]
	TYPE_E, //5 ==== 属性E
}

public class ESpecAttrTypeComparer : IEqualityComparer<ESpecAttrType>{
	public bool Equals(ESpecAttrType x, ESpecAttrType y) { return x == y; }
	public int GetHashCode(ESpecAttrType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}

