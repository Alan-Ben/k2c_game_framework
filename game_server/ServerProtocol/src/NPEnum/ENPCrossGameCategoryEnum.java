package NPEnum;

/*********
 * 跨服游戏类型
 **/
public enum ENPCrossGameCategoryEnum {
	ARENA, //0 ==== 比武擂台
	;
public static final ENPCrossGameCategoryEnum[]  ENPCrossGameCategoryEnum_Values = ENPCrossGameCategoryEnum.values();
public static final int ENPCrossGameCategoryEnum_Length = ENPCrossGameCategoryEnum_Values.length;
public static ENPCrossGameCategoryEnum ENPCrossGameCategoryEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPCrossGameCategoryEnum_Length){ return null; }
	return ENPCrossGameCategoryEnum_Values[_ivalue];
}
}

