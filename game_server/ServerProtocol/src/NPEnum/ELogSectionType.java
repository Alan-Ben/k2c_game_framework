package NPEnum;

/*********
 * 日志截面类型
 **/
public enum ELogSectionType {
	LVL, //0 ==== 等级
	DAY, //1 ==== 每日
	;
public static final ELogSectionType[]  ELogSectionType_Values = ELogSectionType.values();
public static final int ELogSectionType_Length = ELogSectionType_Values.length;
public static ELogSectionType ELogSectionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ELogSectionType_Length){ return null; }
	return ELogSectionType_Values[_ivalue];
}
}

