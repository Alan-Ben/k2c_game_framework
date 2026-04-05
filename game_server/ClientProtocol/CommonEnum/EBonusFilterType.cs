using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace CommonEnum
{

/// <summary>
/// 加成过滤类型
/// </summary>
public enum EBonusFilterType {
	NONE, //0 ==== 
	[InspectorName("HERO_ATTR - idx[1] - 特长伙伴")]
	HERO_ATTR, //1 ==== 特长伙伴
	[InspectorName("BUILDING_ATTR - idx[2] - 特长建筑")]
	BUILDING_ATTR, //2 ==== 特长建筑
	[InspectorName("BUILDING_ID - idx[3] - 指定建筑Id")]
	BUILDING_ID, //3 ==== 指定建筑Id
	[InspectorName("STUDENT_SEX - idx[4] - 指定性别学生")]
	STUDENT_SEX, //4 ==== 指定性别学生
	[InspectorName("STUDENT_ATTR - idx[5] - 指定相性学生")]
	STUDENT_ATTR, //5 ==== 指定相性学生
	[InspectorName("CONSORT_ID - idx[6] - 指定家人Id")]
	CONSORT_ID, //6 ==== 指定家人Id
	[InspectorName("HERO_ID - idx[7] - 指定伙伴ID")]
	HERO_ID, //7 ==== 指定伙伴ID
	[InspectorName("QUALITY - idx[8] - 指定品质 仅支持大臣")]
	QUALITY, //8 ==== 指定品质 仅支持大臣
	[InspectorName("TREASURE_HUNT_TREASURE_ID - idx[9] - 指定太空寻宝奇物ID")]
	TREASURE_HUNT_TREASURE_ID, //9 ==== 指定太空寻宝奇物ID
}

public class EBonusFilterTypeComparer : IEqualityComparer<EBonusFilterType>{
	public bool Equals(EBonusFilterType x, EBonusFilterType y) { return x == y; }
	public int GetHashCode(EBonusFilterType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 10;
}
}

