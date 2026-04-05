using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ConditionEnum
{

/// <summary>
/// 建筑条件类型
/// </summary>
public enum EBuildingConditionType {
	NONE, //0 ==== 
	[InspectorName("CS_BUILDING_ID - idx[1] - 判断是否是指定建筑 CS_BUILDING_ID:BuildingId")]
	CS_BUILDING_ID, //1 ==== 判断是否是指定建筑 CS_BUILDING_ID:BuildingId
	[InspectorName("CS_SPEC_ATTR_TYPE - idx[2] - 判断建筑是否是指定偏向属性 CS_SPEC_ATTR_TYPE:ESpecAttrType")]
	CS_SPEC_ATTR_TYPE, //2 ==== 判断建筑是否是指定偏向属性 CS_SPEC_ATTR_TYPE:ESpecAttrType
}

public class EBuildingConditionTypeComparer : IEqualityComparer<EBuildingConditionType>{
	public bool Equals(EBuildingConditionType x, EBuildingConditionType y) { return x == y; }
	public int GetHashCode(EBuildingConditionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

