package Common.BagItemUseEnum;

/*********
 * 背包使用道具-选择目标类型
 **/
public enum EBagItemUse_TargetType {
	NONE, //0 ==== 
	SELECT, //1 ==== 选择
	RAND, //2 ==== 随机
	;
public static final EBagItemUse_TargetType[]  EBagItemUse_TargetType_Values = EBagItemUse_TargetType.values();
public static final int EBagItemUse_TargetType_Length = EBagItemUse_TargetType_Values.length;
public static EBagItemUse_TargetType EBagItemUse_TargetType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBagItemUse_TargetType_Length){ return null; }
	return EBagItemUse_TargetType_Values[_ivalue];
}
}

