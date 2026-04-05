package Common.MarsEnum;

/*********
 * 火星建筑类型
 **/
public enum EMarsBuildingType {
	NONE, //0 ==== 
	HOME, //1 ==== 主基地
	FOOD, //2 ==== 生态园
	HOSPITAL, //3 ==== 医务室
	SOLDIER, //4 ==== 兵工厂
	REPAIR, //5 ==== 维修室
	TECHNOLOGY, //6 ==== 科研所
	ENERGY, //7 ==== 能源厂
	LIVING, //8 ==== 居住舱
	LAW, //9 ==== 律令所
	EXPLORE, //10 ==== 灯塔
	HELP, //11 ==== 互助
	;
public static final EMarsBuildingType[]  EMarsBuildingType_Values = EMarsBuildingType.values();
public static final int EMarsBuildingType_Length = EMarsBuildingType_Values.length;
public static EMarsBuildingType EMarsBuildingType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EMarsBuildingType_Length){ return null; }
	return EMarsBuildingType_Values[_ivalue];
}
}

