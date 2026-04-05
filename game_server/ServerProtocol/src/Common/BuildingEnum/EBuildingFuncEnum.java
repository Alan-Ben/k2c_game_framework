package Common.BuildingEnum;

/*********
 * 建筑功能类型
 **/
public enum EBuildingFuncEnum {
	FARM, //0 ==== 农田建筑
	BUSINESS, //1 ==== 经营建筑
	;
public static final EBuildingFuncEnum[]  EBuildingFuncEnum_Values = EBuildingFuncEnum.values();
public static final int EBuildingFuncEnum_Length = EBuildingFuncEnum_Values.length;
public static EBuildingFuncEnum EBuildingFuncEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBuildingFuncEnum_Length){ return null; }
	return EBuildingFuncEnum_Values[_ivalue];
}
}

