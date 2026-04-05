package Common.CrossTeamEnum;

/*********
 * 队伍加入条件
 **/
public enum ENPCrossTeamJoinCond {
	NONE, //0 ==== 
	MIN_POWER, //1 ==== 最小战力
	MIN_MARS_POWER, //2 ==== 最小火星战力
	;
public static final ENPCrossTeamJoinCond[]  ENPCrossTeamJoinCond_Values = ENPCrossTeamJoinCond.values();
public static final int ENPCrossTeamJoinCond_Length = ENPCrossTeamJoinCond_Values.length;
public static ENPCrossTeamJoinCond ENPCrossTeamJoinCond_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCrossTeamJoinCond_Length){ return null; }
	return ENPCrossTeamJoinCond_Values[_ivalue];
}
}

