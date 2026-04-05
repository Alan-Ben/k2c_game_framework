package Common.GuildEnum;

/*********
 * 联盟职位类型
 **/
public enum EGuildPositionType {
	NONE, //0 ==== 
	MEMBER, //1 ==== 成员
	ELITE, //2 ==== 精英
	DEPUTY_LEADER, //3 ==== 副盟主
	LEADER, //4 ==== 盟主
	;
public static final EGuildPositionType[]  EGuildPositionType_Values = EGuildPositionType.values();
public static final int EGuildPositionType_Length = EGuildPositionType_Values.length;
public static EGuildPositionType EGuildPositionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildPositionType_Length){ return null; }
	return EGuildPositionType_Values[_ivalue];
}
}

