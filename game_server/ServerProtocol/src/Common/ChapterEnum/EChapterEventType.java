package Common.ChapterEnum;

/*********
 * 关卡事件类型
 **/
public enum EChapterEventType {
	NONE, //0 ==== 
	REWARD, //1 ==== 奖励
	DISPATCH, //2 ==== 大臣派遣
	CHOICE, //3 ==== 选择
	;
public static final EChapterEventType[]  EChapterEventType_Values = EChapterEventType.values();
public static final int EChapterEventType_Length = EChapterEventType_Values.length;
public static EChapterEventType EChapterEventType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EChapterEventType_Length){ return null; }
	return EChapterEventType_Values[_ivalue];
}
}

