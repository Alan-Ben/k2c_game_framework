using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.PrivilegeCardEnum
{

/// <summary>
/// 权益卡类型
/// </summary>
public enum EPrivilegeCardType {
	NONE, //0 ==== 
	[InspectorName("MONTH - idx[1] - 月卡")]
	MONTH, //1 ==== 月卡
	[InspectorName("YEAR - idx[2] - 年卡")]
	YEAR, //2 ==== 年卡
}

public class EPrivilegeCardTypeComparer : IEqualityComparer<EPrivilegeCardType>{
	public bool Equals(EPrivilegeCardType x, EPrivilegeCardType y) { return x == y; }
	public int GetHashCode(EPrivilegeCardType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

