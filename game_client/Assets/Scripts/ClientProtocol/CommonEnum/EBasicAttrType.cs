using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 基础属性类型
/// </summary>
public enum EBasicAttrType {
	NONE, //0 ==== 
	[InspectorName("POWER - idx[1] - 实力")]
	POWER, //1 ==== 实力
	[InspectorName("TALENT - idx[2] - 资质")]
	TALENT, //2 ==== 资质
	[InspectorName("POWER_PER - idx[3] - 实例万分比加成")]
	POWER_PER, //3 ==== 实例万分比加成
}

public class EBasicAttrTypeComparer : IEqualityComparer<EBasicAttrType>{
	public bool Equals(EBasicAttrType x, EBasicAttrType y) { return x == y; }
	public int GetHashCode(EBasicAttrType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

