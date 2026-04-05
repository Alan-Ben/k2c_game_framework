package CommonEnum;

/*********
 * 基础属性类型
 **/
public enum EBasicAttrType {
	NONE, //0 ==== 
	POWER, //1 ==== 实力
	TALENT, //2 ==== 资质
	POWER_PER, //3 ==== 实例万分比加成
	;
public static final EBasicAttrType[]  EBasicAttrType_Values = EBasicAttrType.values();
public static final int EBasicAttrType_Length = EBasicAttrType_Values.length;
public static EBasicAttrType EBasicAttrType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBasicAttrType_Length){ return null; }
	return EBasicAttrType_Values[_ivalue];
}
}

