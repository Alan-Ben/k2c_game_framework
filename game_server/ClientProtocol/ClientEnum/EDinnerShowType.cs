using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace ClientEnum
{

/// <summary>
/// 宴会展示类型
/// </summary>
public enum EDinnerShowType {
	NONE, //0 ==== 
	[InspectorName("FAMILY - idx[1] - 妃子宴")]
	FAMILY, //1 ==== 妃子宴
	[InspectorName("CELEBRATION - idx[2] - 庆功宴")]
	CELEBRATION, //2 ==== 庆功宴
	[InspectorName("PARTY - idx[3] - 普通酒会")]
	PARTY, //3 ==== 普通酒会
	[InspectorName("HIGH_PARTY - idx[4] - 高级酒会")]
	HIGH_PARTY, //4 ==== 高级酒会
	[InspectorName("GIFTDE_CHILD_CELE - idx[5] - 卷王子嗣高级酒会")]
	GIFTDE_CHILD_CELE, //5 ==== 卷王子嗣高级酒会
}

public class EDinnerShowTypeComparer : IEqualityComparer<EDinnerShowType>{
	public bool Equals(EDinnerShowType x, EDinnerShowType y) { return x == y; }
	public int GetHashCode(EDinnerShowType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}

