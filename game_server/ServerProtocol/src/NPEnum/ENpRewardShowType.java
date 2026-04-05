package NPEnum;

/*********
 * 游戏奖励获取展示样式
 **/
public enum ENpRewardShowType {
	DEFAULT, //0 ==== 默认样式
	TIP, //1 ==== tip提示
	NOT_DISPLAY, //2 ==== 不展示
	;
public static final ENpRewardShowType[]  ENpRewardShowType_Values = ENpRewardShowType.values();
public static final int ENpRewardShowType_Length = ENpRewardShowType_Values.length;
public static ENpRewardShowType ENpRewardShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENpRewardShowType_Length){ return null; }
	return ENpRewardShowType_Values[_ivalue];
}
}

