package Common.GuildEnum;

/*********
 * 联盟弹劾盟主事件状态
 **/
public enum EGuildImpeachLeaderEventState {
	CAN_IMPEACH, //0 ==== 可弹劾
	IN_IMPEACH, //1 ==== 弹劾中
	IMPEACH_SUCCESS, //2 ==== 弹劾成功
	IMPEACH_FAIL, //3 ==== 弹劾失败
	;
public static final EGuildImpeachLeaderEventState[]  EGuildImpeachLeaderEventState_Values = EGuildImpeachLeaderEventState.values();
public static final int EGuildImpeachLeaderEventState_Length = EGuildImpeachLeaderEventState_Values.length;
public static EGuildImpeachLeaderEventState EGuildImpeachLeaderEventState_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildImpeachLeaderEventState_Length){ return null; }
	return EGuildImpeachLeaderEventState_Values[_ivalue];
}
}

