using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ConditionEnum
{

/// <summary>
/// 大臣条件类型
/// </summary>
public enum EHeroConditionType {
	NONE, //0 ==== 
	[InspectorName("CS_LEVEL - idx[1] - 判断大臣等级是否在范围内 CS_LEVEL:min:max")]
	CS_LEVEL, //1 ==== 判断大臣等级是否在范围内 CS_LEVEL:min:max
	[InspectorName("CS_STAR - idx[2] - 判断大臣星级是否在范围内 CS_STAR:min:max")]
	CS_STAR, //2 ==== 判断大臣星级是否在范围内 CS_STAR:min:max
	[InspectorName("CS_ATTR - idx[3] - 判断大臣是否拥有该特长 CS_ATTR:ESpecAttrType")]
	CS_ATTR, //3 ==== 判断大臣是否拥有该特长 CS_ATTR:ESpecAttrType
	[InspectorName("CS_STEP - idx[4] - 判断大臣阶段是否在范围内 CS_STEP:min:max")]
	CS_STEP, //4 ==== 判断大臣阶段是否在范围内 CS_STEP:min:max
}

public class EHeroConditionTypeComparer : IEqualityComparer<EHeroConditionType>{
	public bool Equals(EHeroConditionType x, EHeroConditionType y) { return x == y; }
	public int GetHashCode(EHeroConditionType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

