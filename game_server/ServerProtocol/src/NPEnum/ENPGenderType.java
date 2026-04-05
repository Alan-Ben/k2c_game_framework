package NPEnum;

/*********
 * 性别类型
 **/
public enum ENPGenderType {
	NONE, //0 ==== 
	MALE, //1 ==== 男性
	FEMALE, //2 ==== 女性
	;
public static final ENPGenderType[]  ENPGenderType_Values = ENPGenderType.values();
public static final int ENPGenderType_Length = ENPGenderType_Values.length;
public static ENPGenderType ENPGenderType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPGenderType_Length){ return null; }
	return ENPGenderType_Values[_ivalue];
}
}

