package Common.BagItemUseEnum;

/*********
 * 背包使用道具-大臣获得展示类型
 **/
public enum EBagItemUse_HeroDrawShowType {
	NONE, //0 ==== 
	POWER, //1 ==== 实力
	;
public static final EBagItemUse_HeroDrawShowType[]  EBagItemUse_HeroDrawShowType_Values = EBagItemUse_HeroDrawShowType.values();
public static final int EBagItemUse_HeroDrawShowType_Length = EBagItemUse_HeroDrawShowType_Values.length;
public static EBagItemUse_HeroDrawShowType EBagItemUse_HeroDrawShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBagItemUse_HeroDrawShowType_Length){ return null; }
	return EBagItemUse_HeroDrawShowType_Values[_ivalue];
}
}

