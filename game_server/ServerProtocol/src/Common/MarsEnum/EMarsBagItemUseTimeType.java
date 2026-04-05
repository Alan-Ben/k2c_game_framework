package Common.MarsEnum;

/*********
 * 火星系统-火星系统专用消减时间类型
 **/
public enum EMarsBagItemUseTimeType {
	NONE, //0 ==== 
	ALL, //1 ==== 全部
	MARS_BUILDING, //2 ==== 火星建筑时间加速
	MARS_TECH, //3 ==== 火星科技时间消减
	MARS_TEAM_REPAIR, //4 ==== 火星队伍修复时间加速
	;
public static final EMarsBagItemUseTimeType[]  EMarsBagItemUseTimeType_Values = EMarsBagItemUseTimeType.values();
public static final int EMarsBagItemUseTimeType_Length = EMarsBagItemUseTimeType_Values.length;
public static EMarsBagItemUseTimeType EMarsBagItemUseTimeType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsBagItemUseTimeType_Length){ return null; }
	return EMarsBagItemUseTimeType_Values[_ivalue];
}
}

