package NPEnum;

/*********
 * 活动排期类型
 **/
public enum EActivityScheduleEnum {
	NONE, //0 ==== 
	SINGLE, //1 ==== 单服
	CROSS, //2 ==== 跨服
	;
public static final EActivityScheduleEnum[]  EActivityScheduleEnum_Values = EActivityScheduleEnum.values();
public static final int EActivityScheduleEnum_Length = EActivityScheduleEnum_Values.length;
public static EActivityScheduleEnum EActivityScheduleEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EActivityScheduleEnum_Length){ return null; }
	return EActivityScheduleEnum_Values[_ivalue];
}
}

