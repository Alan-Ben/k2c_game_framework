using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星居民帮助类型
/// </summary>
public enum EMarsPeopleHelpType {
	NONE, //0 ==== 
	[InspectorName("REWARD - idx[1] - 直接奖励")]
	REWARD, //1 ==== 直接奖励
	[InspectorName("CHOICE - idx[2] - 需要选择回答")]
	CHOICE, //2 ==== 需要选择回答
}

public class EMarsPeopleHelpTypeComparer : IEqualityComparer<EMarsPeopleHelpType>{
	public bool Equals(EMarsPeopleHelpType x, EMarsPeopleHelpType y) { return x == y; }
	public int GetHashCode(EMarsPeopleHelpType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

