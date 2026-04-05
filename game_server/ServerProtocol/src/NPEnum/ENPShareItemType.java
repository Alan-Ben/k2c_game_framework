package NPEnum;

/*********
 * 分享物品类型
 **/
public enum ENPShareItemType {
	NONE, //0 ==== 
	BOX, //1 ==== 宝箱
	;
public static final ENPShareItemType[]  ENPShareItemType_Values = ENPShareItemType.values();
public static final int ENPShareItemType_Length = ENPShareItemType_Values.length;
public static ENPShareItemType ENPShareItemType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPShareItemType_Length){ return null; }
	return ENPShareItemType_Values[_ivalue];
}
}

