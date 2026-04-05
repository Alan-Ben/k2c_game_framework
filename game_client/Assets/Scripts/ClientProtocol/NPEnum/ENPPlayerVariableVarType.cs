using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家的参数类型 特殊定义的值类型
/// </summary>
public enum ENPPlayerVariableVarType {
	NONE, //0 ==== 
	[InspectorName("BUY_TIMES - idx[1] - 购买次数")]
	BUY_TIMES, //1 ==== 购买次数
	[InspectorName("HERO_ID - idx[2] - 大臣id")]
	HERO_ID, //2 ==== 大臣id
	[InspectorName("CONSORT_ID - idx[3] - 情人id")]
	CONSORT_ID, //3 ==== 情人id
	[InspectorName("USE_COUNT - idx[4] - 使用次数")]
	USE_COUNT, //4 ==== 使用次数
	[InspectorName("SPEC_ATTR_TYPE - idx[5] - 相性")]
	SPEC_ATTR_TYPE, //5 ==== 相性
	[InspectorName("COUNT - idx[6] - 通用数值")]
	COUNT, //6 ==== 通用数值
	[InspectorName("ID - idx[7] - 通用实例ID")]
	ID, //7 ==== 通用实例ID
	[InspectorName("QUALITY - idx[8] - 通用品质")]
	QUALITY, //8 ==== 通用品质
}

public class ENPPlayerVariableVarTypeComparer : IEqualityComparer<ENPPlayerVariableVarType>{
	public bool Equals(ENPPlayerVariableVarType x, ENPPlayerVariableVarType y) { return x == y; }
	public int GetHashCode(ENPPlayerVariableVarType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 9;
}
}

