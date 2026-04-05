package NPEnum;

/*********
 * 背包物品类型
 **/
public enum ENPBagItemType {
	NONE, //0 ==== 
	RES, //1 ==== 资源
	HERO, //2 ==== 骑士
	FUNCTION, //3 ==== 功能
	OTHER, //4 ==== 其他
	;
public static final ENPBagItemType[]  ENPBagItemType_Values = ENPBagItemType.values();
public static final int ENPBagItemType_Length = ENPBagItemType_Values.length;
public static ENPBagItemType ENPBagItemType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPBagItemType_Length){ return null; }
	return ENPBagItemType_Values[_ivalue];
}
}

