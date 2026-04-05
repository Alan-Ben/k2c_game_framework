package Common.CrossTeamEnum;

/*********
 * 队伍成员职位
 **/
public enum ENPCrossTeamMemberPos {
	NONE, //0 ==== 无职位
	LEADER, //1 ==== 队长
	;
public static final ENPCrossTeamMemberPos[]  ENPCrossTeamMemberPos_Values = ENPCrossTeamMemberPos.values();
public static final int ENPCrossTeamMemberPos_Length = ENPCrossTeamMemberPos_Values.length;
public static ENPCrossTeamMemberPos ENPCrossTeamMemberPos_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCrossTeamMemberPos_Length){ return null; }
	return ENPCrossTeamMemberPos_Values[_ivalue];
}
}

