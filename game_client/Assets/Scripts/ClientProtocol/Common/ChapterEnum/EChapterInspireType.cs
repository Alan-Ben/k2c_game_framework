using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ChapterEnum
{

/// <summary>
/// 关卡鼓舞类型
/// </summary>
public enum EChapterInspireType {
	NONE, //0 ==== 
	[InspectorName("GOLD - idx[1] - 金币鼓舞")]
	GOLD, //1 ==== 金币鼓舞
	[InspectorName("CRYSTAL - idx[2] - 水晶鼓舞")]
	CRYSTAL, //2 ==== 水晶鼓舞
	[InspectorName("ITEM - idx[3] - 道具鼓舞")]
	ITEM, //3 ==== 道具鼓舞
}

public class EChapterInspireTypeComparer : IEqualityComparer<EChapterInspireType>{
	public bool Equals(EChapterInspireType x, EChapterInspireType y) { return x == y; }
	public int GetHashCode(EChapterInspireType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

