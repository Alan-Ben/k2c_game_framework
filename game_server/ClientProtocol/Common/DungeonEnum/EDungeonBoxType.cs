using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.DungeonEnum
{

/// <summary>
/// 副本宝箱类型
/// </summary>
public enum EDungeonBoxType {
	[InspectorName("MIDDAY - idx[0] - 午间副本宝箱")]
	MIDDAY, //0 ==== 午间副本宝箱
	[InspectorName("EVENING - idx[1] - 晚间副本宝箱")]
	EVENING, //1 ==== 晚间副本宝箱
}

public class EDungeonBoxTypeComparer : IEqualityComparer<EDungeonBoxType>{
	public bool Equals(EDungeonBoxType x, EDungeonBoxType y) { return x == y; }
	public int GetHashCode(EDungeonBoxType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

