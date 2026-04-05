package CommonEnum;

/*********
 * AI聊天角色类型
 **/
public enum EAiChatRoleType {
	SYSTEM, //0 ==== 系统
	USER, //1 ==== 用户
	;
public static final EAiChatRoleType[]  EAiChatRoleType_Values = EAiChatRoleType.values();
public static final int EAiChatRoleType_Length = EAiChatRoleType_Values.length;
public static EAiChatRoleType EAiChatRoleType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EAiChatRoleType_Length){ return null; }
	return EAiChatRoleType_Values[_ivalue];
}
}

