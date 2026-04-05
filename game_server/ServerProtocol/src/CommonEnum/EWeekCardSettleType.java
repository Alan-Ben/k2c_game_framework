package CommonEnum;

/*********
 * 周卡处理类型
 **/
public enum EWeekCardSettleType {
	NONE, //0 ==== 
	LEVY, //1 ==== 征收
	ANECDOTE, //2 ==== 政务
	CONSORT_RND_CALL, //3 ==== 妃子倾诉
	CHILD_TRAIN, //4 ==== 子嗣培养
	COLLEGE_STUDY, //5 ==== 大学学习
	TRAVEL, //6 ==== 游历
	;
public static final EWeekCardSettleType[]  EWeekCardSettleType_Values = EWeekCardSettleType.values();
public static final int EWeekCardSettleType_Length = EWeekCardSettleType_Values.length;
public static EWeekCardSettleType EWeekCardSettleType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EWeekCardSettleType_Length){ return null; }
	return EWeekCardSettleType_Values[_ivalue];
}
}

