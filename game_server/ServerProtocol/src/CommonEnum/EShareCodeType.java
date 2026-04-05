package CommonEnum;

/*********
 * 分享码类型
 **/
public enum EShareCodeType {
	MAKE_FACE, //0 ==== 捏脸数据 PlayerShow_MakeFace
	CLOTHES_SHARE, //1 ==== 服装分享 Clothes_ClothesShareRawInfo to Clothes_ClothesShareInfo
	;
public static final EShareCodeType[]  EShareCodeType_Values = EShareCodeType.values();
public static final int EShareCodeType_Length = EShareCodeType_Values.length;
public static EShareCodeType EShareCodeType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EShareCodeType_Length){ return null; }
	return EShareCodeType_Values[_ivalue];
}
}

