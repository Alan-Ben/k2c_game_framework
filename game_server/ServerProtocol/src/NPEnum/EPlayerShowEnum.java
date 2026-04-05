package NPEnum;

/*********
 * 玩家展示类型
 **/
public enum EPlayerShowEnum {
	NONE, //0 ==== 
	MAKE_FACE, //1 ==== 捏脸数据
	;
public static final EPlayerShowEnum[]  EPlayerShowEnum_Values = EPlayerShowEnum.values();
public static final int EPlayerShowEnum_Length = EPlayerShowEnum_Values.length;
public static EPlayerShowEnum EPlayerShowEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EPlayerShowEnum_Length){ return null; }
	return EPlayerShowEnum_Values[_ivalue];
}
}

