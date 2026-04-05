using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 性别类型
/// </summary>
public enum ENPGenderType {
	NONE, //0 ==== 
	[InspectorName("MALE - idx[1] - 男性")]
	MALE, //1 ==== 男性
	[InspectorName("FEMALE - idx[2] - 女性")]
	FEMALE, //2 ==== 女性
}

public class ENPGenderTypeComparer : IEqualityComparer<ENPGenderType>{
	public bool Equals(ENPGenderType x, ENPGenderType y) { return x == y; }
	public int GetHashCode(ENPGenderType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

