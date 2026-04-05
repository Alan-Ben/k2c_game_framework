using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.QuestEnum
{

/// <summary>
/// 日常任务类型枚举
/// </summary>
public enum EDailyQuestType {
	NONE, //0 ==== 
	[InspectorName("DAY - idx[1] - 每日任务")]
	DAY, //1 ==== 每日任务
	[InspectorName("INN - idx[2] - 旅店")]
	INN, //2 ==== 旅店
	[InspectorName("TREASURE_HUNT - idx[3] - 太空寻宝")]
	TREASURE_HUNT, //3 ==== 太空寻宝
}

public class EDailyQuestTypeComparer : IEqualityComparer<EDailyQuestType>{
	public bool Equals(EDailyQuestType x, EDailyQuestType y) { return x == y; }
	public int GetHashCode(EDailyQuestType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

