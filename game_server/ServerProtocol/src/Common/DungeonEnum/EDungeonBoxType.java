package Common.DungeonEnum;

/*********
 * 副本宝箱类型
 **/
public enum EDungeonBoxType {
	MIDDAY, //0 ==== 午间副本宝箱
	EVENING, //1 ==== 晚间副本宝箱
	;
public static final EDungeonBoxType[]  EDungeonBoxType_Values = EDungeonBoxType.values();
public static final int EDungeonBoxType_Length = EDungeonBoxType_Values.length;
public static EDungeonBoxType EDungeonBoxType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EDungeonBoxType_Length){ return null; }
	return EDungeonBoxType_Values[_ivalue];
}
}

