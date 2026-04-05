package CommonEnum;

/*********
 * 加成过滤类型
 **/
public enum EBonusFilterType {
	NONE, //0 ==== 
	HERO_ATTR, //1 ==== 特长伙伴
	BUILDING_ATTR, //2 ==== 特长建筑
	BUILDING_ID, //3 ==== 指定建筑Id
	STUDENT_SEX, //4 ==== 指定性别学生
	STUDENT_ATTR, //5 ==== 指定相性学生
	CONSORT_ID, //6 ==== 指定家人Id
	HERO_ID, //7 ==== 指定伙伴ID
	QUALITY, //8 ==== 指定品质 仅支持大臣
	TREASURE_HUNT_TREASURE_ID, //9 ==== 指定太空寻宝奇物ID
	;
public static final EBonusFilterType[]  EBonusFilterType_Values = EBonusFilterType.values();
public static final int EBonusFilterType_Length = EBonusFilterType_Values.length;
public static EBonusFilterType EBonusFilterType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EBonusFilterType_Length){ return null; }
	return EBonusFilterType_Values[_ivalue];
}
}

