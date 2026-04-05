package CommonEnum;

/*********
 * 成就类型
 **/
public enum EAchieveType {
	NONE, //0 ==== 
	VILLAGE, //1 ==== 城市建设
	FAMILY, //2 ==== 缘分相遇
	PROGRESS, //3 ==== 前进道路
	TREASURE, //4 ==== 宝物收藏
	LIFESTYLE, //5 ==== 异世生活
	EARNING_GOAL, //6 ==== 赚速目标
	TREASURE_HUNT, //7 ==== 太空寻宝
	MARS, //8 ==== 火星成就
	;
public static final EAchieveType[]  EAchieveType_Values = EAchieveType.values();
public static final int EAchieveType_Length = EAchieveType_Values.length;
public static EAchieveType EAchieveType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EAchieveType_Length){ return null; }
	return EAchieveType_Values[_ivalue];
}
}

