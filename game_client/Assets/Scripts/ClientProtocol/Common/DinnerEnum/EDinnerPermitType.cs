using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.DinnerEnum
{

/// <summary>
/// 宴会凭证类型
/// </summary>
public enum EDinnerPermitType {
	NONE, //0 ==== 
	[InspectorName("FAMILY - idx[1] - 家庭宴")]
	FAMILY, //1 ==== 家庭宴
	[InspectorName("CHILD_CELE - idx[2] - 子嗣庆功宴")]
	CHILD_CELE, //2 ==== 子嗣庆功宴
	[InspectorName("TOWER_CELE - idx[3] - 爬塔庆功宴")]
	TOWER_CELE, //3 ==== 爬塔庆功宴
	[InspectorName("GIFTDE_CHILD_CELE - idx[4] - 卷王子嗣庆功宴")]
	GIFTDE_CHILD_CELE, //4 ==== 卷王子嗣庆功宴
}

public class EDinnerPermitTypeComparer : IEqualityComparer<EDinnerPermitType>{
	public bool Equals(EDinnerPermitType x, EDinnerPermitType y) { return x == y; }
	public int GetHashCode(EDinnerPermitType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

