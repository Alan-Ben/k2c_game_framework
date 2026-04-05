package Common.BagItemUseEnum;

/*********
 * 背包使用道具-妃子类型
 **/
public enum EBagItemUse_ConsortType {
	NONE, //0 ==== 
	INTIMACY, //1 ==== 亲密度
	CHARM, //2 ==== 魅力
	CHARM_POINT, //3 ==== 加护点
	;
public static final EBagItemUse_ConsortType[]  EBagItemUse_ConsortType_Values = EBagItemUse_ConsortType.values();
public static final int EBagItemUse_ConsortType_Length = EBagItemUse_ConsortType_Values.length;
public static EBagItemUse_ConsortType EBagItemUse_ConsortType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBagItemUse_ConsortType_Length){ return null; }
	return EBagItemUse_ConsortType_Values[_ivalue];
}
}

