using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ChapterEnum
{

/// <summary>
/// 关卡事件类型
/// </summary>
public enum EChapterEventType {
	NONE, //0 ==== 
	[InspectorName("REWARD - idx[1] - 奖励")]
	REWARD, //1 ==== 奖励
	[InspectorName("DISPATCH - idx[2] - 大臣派遣")]
	DISPATCH, //2 ==== 大臣派遣
	[InspectorName("CHOICE - idx[3] - 选择")]
	CHOICE, //3 ==== 选择
}

public class EChapterEventTypeComparer : IEqualityComparer<EChapterEventType>{
	public bool Equals(EChapterEventType x, EChapterEventType y) { return x == y; }
	public int GetHashCode(EChapterEventType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

