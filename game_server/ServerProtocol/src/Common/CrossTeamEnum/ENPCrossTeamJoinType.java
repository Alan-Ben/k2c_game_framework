package Common.CrossTeamEnum;

/*********
 * 队伍加入方式
 **/
public enum ENPCrossTeamJoinType {
	NONE, //0 ==== 
	FREE_JOIN, //1 ==== 自由加入
	FORBID_JOIN, //2 ==== 禁止加入
	COND_JOIN, //3 ==== 条件加入
	;
public static final ENPCrossTeamJoinType[]  ENPCrossTeamJoinType_Values = ENPCrossTeamJoinType.values();
public static final int ENPCrossTeamJoinType_Length = ENPCrossTeamJoinType_Values.length;
public static ENPCrossTeamJoinType ENPCrossTeamJoinType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCrossTeamJoinType_Length){ return null; }
	return ENPCrossTeamJoinType_Values[_ivalue];
}
}

