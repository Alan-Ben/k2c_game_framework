package Common.ConditionEnum;

/*********
 * 大臣条件类型
 **/
public enum EHeroConditionType {
	NONE, //0 ==== 
	CS_LEVEL, //1 ==== 判断大臣等级是否在范围内 CS_LEVEL:min:max
	CS_STAR, //2 ==== 判断大臣星级是否在范围内 CS_STAR:min:max
	CS_ATTR, //3 ==== 判断大臣是否拥有该特长 CS_ATTR:ESpecAttrType
	CS_STEP, //4 ==== 判断大臣阶段是否在范围内 CS_STEP:min:max
	;
public static final EHeroConditionType[]  EHeroConditionType_Values = EHeroConditionType.values();
public static final int EHeroConditionType_Length = EHeroConditionType_Values.length;
public static EHeroConditionType EHeroConditionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EHeroConditionType_Length){ return null; }
	return EHeroConditionType_Values[_ivalue];
}
}

