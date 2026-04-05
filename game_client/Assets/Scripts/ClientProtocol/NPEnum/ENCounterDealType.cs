using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 计数器类型操作类型
/// </summary>
public enum ENCounterDealType {
	[InspectorName("ADD - idx[0] - 增加")]
	ADD, //0 ==== 增加
	[InspectorName("REDUCE - idx[1] - 减少")]
	REDUCE, //1 ==== 减少
	[InspectorName("SET - idx[2] - 设置")]
	SET, //2 ==== 设置
	[InspectorName("SET_GT - idx[3] - 只比当前值才设置")]
	SET_GT, //3 ==== 只比当前值才设置
}

public class ENCounterDealTypeComparer : IEqualityComparer<ENCounterDealType>{
	public bool Equals(ENCounterDealType x, ENCounterDealType y) { return x == y; }
	public int GetHashCode(ENCounterDealType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

