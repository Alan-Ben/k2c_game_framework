using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 空间条件枚举
/// </summary>
public enum ENPSpaceConditionType {
	NONE, //0 ==== 
	[InspectorName("S_AREA_ITEM_EXIST - idx[1] - 空间对象是否存在 S_AREA_ITEM_EXIST:spaceItem_id")]
	S_AREA_ITEM_EXIST, //1 ==== 空间对象是否存在 S_AREA_ITEM_EXIST:spaceItem_id
}

public class ENPSpaceConditionTypeComparer : IEqualityComparer<ENPSpaceConditionType>{
	public bool Equals(ENPSpaceConditionType x, ENPSpaceConditionType y) { return x == y; }
	public int GetHashCode(ENPSpaceConditionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

