package NPEnum;

/*********
 * 小游戏结果类型
 **/
public enum ENPMiniGameResultType {
	NONE, //0 ==== 
	SUCCESS, //1 ==== 胜利
	FAIL, //2 ==== 失败
	;
public static final ENPMiniGameResultType[]  ENPMiniGameResultType_Values = ENPMiniGameResultType.values();
public static final int ENPMiniGameResultType_Length = ENPMiniGameResultType_Values.length;
public static ENPMiniGameResultType ENPMiniGameResultType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPMiniGameResultType_Length){ return null; }
	return ENPMiniGameResultType_Values[_ivalue];
}
}

