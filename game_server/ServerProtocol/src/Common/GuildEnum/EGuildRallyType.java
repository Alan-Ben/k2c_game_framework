package Common.GuildEnum;

/*********
 * 联盟集结类型
 **/
public enum EGuildRallyType {
	NONE, //0 ==== 
	DEFAULT, //1 ==== 默认集结
	;
public static final EGuildRallyType[]  EGuildRallyType_Values = EGuildRallyType.values();
public static final int EGuildRallyType_Length = EGuildRallyType_Values.length;
public static EGuildRallyType EGuildRallyType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildRallyType_Length){ return null; }
	return EGuildRallyType_Values[_ivalue];
}
}

