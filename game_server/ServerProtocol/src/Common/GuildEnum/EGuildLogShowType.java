package Common.GuildEnum;

/*********
 * 联盟日志展示类型
 **/
public enum EGuildLogShowType {
	NONE, //0 ==== 
	CHAT, //1 ==== 聊天
	POPUP_WINDOW, //2 ==== 弹窗
	;
public static final EGuildLogShowType[]  EGuildLogShowType_Values = EGuildLogShowType.values();
public static final int EGuildLogShowType_Length = EGuildLogShowType_Values.length;
public static EGuildLogShowType EGuildLogShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildLogShowType_Length){ return null; }
	return EGuildLogShowType_Values[_ivalue];
}
}

