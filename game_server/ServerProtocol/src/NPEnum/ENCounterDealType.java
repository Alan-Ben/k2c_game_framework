package NPEnum;

/*********
 * 计数器类型操作类型
 **/
public enum ENCounterDealType {
	ADD, //0 ==== 增加
	REDUCE, //1 ==== 减少
	SET, //2 ==== 设置
	SET_GT, //3 ==== 只比当前值才设置
	;
public static final ENCounterDealType[]  ENCounterDealType_Values = ENCounterDealType.values();
public static final int ENCounterDealType_Length = ENCounterDealType_Values.length;
public static ENCounterDealType ENCounterDealType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENCounterDealType_Length){ return null; }
	return ENCounterDealType_Values[_ivalue];
}
}

