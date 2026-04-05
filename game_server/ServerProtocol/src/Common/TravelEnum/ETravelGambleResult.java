package Common.TravelEnum;

/*********
 * 博彩结果类型
 **/
public enum ETravelGambleResult {
	NONE, //0 ==== 
	WIN, //1 ==== 胜利
	LOSE, //2 ==== 失败
	JACKPOT, //3 ==== 特别大奖
	ABANDON, //4 ==== 放弃
	;
public static final ETravelGambleResult[]  ETravelGambleResult_Values = ETravelGambleResult.values();
public static final int ETravelGambleResult_Length = ETravelGambleResult_Values.length;
public static ETravelGambleResult ETravelGambleResult_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETravelGambleResult_Length){ return null; }
	return ETravelGambleResult_Values[_ivalue];
}
}

