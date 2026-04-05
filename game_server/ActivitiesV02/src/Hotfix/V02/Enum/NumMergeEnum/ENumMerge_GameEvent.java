package Hotfix.V02.Enum.NumMergeEnum;

/*********
 * 数字合并活动事件
 **/
public enum ENumMerge_GameEvent {
	NONE, //0 ==== 
	NUM_MERGE_GAME_INIT(102000), //1 ==== 数字合并初始化
	NUM_MERGE_MOVE(102001), //2 ==== 移动操作
	NUM_MERGE_GAME_OVER(102002), //3 ==== 游戏结束
	NUM_MERGE_USE_ORGANIZE_ITEM(102003), //4 ==== 使用整理道具
	NUM_MERGE_USE_ELIMINATE_ITEM(102004), //5 ==== 使用消除道具
	NUM_MERGE_DRAW_BOX(102005), //6 ==== 领取宝箱
	;
public static final ENumMerge_GameEvent[]  ENumMerge_GameEvent_Values = ENumMerge_GameEvent.values();
public static final int ENumMerge_GameEvent_Length = ENumMerge_GameEvent_Values.length;
public static ENumMerge_GameEvent ENumMerge_GameEvent_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENumMerge_GameEvent_Length){ return null; }
	return ENumMerge_GameEvent_Values[_ivalue];
}
	;
public static int _g_iLastValue = 0;
public static void setLastValue(int _lastValue){_g_iLastValue = _lastValue;}
public static int nextAutoValue(){_g_iLastValue++; return _g_iLastValue;}
private int _m_iValue;
private ENumMerge_GameEvent(int _value){_m_iValue = _value; setLastValue(_m_iValue);}
private ENumMerge_GameEvent(){_m_iValue = nextAutoValue();}
public int value(){return _m_iValue;}
}

