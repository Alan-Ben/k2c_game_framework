package Common.ConsortEnum;

/*********
 * 家人剧情类型
 **/
public enum EConsortStoryType {
	NONE, //0 ==== 
	PLAY, //1 ==== 游玩
	CALL, //2 ==== 邀约
	MARRY, //3 ==== 成为家人
	FIRST_CALL, //4 ==== 首次邀约
	SECOND_CALL, //5 ==== 二次邀约
	;
public static final EConsortStoryType[]  EConsortStoryType_Values = EConsortStoryType.values();
public static final int EConsortStoryType_Length = EConsortStoryType_Values.length;
public static EConsortStoryType EConsortStoryType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EConsortStoryType_Length){ return null; }
	return EConsortStoryType_Values[_ivalue];
}
}

