using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Hotfix.TileMatchEnum
{

/// <summary>
/// 三消活动事件
/// </summary>
public enum ETileMatch_GameEvent {
	NONE, //0 ==== 
	[InspectorName("TILE_MATCH_GAME_INIT - [101000] - idx[1] - 三消初始化")]
	TILE_MATCH_GAME_INIT=101000, //1 ==== 三消初始化
	[InspectorName("TILE_MATCH_SWITCH - [101001] - idx[2] - 三消交换操作")]
	TILE_MATCH_SWITCH=101001, //2 ==== 三消交换操作
	[InspectorName("TILE_MATCH_DRAW_STEP_REWARD - [101002] - idx[3] - 三消领取阶段奖励")]
	TILE_MATCH_DRAW_STEP_REWARD=101002, //3 ==== 三消领取阶段奖励
}

public class ETileMatch_GameEventComparer : IEqualityComparer<ETileMatch_GameEvent>{
	public bool Equals(ETileMatch_GameEvent x, ETileMatch_GameEvent y) { return x == y; }
	public int GetHashCode(ETileMatch_GameEvent obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 4;
}
}

