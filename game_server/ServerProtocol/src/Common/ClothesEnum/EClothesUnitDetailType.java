package Common.ClothesEnum;

/*********
 * 时装单品详细类型
 **/
public enum EClothesUnitDetailType {
	NONE, //0 ==== 
	HAIRSTYLE, //1 ==== 发型
	DRESS, //2 ==== 连衣裙
	COAT, //3 ==== 外套
	TOP, //4 ==== 上衣
	BOTTOMS, //5 ==== 下装
	SOCKS, //6 ==== 袜子
	SHOES, //7 ==== 鞋子
	HAT, //8 ==== 帽子
	FACE_ACCESSORY, //9 ==== 面饰
	EAR_ACCESSORY, //10 ==== 耳饰
	NECK_ACCESSORY, //11 ==== 颈饰
	GLOVES, //12 ==== 手套
	HANDHELD, //13 ==== 手持物
	HAIR_ACCESSORY, //14 ==== 发饰
	WRIST_ACCESSORY, //15 ==== 腕饰
	RING, //16 ==== 戒指
	WING, //17 ==== 翅膀
	EYEBROW_MAKEUP, //18 ==== 眉妆
	COSMETIC_CONTACT_LENSES, //19 ==== 美瞳
	EYE_SHADOW, //20 ==== 眼影
	EYE_LIINER, //21 ==== 眼线
	EYELASH, //22 ==== 睫毛
	ROUGE, //23 ==== 腮红
	LIP_MAKEUP, //24 ==== 唇妆
	FACE_TATTOOS, //25 ==== 纹面
	TAIL, //26 ==== 尾巴
	BACK_ACCESSORY, //27 ==== 背饰
	CROSSBODY, //28 ==== 斜挎
	FLOATING, //29 ==== 悬浮
	TATTOO, //30 ==== 纹身
	;
public static final EClothesUnitDetailType[]  EClothesUnitDetailType_Values = EClothesUnitDetailType.values();
public static final int EClothesUnitDetailType_Length = EClothesUnitDetailType_Values.length;
public static EClothesUnitDetailType EClothesUnitDetailType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EClothesUnitDetailType_Length){ return null; }
	return EClothesUnitDetailType_Values[_ivalue];
}
}

