package NPEnum;

/*********
 * 效果S_ADD_P_V_FORM的二级枚举
 **/
public enum EEffectPlayerValueType {
	NONE, //0 ==== 
	UNUSE_ANECDOTE, //1 ==== 已废弃-政务事件
	UNUSE_TRAVEL_MESS, //2 ==== 已废弃-游历情报值
	;
public static final EEffectPlayerValueType[]  EEffectPlayerValueType_Values = EEffectPlayerValueType.values();
public static final int EEffectPlayerValueType_Length = EEffectPlayerValueType_Values.length;
public static EEffectPlayerValueType EEffectPlayerValueType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EEffectPlayerValueType_Length){ return null; }
	return EEffectPlayerValueType_Values[_ivalue];
}
}

