package Common.MarsEnum;

/*********
 * 火星探索-PVP日志
 **/
public enum EMarsExplorePVPLogType {
	NONE, //0 ==== 
	MINE_ATTACK, //1 ==== 矿挑战日志
	MINE_DEFEND, //2 ==== 矿防守日志
	BATTLE_EVENT, //3 ==== 事件挑战日志
	BOSS_BATTLE_EVENT, //4 ==== BOSS事件挑战日志
	MINE_COLLECT_COMPLETE, //5 ==== 矿采集完成日志
	;
public static final EMarsExplorePVPLogType[]  EMarsExplorePVPLogType_Values = EMarsExplorePVPLogType.values();
public static final int EMarsExplorePVPLogType_Length = EMarsExplorePVPLogType_Values.length;
public static EMarsExplorePVPLogType EMarsExplorePVPLogType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsExplorePVPLogType_Length){ return null; }
	return EMarsExplorePVPLogType_Values[_ivalue];
}
}

