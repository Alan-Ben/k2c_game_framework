using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家繁荣度类别枚举
/// </summary>
public enum ENPFlourishCategory {
	NONE, //0 ==== 
	[InspectorName("TOTAL - idx[1] - 总繁荣度")]
	TOTAL, //1 ==== 总繁荣度
	[InspectorName("BUILDING - idx[2] - 建筑类别")]
	BUILDING, //2 ==== 建筑类别
	[InspectorName("PET - idx[3] - 宠物类别")]
	PET, //3 ==== 宠物类别
	[InspectorName("MUSEUM - idx[4] - 收藏品类别")]
	MUSEUM, //4 ==== 收藏品类别
}

public class ENPFlourishCategoryComparer : IEqualityComparer<ENPFlourishCategory>{
	public bool Equals(ENPFlourishCategory x, ENPFlourishCategory y) { return x == y; }
	public int GetHashCode(ENPFlourishCategory obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

