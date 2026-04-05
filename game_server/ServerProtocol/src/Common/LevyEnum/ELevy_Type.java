package Common.LevyEnum;

/*********
 * 征收类型类型
 **/
public enum ELevy_Type {
	NONE, //0 ==== 
	SILVER, //1 ==== 银币 -> 金币
	FOOD, //2 ==== 粮食 -> 面粉
	SOLDIER, //3 ==== 士兵 -> 面包
	;
public static final ELevy_Type[]  ELevy_Type_Values = ELevy_Type.values();
public static final int ELevy_Type_Length = ELevy_Type_Values.length;
public static ELevy_Type ELevy_Type_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ELevy_Type_Length){ return null; }
	return ELevy_Type_Values[_ivalue];
}
}

