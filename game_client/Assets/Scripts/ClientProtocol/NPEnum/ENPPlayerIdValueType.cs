using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家根据Id获取数值的类型枚举
/// </summary>
public enum ENPPlayerIdValueType {
	NONE, //0 ==== 
	[InspectorName("CONSORT_INTIMACY - idx[1] - 情人亲密度")]
	CONSORT_INTIMACY, //1 ==== 情人亲密度
	[InspectorName("INN_STATION_LEVEL - idx[2] - 旅店设施等级")]
	INN_STATION_LEVEL, //2 ==== 旅店设施等级
	[InspectorName("BUILDING_EARNINGS - idx[3] - 指定建筑赚速")]
	BUILDING_EARNINGS, //3 ==== 指定建筑赚速
}

public class ENPPlayerIdValueTypeComparer : IEqualityComparer<ENPPlayerIdValueType>{
	public bool Equals(ENPPlayerIdValueType x, ENPPlayerIdValueType y) { return x == y; }
	public int GetHashCode(ENPPlayerIdValueType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

