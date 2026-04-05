package Common.GuildEnum;

/*********
 * 联盟加入类型
 **/
public enum EGuildJoinType {
	NONE, //0 ==== 
	FREE_JOIN, //1 ==== 自由加入
	APPROVAL_JOIN, //2 ==== 审批加入
	DECLINE_JOIN, //3 ==== 拒绝加入
	;
public static final EGuildJoinType[]  EGuildJoinType_Values = EGuildJoinType.values();
public static final int EGuildJoinType_Length = EGuildJoinType_Values.length;
public static EGuildJoinType EGuildJoinType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildJoinType_Length){ return null; }
	return EGuildJoinType_Values[_ivalue];
}
}

