package NPEnum;

/*********
 * 宠物使用类型
 **/
public enum ENPPetUseType {
	SPACE_COMMON, //0 ==== 地图通用 适用人走物品消失的
	DIGGING, //1 ==== 挖矿
	;
public static final ENPPetUseType[]  ENPPetUseType_Values = ENPPetUseType.values();
public static final int ENPPetUseType_Length = ENPPetUseType_Values.length;
public static ENPPetUseType ENPPetUseType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPPetUseType_Length){ return null; }
	return ENPPetUseType_Values[_ivalue];
}
}

