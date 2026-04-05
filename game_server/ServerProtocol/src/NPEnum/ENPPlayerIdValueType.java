package NPEnum;

/*********
 * 玩家根据Id获取数值的类型枚举
 **/
public enum ENPPlayerIdValueType {
	NONE, //0 ==== 
	CONSORT_INTIMACY, //1 ==== 情人亲密度
	INN_STATION_LEVEL, //2 ==== 旅店设施等级
	BUILDING_EARNINGS, //3 ==== 指定建筑赚速
	;
public static final ENPPlayerIdValueType[]  ENPPlayerIdValueType_Values = ENPPlayerIdValueType.values();
public static final int ENPPlayerIdValueType_Length = ENPPlayerIdValueType_Values.length;
public static ENPPlayerIdValueType ENPPlayerIdValueType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerIdValueType_Length){ return null; }
	return ENPPlayerIdValueType_Values[_ivalue];
}
}

