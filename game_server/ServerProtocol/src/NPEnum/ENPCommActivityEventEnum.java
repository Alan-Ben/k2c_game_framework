package NPEnum;

/*********
 * 通用活动监听类型
 **/
public enum ENPCommActivityEventEnum {
	START, //0 ==== 活动开启触发监听
	FROZEN, //1 ==== 活动冻结触发监听
	CLOSED, //2 ==== 活动关闭触发监听
	DISCARD, //3 ==== 活动销毁触发监听
	;
public static final ENPCommActivityEventEnum[]  ENPCommActivityEventEnum_Values = ENPCommActivityEventEnum.values();
public static final int ENPCommActivityEventEnum_Length = ENPCommActivityEventEnum_Values.length;
public static ENPCommActivityEventEnum ENPCommActivityEventEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCommActivityEventEnum_Length){ return null; }
	return ENPCommActivityEventEnum_Values[_ivalue];
}
}

