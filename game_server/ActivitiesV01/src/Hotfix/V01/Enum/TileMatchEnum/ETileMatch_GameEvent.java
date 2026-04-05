package Hotfix.V01.Enum.TileMatchEnum;

/*********
 * 三消活动事件
 **/
public enum ETileMatch_GameEvent {
	NONE, //0 ==== 
	TILE_MATCH_GAME_INIT(101000), //1 ==== 三消初始化
	TILE_MATCH_SWITCH(101001), //2 ==== 三消交换操作
	TILE_MATCH_DRAW_STEP_REWARD(101002), //3 ==== 三消领取阶段奖励
	;
public static final ETileMatch_GameEvent[]  ETileMatch_GameEvent_Values = ETileMatch_GameEvent.values();
public static final int ETileMatch_GameEvent_Length = ETileMatch_GameEvent_Values.length;
public static ETileMatch_GameEvent ETileMatch_GameEvent_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETileMatch_GameEvent_Length){ return null; }
	return ETileMatch_GameEvent_Values[_ivalue];
}
	;
public static int _g_iLastValue = 0;
public static void setLastValue(int _lastValue){_g_iLastValue = _lastValue;}
public static int nextAutoValue(){_g_iLastValue++; return _g_iLastValue;}
private int _m_iValue;
private ETileMatch_GameEvent(int _value){_m_iValue = _value; setLastValue(_m_iValue);}
private ETileMatch_GameEvent(){_m_iValue = nextAutoValue();}
public int value(){return _m_iValue;}
}

