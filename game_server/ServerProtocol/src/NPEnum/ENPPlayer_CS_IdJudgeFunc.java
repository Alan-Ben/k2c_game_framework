package NPEnum;

/*********
 * 玩家特殊的条件判断-CS_ID_JUDGE
 **/
public enum ENPPlayer_CS_IdJudgeFunc {
	NONE, //0 ==== 
	STAGE_GOAL_STEP_IS_DONE, //1 ==== 阶段目标是否完成
	HAD_GAIN_TREASURE_HUNT_ORE, //2 ==== 是否获得过太空寻宝矿石
	HAD_GAIN_TREASURE_HUNT_TREASURE, //3 ==== 是否获得过太空寻宝奇物
	HAD_COLLECT_TREASURE_HUNT_COMPOSITE, //4 ==== 是否集齐太空寻宝组合
	HAD_DONE_SYSTEM_QUEST_TASK, //5 ==== 是否已完成指定系统任务
	;
public static final ENPPlayer_CS_IdJudgeFunc[]  ENPPlayer_CS_IdJudgeFunc_Values = ENPPlayer_CS_IdJudgeFunc.values();
public static final int ENPPlayer_CS_IdJudgeFunc_Length = ENPPlayer_CS_IdJudgeFunc_Values.length;
public static ENPPlayer_CS_IdJudgeFunc ENPPlayer_CS_IdJudgeFunc_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayer_CS_IdJudgeFunc_Length){ return null; }
	return ENPPlayer_CS_IdJudgeFunc_Values[_ivalue];
}
}

