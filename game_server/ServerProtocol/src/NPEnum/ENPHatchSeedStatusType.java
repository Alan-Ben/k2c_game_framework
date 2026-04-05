package NPEnum;

/*********
 * 孵化场种子状态
 **/
public enum ENPHatchSeedStatusType {
	NONE, //0 ==== 
	IDLE, //1 ==== 空闲
	PREPARE, //2 ==== 准备队列中
	INCUBATING, //3 ==== 孵化中
	MATURE, //4 ==== 成熟
	;
public static final ENPHatchSeedStatusType[]  ENPHatchSeedStatusType_Values = ENPHatchSeedStatusType.values();
public static final int ENPHatchSeedStatusType_Length = ENPHatchSeedStatusType_Values.length;
public static ENPHatchSeedStatusType ENPHatchSeedStatusType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPHatchSeedStatusType_Length){ return null; }
	return ENPHatchSeedStatusType_Values[_ivalue];
}
}

