package Hotfix.V01.Enum.RegularActivityEnum;

/*********
 * 万能活动事件
 **/
public enum ERegularActivity_GameEvent {
	NONE, //0 ==== 
	REGULAR_SHOP_BUY_ITEM(100001), //1 ==== 万能活动商店购买物品
	REGULAR_USE_ITEM(100002), //2 ==== 万能活动使用道具
	;
public static final ERegularActivity_GameEvent[]  ERegularActivity_GameEvent_Values = ERegularActivity_GameEvent.values();
public static final int ERegularActivity_GameEvent_Length = ERegularActivity_GameEvent_Values.length;
public static ERegularActivity_GameEvent ERegularActivity_GameEvent_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERegularActivity_GameEvent_Length){ return null; }
	return ERegularActivity_GameEvent_Values[_ivalue];
}
	;
public static int _g_iLastValue = 0;
public static void setLastValue(int _lastValue){_g_iLastValue = _lastValue;}
public static int nextAutoValue(){_g_iLastValue++; return _g_iLastValue;}
private int _m_iValue;
private ERegularActivity_GameEvent(int _value){_m_iValue = _value; setLastValue(_m_iValue);}
private ERegularActivity_GameEvent(){_m_iValue = nextAutoValue();}
public int value(){return _m_iValue;}
}

