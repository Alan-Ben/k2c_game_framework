package Common.ClothesEnum;

/*********
 * avatar抽卡条件类型
 **/
public enum EAvatarGachaGuaranteeCondType {
	NONE, //0 ==== 
	QUALITY, //1 ==== 筛选道具品质 QUALITY:min:max
	TAG, //2 ==== 筛选道具TAG TAG:EXAMPLE
	DONT_HAVE, //3 ==== 筛选还没获得的道具
	;
public static final EAvatarGachaGuaranteeCondType[]  EAvatarGachaGuaranteeCondType_Values = EAvatarGachaGuaranteeCondType.values();
public static final int EAvatarGachaGuaranteeCondType_Length = EAvatarGachaGuaranteeCondType_Values.length;
public static EAvatarGachaGuaranteeCondType EAvatarGachaGuaranteeCondType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EAvatarGachaGuaranteeCondType_Length){ return null; }
	return EAvatarGachaGuaranteeCondType_Values[_ivalue];
}
}

