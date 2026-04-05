using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.TreasureHuntEnum
{

/// <summary>
/// 太空寻宝捕获类型
/// </summary>
public enum ETreasureHuntCaptureType {
	[InspectorName("NORMAL - idx[0] - 普通")]
	NORMAL, //0 ==== 普通
	[InspectorName("SINGLE_AKEY - idx[1] - 单次一键")]
	SINGLE_AKEY, //1 ==== 单次一键
	[InspectorName("MULTIPLE_AKEY - idx[2] - 多次一键")]
	MULTIPLE_AKEY, //2 ==== 多次一键
	[InspectorName("DATA_ANALYSE - idx[3] - 数据分析")]
	DATA_ANALYSE, //3 ==== 数据分析
}

public class ETreasureHuntCaptureTypeComparer : IEqualityComparer<ETreasureHuntCaptureType>{
	public bool Equals(ETreasureHuntCaptureType x, ETreasureHuntCaptureType y) { return x == y; }
	public int GetHashCode(ETreasureHuntCaptureType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

