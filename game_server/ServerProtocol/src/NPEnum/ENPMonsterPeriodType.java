package NPEnum;

/*********
 * 怪物阶级枚举
 **/
public enum ENPMonsterPeriodType {
	NONE, //0 ==== 
	BOSS, //1 ==== boss
	CREAM, //2 ==== 精英
	TRASH, //3 ==== 杂鱼
	;
public static final ENPMonsterPeriodType[]  ENPMonsterPeriodType_Values = ENPMonsterPeriodType.values();
public static final int ENPMonsterPeriodType_Length = ENPMonsterPeriodType_Values.length;
public static ENPMonsterPeriodType ENPMonsterPeriodType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPMonsterPeriodType_Length){ return null; }
	return ENPMonsterPeriodType_Values[_ivalue];
}
}

