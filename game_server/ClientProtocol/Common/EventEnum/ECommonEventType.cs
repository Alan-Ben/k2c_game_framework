using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.EventEnum
{

/// <summary>
/// 通用事件类型
/// </summary>
public enum ECommonEventType {
	NONE, //0 ==== 
	[InspectorName("AWARD - idx[1] - 奖励事件")]
	AWARD, //1 ==== 奖励事件
	[InspectorName("CHOICE - idx[2] - 选项事件")]
	CHOICE, //2 ==== 选项事件
	[InspectorName("DIALOG - idx[3] - 剧情事件")]
	DIALOG, //3 ==== 剧情事件
	[InspectorName("DISPATCH - idx[4] - 派遣事件")]
	DISPATCH, //4 ==== 派遣事件
	[InspectorName("PLOT_DIALOG - idx[5] - 剧情对话事件")]
	PLOT_DIALOG, //5 ==== 剧情对话事件
	[InspectorName("MINI_GAME - idx[6] - 小游戏事件")]
	MINI_GAME, //6 ==== 小游戏事件
	[InspectorName("FITTING - idx[7] - 试穿事件")]
	FITTING, //7 ==== 试穿事件
	[InspectorName("AVATAR_SCORE - idx[8] - 评分事件")]
	AVATAR_SCORE, //8 ==== 评分事件
}

public class ECommonEventTypeComparer : IEqualityComparer<ECommonEventType>{
	public bool Equals(ECommonEventType x, ECommonEventType y) { return x == y; }
	public int GetHashCode(ECommonEventType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 9;
}
}

