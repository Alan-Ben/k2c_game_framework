using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.QuestEnum
{

/// <summary>
/// 任务状态
/// </summary>
public enum EQuestStatus {
	NONE, //0 ==== 
	[InspectorName("WAITING - idx[1] - 等待中")]
	WAITING, //1 ==== 等待中
	[InspectorName("PROGRESSING - idx[2] - 进行中")]
	PROGRESSING, //2 ==== 进行中
}

public class EQuestStatusComparer : IEqualityComparer<EQuestStatus>{
	public bool Equals(EQuestStatus x, EQuestStatus y) { return x == y; }
	public int GetHashCode(EQuestStatus obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 3;
}
}

