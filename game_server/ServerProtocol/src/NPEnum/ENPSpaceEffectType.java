package NPEnum;

/*********
 * 玩家效果类型
 **/
public enum ENPSpaceEffectType {
	NONE, //0 ==== 
	S_DSI_GROUP_REFRESH, //1 ==== 执行动态对象刷新组的刷新  S_DSI_GROUP_REFRESH:dynamic_item_refresh的id
	S_DSI_GROUP_DISCARD, //2 ==== 执行动态对象刷新组的销毁  S_DSI_GROUP_DISCARD:dynamic_item_refresh的id
	;
public static final ENPSpaceEffectType[]  ENPSpaceEffectType_Values = ENPSpaceEffectType.values();
public static final int ENPSpaceEffectType_Length = ENPSpaceEffectType_Values.length;
public static ENPSpaceEffectType ENPSpaceEffectType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPSpaceEffectType_Length){ return null; }
	return ENPSpaceEffectType_Values[_ivalue];
}
}

