package NPEnum;

/*********
 * 孵化场种子状态V2
 **/
public enum ENPHatchSeedStatusTypeV2 {
	NONE, //0 ==== 
	PREPARE, //1 ==== 等待中
	INCUBATING, //2 ==== 孵化中
	MATURE, //3 ==== 已成熟
	;
public static final ENPHatchSeedStatusTypeV2[]  ENPHatchSeedStatusTypeV2_Values = ENPHatchSeedStatusTypeV2.values();
public static final int ENPHatchSeedStatusTypeV2_Length = ENPHatchSeedStatusTypeV2_Values.length;
public static ENPHatchSeedStatusTypeV2 ENPHatchSeedStatusTypeV2_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPHatchSeedStatusTypeV2_Length){ return null; }
	return ENPHatchSeedStatusTypeV2_Values[_ivalue];
}
}

