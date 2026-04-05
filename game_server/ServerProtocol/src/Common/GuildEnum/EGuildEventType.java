package Common.GuildEnum;

/*********
 * 联盟事件类型
 **/
public enum EGuildEventType {
	NONE, //0 ==== 
	IMPEACH_LEADER, //1 ==== 弹劾盟主
	;
public static final EGuildEventType[]  EGuildEventType_Values = EGuildEventType.values();
public static final int EGuildEventType_Length = EGuildEventType_Values.length;
public static EGuildEventType EGuildEventType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildEventType_Length){ return null; }
	return EGuildEventType_Values[_ivalue];
}
}

