using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 效果S_ADD_P_V_FORM的二级枚举
/// </summary>
public enum EEffectPlayerValueType {
	NONE, //0 ==== 
	[InspectorName("UNUSE_ANECDOTE - idx[1] - 已废弃-政务事件")]
	UNUSE_ANECDOTE, //1 ==== 已废弃-政务事件
	[InspectorName("UNUSE_TRAVEL_MESS - idx[2] - 已废弃-游历情报值")]
	UNUSE_TRAVEL_MESS, //2 ==== 已废弃-游历情报值
}

public class EEffectPlayerValueTypeComparer : IEqualityComparer<EEffectPlayerValueType>{
	public bool Equals(EEffectPlayerValueType x, EEffectPlayerValueType y) { return x == y; }
	public int GetHashCode(EEffectPlayerValueType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

