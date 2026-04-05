using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 分享物品类型
/// </summary>
public enum ENPShareItemType {
	NONE, //0 ==== 
	[InspectorName("BOX - idx[1] - 宝箱")]
	BOX, //1 ==== 宝箱
}

public class ENPShareItemTypeComparer : IEqualityComparer<ENPShareItemType>{
	public bool Equals(ENPShareItemType x, ENPShareItemType y) { return x == y; }
	public int GetHashCode(ENPShareItemType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

