package Common.MarsEnum;

/*********
 * 火星探索事件类型
 **/
public enum EMarsExploreEventType {
	NONE, //0 ==== 
	BATTLE, //1 ==== 战斗
	BOSS, //2 ==== 固定点刷新战斗事件
	;
public static final EMarsExploreEventType[]  EMarsExploreEventType_Values = EMarsExploreEventType.values();
public static final int EMarsExploreEventType_Length = EMarsExploreEventType_Values.length;
public static EMarsExploreEventType EMarsExploreEventType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsExploreEventType_Length){ return null; }
	return EMarsExploreEventType_Values[_ivalue];
}
}

