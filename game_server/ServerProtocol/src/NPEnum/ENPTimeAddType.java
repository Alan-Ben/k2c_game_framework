package NPEnum;

/*********
 * 时间叠加方式枚举
 **/
public enum ENPTimeAddType {
	ADD, //0 ==== 默认叠加
	SET, //1 ==== 设置
	;
public static final ENPTimeAddType[]  ENPTimeAddType_Values = ENPTimeAddType.values();
public static final int ENPTimeAddType_Length = ENPTimeAddType_Values.length;
public static ENPTimeAddType ENPTimeAddType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPTimeAddType_Length){ return null; }
	return ENPTimeAddType_Values[_ivalue];
}
}

