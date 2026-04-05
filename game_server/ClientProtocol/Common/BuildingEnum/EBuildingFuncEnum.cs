using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.BuildingEnum
{

/// <summary>
/// 建筑功能类型
/// </summary>
public enum EBuildingFuncEnum {
	[InspectorName("FARM - idx[0] - 农田建筑")]
	FARM, //0 ==== 农田建筑
	[InspectorName("BUSINESS - idx[1] - 经营建筑")]
	BUSINESS, //1 ==== 经营建筑
}

public class EBuildingFuncEnumComparer : IEqualityComparer<EBuildingFuncEnum>{
	public bool Equals(EBuildingFuncEnum x, EBuildingFuncEnum y) { return x == y; }
	public int GetHashCode(EBuildingFuncEnum obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 2;
}
}

