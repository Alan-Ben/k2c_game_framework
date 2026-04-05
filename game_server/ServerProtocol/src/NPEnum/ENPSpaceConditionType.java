package NPEnum;

/*********
 * 空间条件枚举
 **/
public enum ENPSpaceConditionType {
	NONE, //0 ==== 
	S_AREA_ITEM_EXIST, //1 ==== 空间对象是否存在 S_AREA_ITEM_EXIST:spaceItem_id
	;
public static final ENPSpaceConditionType[]  ENPSpaceConditionType_Values = ENPSpaceConditionType.values();
public static final int ENPSpaceConditionType_Length = ENPSpaceConditionType_Values.length;
public static ENPSpaceConditionType ENPSpaceConditionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPSpaceConditionType_Length){ return null; }
	return ENPSpaceConditionType_Values[_ivalue];
}
}

