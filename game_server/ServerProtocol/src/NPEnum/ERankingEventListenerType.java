package NPEnum;

/*********
 * 排行事件监听者类型
 **/
public enum ERankingEventListenerType {
	NONE, //0 ==== 无
	RANK, //1 ==== 排行榜
	STEP_REWARD, //2 ==== 阶段奖励
	STEP_REWARD_EVENT_TASK, //3 ==== 阶段事件任务奖励
	;
public static final ERankingEventListenerType[]  ERankingEventListenerType_Values = ERankingEventListenerType.values();
public static final int ERankingEventListenerType_Length = ERankingEventListenerType_Values.length;
public static ERankingEventListenerType ERankingEventListenerType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ERankingEventListenerType_Length){ return null; }
	return ERankingEventListenerType_Values[_ivalue];
}
}

