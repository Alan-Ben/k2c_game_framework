package Common.GuildEnum;

/*********
 * 联盟建设类型
 **/
public enum EGuildConstructType {
	NONE, //0 ==== 
	GOLD, //1 ==== 金币建设
	ITEM, //2 ==== 道具建设
	;
public static final EGuildConstructType[]  EGuildConstructType_Values = EGuildConstructType.values();
public static final int EGuildConstructType_Length = EGuildConstructType_Values.length;
public static EGuildConstructType EGuildConstructType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildConstructType_Length){ return null; }
	return EGuildConstructType_Values[_ivalue];
}
}

