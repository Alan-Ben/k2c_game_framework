package Common.GuildEnum;

/*********
 * 联盟加入限制类型
 **/
public enum EGuildJoinLimitType {
	NONE, //0 ==== 
	NATION_POWER, //1 ==== 国力
	LEVEL, //2 ==== 等级
	;
public static final EGuildJoinLimitType[]  EGuildJoinLimitType_Values = EGuildJoinLimitType.values();
public static final int EGuildJoinLimitType_Length = EGuildJoinLimitType_Values.length;
public static EGuildJoinLimitType EGuildJoinLimitType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildJoinLimitType_Length){ return null; }
	return EGuildJoinLimitType_Values[_ivalue];
}
}

