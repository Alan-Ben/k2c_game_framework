package NPEnum;

/*********
 * 日志记录类型，增删改
 **/
public enum ENpLogType {
	NONE, //0 ==== 
	ADD, //1 ==== 增加
	DEL, //2 ==== 删除
	SET, //3 ==== 设置
	;
public static final ENpLogType[]  ENpLogType_Values = ENpLogType.values();
public static final int ENpLogType_Length = ENpLogType_Values.length;
public static ENpLogType ENpLogType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENpLogType_Length){ return null; }
	return ENpLogType_Values[_ivalue];
}
}

