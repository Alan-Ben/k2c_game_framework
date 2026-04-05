package Common.BagItemUseEnum;

/*********
 * 背包使用道具-妃子获得展示类型
 **/
public enum EBagItemUse_ConsortDrawShowType {
	NONE, //0 ==== 
	CHARM, //1 ==== 加护力
	INTIMACY, //2 ==== 亲密度
	CHARM_POINT, //3 ==== 加护点
	;
public static final EBagItemUse_ConsortDrawShowType[]  EBagItemUse_ConsortDrawShowType_Values = EBagItemUse_ConsortDrawShowType.values();
public static final int EBagItemUse_ConsortDrawShowType_Length = EBagItemUse_ConsortDrawShowType_Values.length;
public static EBagItemUse_ConsortDrawShowType EBagItemUse_ConsortDrawShowType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBagItemUse_ConsortDrawShowType_Length){ return null; }
	return EBagItemUse_ConsortDrawShowType_Values[_ivalue];
}
}

