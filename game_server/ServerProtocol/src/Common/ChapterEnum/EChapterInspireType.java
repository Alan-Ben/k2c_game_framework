package Common.ChapterEnum;

/*********
 * 关卡鼓舞类型
 **/
public enum EChapterInspireType {
	NONE, //0 ==== 
	GOLD, //1 ==== 金币鼓舞
	CRYSTAL, //2 ==== 水晶鼓舞
	ITEM, //3 ==== 道具鼓舞
	;
public static final EChapterInspireType[]  EChapterInspireType_Values = EChapterInspireType.values();
public static final int EChapterInspireType_Length = EChapterInspireType_Values.length;
public static EChapterInspireType EChapterInspireType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EChapterInspireType_Length){ return null; }
	return EChapterInspireType_Values[_ivalue];
}
}

