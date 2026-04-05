using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.MarsEnum
{

/// <summary>
/// 火星建筑类型
/// </summary>
public enum EMarsBuildingType {
	NONE, //0 ==== 
	[InspectorName("HOME - idx[1] - 主基地")]
	HOME, //1 ==== 主基地
	[InspectorName("FOOD - idx[2] - 生态园")]
	FOOD, //2 ==== 生态园
	[InspectorName("HOSPITAL - idx[3] - 医务室")]
	HOSPITAL, //3 ==== 医务室
	[InspectorName("SOLDIER - idx[4] - 兵工厂")]
	SOLDIER, //4 ==== 兵工厂
	[InspectorName("REPAIR - idx[5] - 维修室")]
	REPAIR, //5 ==== 维修室
	[InspectorName("TECHNOLOGY - idx[6] - 科研所")]
	TECHNOLOGY, //6 ==== 科研所
	[InspectorName("ENERGY - idx[7] - 能源厂")]
	ENERGY, //7 ==== 能源厂
	[InspectorName("LIVING - idx[8] - 居住舱")]
	LIVING, //8 ==== 居住舱
	[InspectorName("LAW - idx[9] - 律令所")]
	LAW, //9 ==== 律令所
	[InspectorName("EXPLORE - idx[10] - 灯塔")]
	EXPLORE, //10 ==== 灯塔
	[InspectorName("HELP - idx[11] - 互助")]
	HELP, //11 ==== 互助
}

public class EMarsBuildingTypeComparer : IEqualityComparer<EMarsBuildingType>{
	public bool Equals(EMarsBuildingType x, EMarsBuildingType y) { return x == y; }
	public int GetHashCode(EMarsBuildingType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 12;
}
}

