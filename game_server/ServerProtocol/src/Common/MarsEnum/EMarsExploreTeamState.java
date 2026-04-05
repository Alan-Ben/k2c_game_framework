package Common.MarsEnum;

/*********
 * 火星探索队伍状态
 **/
public enum EMarsExploreTeamState {
	NONE, //0 ==== 未解锁
	ERROR, //1 ==== 错误状态，需要手动处理
	IDLE, //2 ==== 空闲中
	MARCH, //3 ==== 行军中
	BACK, //4 ==== 返程中
	REPAIR, //5 ==== 修理中
	BATTLE, //6 ==== Battle事件战斗中
	COLLECT, //7 ==== 采集中
	BOSS_BATTLE, //8 ==== Boss事件战斗中
	WAIT_RALLY, //9 ==== 等待集结出发
	;
public static final EMarsExploreTeamState[]  EMarsExploreTeamState_Values = EMarsExploreTeamState.values();
public static final int EMarsExploreTeamState_Length = EMarsExploreTeamState_Values.length;
public static EMarsExploreTeamState EMarsExploreTeamState_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsExploreTeamState_Length){ return null; }
	return EMarsExploreTeamState_Values[_ivalue];
}
}

