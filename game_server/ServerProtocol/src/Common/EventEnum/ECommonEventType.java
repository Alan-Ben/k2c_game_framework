package Common.EventEnum;

/*********
 * 通用事件类型
 **/
public enum ECommonEventType {
	NONE, //0 ==== 
	AWARD, //1 ==== 奖励事件
	CHOICE, //2 ==== 选项事件
	DIALOG, //3 ==== 剧情事件
	DISPATCH, //4 ==== 派遣事件
	PLOT_DIALOG, //5 ==== 剧情对话事件
	MINI_GAME, //6 ==== 小游戏事件
	FITTING, //7 ==== 试穿事件
	AVATAR_SCORE, //8 ==== 评分事件
	;
public static final ECommonEventType[]  ECommonEventType_Values = ECommonEventType.values();
public static final int ECommonEventType_Length = ECommonEventType_Values.length;
public static ECommonEventType ECommonEventType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ECommonEventType_Length){ return null; }
	return ECommonEventType_Values[_ivalue];
}
}

