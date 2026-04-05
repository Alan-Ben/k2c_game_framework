package Common.BagItemUseEnum;

/*********
 * 背包使用道具-大臣类型
 **/
public enum EBagItemUse_HeroType {
	NONE, //0 ==== 
	POWER, //1 ==== 实力
	;
public static final EBagItemUse_HeroType[]  EBagItemUse_HeroType_Values = EBagItemUse_HeroType.values();
public static final int EBagItemUse_HeroType_Length = EBagItemUse_HeroType_Values.length;
public static EBagItemUse_HeroType EBagItemUse_HeroType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBagItemUse_HeroType_Length){ return null; }
	return EBagItemUse_HeroType_Values[_ivalue];
}
}

