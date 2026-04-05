package Common.ConditionEnum;

/*********
 * 建筑条件类型
 **/
public enum EBuildingConditionType {
	NONE, //0 ==== 
	CS_BUILDING_ID, //1 ==== 判断是否是指定建筑 CS_BUILDING_ID:BuildingId
	CS_SPEC_ATTR_TYPE, //2 ==== 判断建筑是否是指定偏向属性 CS_SPEC_ATTR_TYPE:ESpecAttrType
	;
public static final EBuildingConditionType[]  EBuildingConditionType_Values = EBuildingConditionType.values();
public static final int EBuildingConditionType_Length = EBuildingConditionType_Values.length;
public static EBuildingConditionType EBuildingConditionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBuildingConditionType_Length){ return null; }
	return EBuildingConditionType_Values[_ivalue];
}
}

