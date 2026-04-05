using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.NumMergeEnum
{

/// <summary>
/// 数字合并活动事件
/// </summary>
public enum ENumMerge_GameEvent {
	NONE, //0 ==== 
	[InspectorName("NUM_MERGE_GAME_INIT - [102000] - idx[1] - 数字合并初始化")]
	NUM_MERGE_GAME_INIT=102000, //1 ==== 数字合并初始化
	[InspectorName("NUM_MERGE_MOVE - [102001] - idx[2] - 移动操作")]
	NUM_MERGE_MOVE=102001, //2 ==== 移动操作
	[InspectorName("NUM_MERGE_GAME_OVER - [102002] - idx[3] - 游戏结束")]
	NUM_MERGE_GAME_OVER=102002, //3 ==== 游戏结束
	[InspectorName("NUM_MERGE_USE_ORGANIZE_ITEM - [102003] - idx[4] - 使用整理道具")]
	NUM_MERGE_USE_ORGANIZE_ITEM=102003, //4 ==== 使用整理道具
	[InspectorName("NUM_MERGE_USE_ELIMINATE_ITEM - [102004] - idx[5] - 使用消除道具")]
	NUM_MERGE_USE_ELIMINATE_ITEM=102004, //5 ==== 使用消除道具
	[InspectorName("NUM_MERGE_DRAW_BOX - [102005] - idx[6] - 领取宝箱")]
	NUM_MERGE_DRAW_BOX=102005, //6 ==== 领取宝箱
}

public class ENumMerge_GameEventComparer : IEqualityComparer<ENumMerge_GameEvent>{
	public bool Equals(ENumMerge_GameEvent x, ENumMerge_GameEvent y) { return x == y; }
	public int GetHashCode(ENumMerge_GameEvent obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 7;
}
}

