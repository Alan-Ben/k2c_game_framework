package NPEnum;

/*********
 * 玩家的参数类型 特殊定义的值类型
 **/
public enum ENPPlayerVariableVarType {
	NONE, //0 ==== 
	BUY_TIMES, //1 ==== 购买次数
	HERO_ID, //2 ==== 大臣id
	CONSORT_ID, //3 ==== 情人id
	USE_COUNT, //4 ==== 使用次数
	SPEC_ATTR_TYPE, //5 ==== 相性
	COUNT, //6 ==== 通用数值
	ID, //7 ==== 通用实例ID
	QUALITY, //8 ==== 通用品质
	;
public static final ENPPlayerVariableVarType[]  ENPPlayerVariableVarType_Values = ENPPlayerVariableVarType.values();
public static final int ENPPlayerVariableVarType_Length = ENPPlayerVariableVarType_Values.length;
public static ENPPlayerVariableVarType ENPPlayerVariableVarType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPlayerVariableVarType_Length){ return null; }
	return ENPPlayerVariableVarType_Values[_ivalue];
}
}

