using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.TileMatchEnum
{

/// <summary>
/// 三消格子类型
/// </summary>
public enum ETileMatch_BlockType {
	NONE, //0 ==== 
	[InspectorName("BOOM - idx[1] - 宝箱")]
	BOOM, //1 ==== 宝箱
	[InspectorName("ROCKET - idx[2] - 火箭")]
	ROCKET, //2 ==== 火箭
	[InspectorName("RAINBOW - idx[3] - 彩虹")]
	RAINBOW, //3 ==== 彩虹
}

public class ETileMatch_BlockTypeComparer : IEqualityComparer<ETileMatch_BlockType>{
	public bool Equals(ETileMatch_BlockType x, ETileMatch_BlockType y) { return x == y; }
	public int GetHashCode(ETileMatch_BlockType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

