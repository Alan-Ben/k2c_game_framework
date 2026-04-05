package CommonEnum;

/*********
 * 特长属性类型
 **/
public enum ESpecAttrType {
	NONE, //0 ==== 
	TYPE_A, //1 ==== 属性A
	TYPE_B, //2 ==== 属性B
	TYPE_C, //3 ==== 属性C
	TYPE_D, //4 ==== 属性D
	TYPE_E, //5 ==== 属性E
	;
public static final ESpecAttrType[]  ESpecAttrType_Values = ESpecAttrType.values();
public static final int ESpecAttrType_Length = ESpecAttrType_Values.length;
public static ESpecAttrType ESpecAttrType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ESpecAttrType_Length){ return null; }
	return ESpecAttrType_Values[_ivalue];
}
}

