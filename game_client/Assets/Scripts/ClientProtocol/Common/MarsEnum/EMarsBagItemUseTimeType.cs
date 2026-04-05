using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星系统-火星系统专用消减时间类型
/// </summary>
public enum EMarsBagItemUseTimeType {
	NONE, //0 ==== 
	[InspectorName("ALL - idx[1] - 全部")]
	ALL, //1 ==== 全部
	[InspectorName("MARS_BUILDING - idx[2] - 火星建筑时间加速")]
	MARS_BUILDING, //2 ==== 火星建筑时间加速
	[InspectorName("MARS_TECH - idx[3] - 火星科技时间消减")]
	MARS_TECH, //3 ==== 火星科技时间消减
	[InspectorName("MARS_TEAM_REPAIR - idx[4] - 火星队伍修复时间加速")]
	MARS_TEAM_REPAIR, //4 ==== 火星队伍修复时间加速
}

public class EMarsBagItemUseTimeTypeComparer : IEqualityComparer<EMarsBagItemUseTimeType>{
	public bool Equals(EMarsBagItemUseTimeType x, EMarsBagItemUseTimeType y) { return x == y; }
	public int GetHashCode(EMarsBagItemUseTimeType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 5;
}
}

