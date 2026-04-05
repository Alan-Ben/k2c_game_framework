using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.QuestEnum
{

/// <summary>
/// 任务类型
/// </summary>
public enum EQuestType {
	NONE, //0 ==== 
	[InspectorName("MAIN - idx[1] - 主线任务")]
	MAIN, //1 ==== 主线任务
	[InspectorName("BRANCH - idx[2] - 支线任务")]
	BRANCH, //2 ==== 支线任务
	[InspectorName("WISH - idx[3] - 心愿任务")]
	WISH, //3 ==== 心愿任务
}

public class EQuestTypeComparer : IEqualityComparer<EQuestType>{
	public bool Equals(EQuestType x, EQuestType y) { return x == y; }
	public int GetHashCode(EQuestType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

