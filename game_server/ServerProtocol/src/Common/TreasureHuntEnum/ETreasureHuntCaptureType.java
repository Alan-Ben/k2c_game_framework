package Common.TreasureHuntEnum;

/*********
 * 太空寻宝捕获类型
 **/
public enum ETreasureHuntCaptureType {
	NORMAL, //0 ==== 普通
	SINGLE_AKEY, //1 ==== 单次一键
	MULTIPLE_AKEY, //2 ==== 多次一键
	DATA_ANALYSE, //3 ==== 数据分析
	;
public static final ETreasureHuntCaptureType[]  ETreasureHuntCaptureType_Values = ETreasureHuntCaptureType.values();
public static final int ETreasureHuntCaptureType_Length = ETreasureHuntCaptureType_Values.length;
public static ETreasureHuntCaptureType ETreasureHuntCaptureType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ETreasureHuntCaptureType_Length){ return null; }
	return ETreasureHuntCaptureType_Values[_ivalue];
}
}

