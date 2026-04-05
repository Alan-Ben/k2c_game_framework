package Common.QuestEnum;

/*********
 * 任务类型
 **/
public enum EQuestType {
	NONE, //0 ==== 
	MAIN, //1 ==== 主线任务
	BRANCH, //2 ==== 支线任务
	WISH, //3 ==== 心愿任务
	;
public static final EQuestType[]  EQuestType_Values = EQuestType.values();
public static final int EQuestType_Length = EQuestType_Values.length;
public static EQuestType EQuestType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EQuestType_Length){ return null; }
	return EQuestType_Values[_ivalue];
}
}

