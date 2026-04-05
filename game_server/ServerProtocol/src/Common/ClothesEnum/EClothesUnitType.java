package Common.ClothesEnum;

/*********
 * 时装类型
 **/
public enum EClothesUnitType {
	NONE, //0 ==== 
	HAIRSTYLE, //1 ==== 发型
	DRESS, //2 ==== 连衣裙
	COAT, //3 ==== 外套
	TOP, //4 ==== 上衣
	BOTTOMS, //5 ==== 下装
	SOCKS, //6 ==== 袜子
	SHOES, //7 ==== 鞋子
	ACCESSORIES, //8 ==== 饰品
	EYEBROW_MAKEUP, //9 ==== 眉妆
	COSMETIC_CONTACT_LENSES, //10 ==== 美瞳
	EYE_SHADOW, //11 ==== 眼影
	EYE_LIINER, //12 ==== 眼线
	EYELASH, //13 ==== 睫毛
	ROUGE, //14 ==== 腮红
	LIP_MAKEUP, //15 ==== 唇妆
	FACE_TATTOOS, //16 ==== 纹面
	;
public static final EClothesUnitType[]  EClothesUnitType_Values = EClothesUnitType.values();
public static final int EClothesUnitType_Length = EClothesUnitType_Values.length;
public static EClothesUnitType EClothesUnitType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EClothesUnitType_Length){ return null; }
	return EClothesUnitType_Values[_ivalue];
}
}

