package NPEnum;

/*********
 * 玩家繁荣度类别枚举
 **/
public enum ENPFlourishCategory {
	NONE, //0 ==== 
	TOTAL, //1 ==== 总繁荣度
	BUILDING, //2 ==== 建筑类别
	PET, //3 ==== 宠物类别
	MUSEUM, //4 ==== 收藏品类别
	;
public static final ENPFlourishCategory[]  ENPFlourishCategory_Values = ENPFlourishCategory.values();
public static final int ENPFlourishCategory_Length = ENPFlourishCategory_Values.length;
public static ENPFlourishCategory ENPFlourishCategory_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPFlourishCategory_Length){ return null; }
	return ENPFlourishCategory_Values[_ivalue];
}
}

