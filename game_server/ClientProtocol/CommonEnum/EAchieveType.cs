using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 成就类型
/// </summary>
public enum EAchieveType {
	NONE, //0 ==== 
	[InspectorName("VILLAGE - idx[1] - 城市建设")]
	VILLAGE, //1 ==== 城市建设
	[InspectorName("FAMILY - idx[2] - 缘分相遇")]
	FAMILY, //2 ==== 缘分相遇
	[InspectorName("PROGRESS - idx[3] - 前进道路")]
	PROGRESS, //3 ==== 前进道路
	[InspectorName("TREASURE - idx[4] - 宝物收藏")]
	TREASURE, //4 ==== 宝物收藏
	[InspectorName("LIFESTYLE - idx[5] - 异世生活")]
	LIFESTYLE, //5 ==== 异世生活
	[InspectorName("EARNING_GOAL - idx[6] - 赚速目标")]
	EARNING_GOAL, //6 ==== 赚速目标
	[InspectorName("TREASURE_HUNT - idx[7] - 太空寻宝")]
	TREASURE_HUNT, //7 ==== 太空寻宝
	[InspectorName("MARS - idx[8] - 火星成就")]
	MARS, //8 ==== 火星成就
}

public class EAchieveTypeComparer : IEqualityComparer<EAchieveType>{
	public bool Equals(EAchieveType x, EAchieveType y) { return x == y; }
	public int GetHashCode(EAchieveType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 9;
}
}

