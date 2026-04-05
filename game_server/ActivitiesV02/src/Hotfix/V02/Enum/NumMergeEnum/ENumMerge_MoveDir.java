package Hotfix.V02.Enum.NumMergeEnum;

/*********
 * 数字合并移动方向
 **/
public enum ENumMerge_MoveDir {
	NONE, //0 ==== 
	UP, //1 ==== 向上
	DOWN, //2 ==== 向下
	LEFT, //3 ==== 向左
	RIGHT, //4 ==== 向右
	;
public static final ENumMerge_MoveDir[]  ENumMerge_MoveDir_Values = ENumMerge_MoveDir.values();
public static final int ENumMerge_MoveDir_Length = ENumMerge_MoveDir_Values.length;
public static ENumMerge_MoveDir ENumMerge_MoveDir_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENumMerge_MoveDir_Length){ return null; }
	return ENumMerge_MoveDir_Values[_ivalue];
}
}

