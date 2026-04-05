package NPEnum;

/*********
 * 钉钉预警类型
 **/
public enum ENPDDAlertType {
	COMMON, //0 ==== 日常
	MEMORY, //1 ==== 内存
	;
public static final ENPDDAlertType[]  ENPDDAlertType_Values = ENPDDAlertType.values();
public static final int ENPDDAlertType_Length = ENPDDAlertType_Values.length;
public static ENPDDAlertType ENPDDAlertType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPDDAlertType_Length){ return null; }
	return ENPDDAlertType_Values[_ivalue];
}
}

