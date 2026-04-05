package NPEnum;

/*********
 * Comm服务器自增数据类型
 **/
public enum ENPCommonGeneralEnum {
	ARENA_ID, //0 ==== 比武擂台
	;
public static final ENPCommonGeneralEnum[]  ENPCommonGeneralEnum_Values = ENPCommonGeneralEnum.values();
public static final int ENPCommonGeneralEnum_Length = ENPCommonGeneralEnum_Values.length;
public static ENPCommonGeneralEnum ENPCommonGeneralEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCommonGeneralEnum_Length){ return null; }
	return ENPCommonGeneralEnum_Values[_ivalue];
}
}

