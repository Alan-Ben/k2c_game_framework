package Common.ActivityEnum;

/*********
 * 活动状态枚举
 **/
public enum EActivityState {
	PLAN, //0 ==== 待开启
	PLAYING, //1 ==== 运行中
	SETTLING, //2 ==== 结算中
	REWARDING, //3 ==== 领奖期
	CLOSED, //4 ==== 已关闭
	CAN_DISCARD, //5 ==== 可销毁
	RESTORE, //6 ==== 活动状态恢复中
	INITIALIZING, //7 ==== 活动初始化中
	LOAD_FROM_DB, //8 ==== 从数据库加载中
	;
public static final EActivityState[]  EActivityState_Values = EActivityState.values();
public static final int EActivityState_Length = EActivityState_Values.length;
public static EActivityState EActivityState_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EActivityState_Length){ return null; }
	return EActivityState_Values[_ivalue];
}
}

