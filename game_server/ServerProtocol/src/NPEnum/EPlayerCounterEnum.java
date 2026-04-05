package NPEnum;

/*********
 * 玩家计数类型
 **/
public enum EPlayerCounterEnum {
	FRIEND, //0 ==== 好友数量
	FRIEND_APPLY, //1 ==== 好友申请数量
	FRIEND_LIMIT, //2 ==== 好友数量上限
	;
public static final EPlayerCounterEnum[]  EPlayerCounterEnum_Values = EPlayerCounterEnum.values();
public static final int EPlayerCounterEnum_Length = EPlayerCounterEnum_Values.length;
public static EPlayerCounterEnum EPlayerCounterEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EPlayerCounterEnum_Length){ return null; }
	return EPlayerCounterEnum_Values[_ivalue];
}
}

