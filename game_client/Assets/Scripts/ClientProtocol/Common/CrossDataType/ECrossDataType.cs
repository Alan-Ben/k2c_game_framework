using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.CrossDataType
{

/// <summary>
/// 跨服数据类型枚举
/// </summary>
public enum ECrossDataType {
	NONE, //0 ==== 
	[InspectorName("MARS_MINE - idx[1] - 火星矿")]
	MARS_MINE, //1 ==== 火星矿
}

public class ECrossDataTypeComparer : IEqualityComparer<ECrossDataType>{
	public bool Equals(ECrossDataType x, ECrossDataType y) { return x == y; }
	public int GetHashCode(ECrossDataType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

