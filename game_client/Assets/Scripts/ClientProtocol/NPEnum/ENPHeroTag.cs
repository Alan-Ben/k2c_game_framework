using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 英雄标记处理
/// </summary>
public enum ENPHeroTag {
	[InspectorName("NONE - idx[0] - 无效属性")]
	NONE, //0 ==== 无效属性
	[InspectorName("RANGE_ATT - idx[1] - 远程")]
	RANGE_ATT, //1 ==== 远程
	[InspectorName("NEAR_ATT - idx[2] - 近战")]
	NEAR_ATT, //2 ==== 近战
	[InspectorName("MAGIC_ATT - idx[3] - 魔法")]
	MAGIC_ATT, //3 ==== 魔法
}

public class ENPHeroTagComparer : IEqualityComparer<ENPHeroTag>{
	public bool Equals(ENPHeroTag x, ENPHeroTag y) { return x == y; }
	public int GetHashCode(ENPHeroTag obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

