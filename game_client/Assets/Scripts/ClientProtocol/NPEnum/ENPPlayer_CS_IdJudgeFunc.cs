using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace NPEnum
{

/// <summary>
/// 玩家特殊的条件判断-CS_ID_JUDGE
/// </summary>
public enum ENPPlayer_CS_IdJudgeFunc {
	NONE, //0 ==== 
	[InspectorName("STAGE_GOAL_STEP_IS_DONE - idx[1] - 阶段目标是否完成")]
	STAGE_GOAL_STEP_IS_DONE, //1 ==== 阶段目标是否完成
	[InspectorName("HAD_GAIN_TREASURE_HUNT_ORE - idx[2] - 是否获得过太空寻宝矿石")]
	HAD_GAIN_TREASURE_HUNT_ORE, //2 ==== 是否获得过太空寻宝矿石
	[InspectorName("HAD_GAIN_TREASURE_HUNT_TREASURE - idx[3] - 是否获得过太空寻宝奇物")]
	HAD_GAIN_TREASURE_HUNT_TREASURE, //3 ==== 是否获得过太空寻宝奇物
	[InspectorName("HAD_COLLECT_TREASURE_HUNT_COMPOSITE - idx[4] - 是否集齐太空寻宝组合")]
	HAD_COLLECT_TREASURE_HUNT_COMPOSITE, //4 ==== 是否集齐太空寻宝组合
	[InspectorName("HAD_DONE_SYSTEM_QUEST_TASK - idx[5] - 是否已完成指定系统任务")]
	HAD_DONE_SYSTEM_QUEST_TASK, //5 ==== 是否已完成指定系统任务
}

public class ENPPlayer_CS_IdJudgeFuncComparer : IEqualityComparer<ENPPlayer_CS_IdJudgeFunc>{
	public bool Equals(ENPPlayer_CS_IdJudgeFunc x, ENPPlayer_CS_IdJudgeFunc y) { return x == y; }
	public int GetHashCode(ENPPlayer_CS_IdJudgeFunc obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 6;
}
}

