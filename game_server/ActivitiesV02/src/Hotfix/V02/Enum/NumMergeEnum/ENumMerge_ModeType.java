package Hotfix.V02.Enum.NumMergeEnum;

/*********
 * 数字合并模式类型
 **/
public enum ENumMerge_ModeType {
	NONE, //0 ==== 
	NORMAL, //1 ==== 普通模式
	ADVANCED, //2 ==== 快速模式
	ULTRA, //3 ==== 极速模式
	;
public static final ENumMerge_ModeType[]  ENumMerge_ModeType_Values = ENumMerge_ModeType.values();
public static final int ENumMerge_ModeType_Length = ENumMerge_ModeType_Values.length;
public static ENumMerge_ModeType ENumMerge_ModeType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENumMerge_ModeType_Length){ return null; }
	return ENumMerge_ModeType_Values[_ivalue];
}
}

