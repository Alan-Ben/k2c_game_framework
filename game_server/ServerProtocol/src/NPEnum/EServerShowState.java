package NPEnum;

/*********
 * 服务器对外显示状态
 **/
public enum EServerShowState {
	NEW, //0 ==== 新服
	FULL, //1 ==== 爆满
	RECOMMEND, //2 ==== 推荐
	;
public static final EServerShowState[]  EServerShowState_Values = EServerShowState.values();
public static final int EServerShowState_Length = EServerShowState_Values.length;
public static EServerShowState EServerShowState_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EServerShowState_Length){ return null; }
	return EServerShowState_Values[_ivalue];
}
}

