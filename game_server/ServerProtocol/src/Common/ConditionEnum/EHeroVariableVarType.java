package Common.ConditionEnum;

/*********
 * 大臣的参数类型 特殊定义的值类型
 **/
public enum EHeroVariableVarType {
	NONE, //0 ==== 
	;
public static final EHeroVariableVarType[]  EHeroVariableVarType_Values = EHeroVariableVarType.values();
public static final int EHeroVariableVarType_Length = EHeroVariableVarType_Values.length;
public static EHeroVariableVarType EHeroVariableVarType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EHeroVariableVarType_Length){ return null; }
	return EHeroVariableVarType_Values[_ivalue];
}
}

