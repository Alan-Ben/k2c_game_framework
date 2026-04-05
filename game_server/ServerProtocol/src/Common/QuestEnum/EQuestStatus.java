package Common.QuestEnum;

/*********
 * 任务状态
 **/
public enum EQuestStatus {
	NONE, //0 ==== 
	WAITING, //1 ==== 等待中
	PROGRESSING, //2 ==== 进行中
	;
public static final EQuestStatus[]  EQuestStatus_Values = EQuestStatus.values();
public static final int EQuestStatus_Length = EQuestStatus_Values.length;
public static EQuestStatus EQuestStatus_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EQuestStatus_Length){ return null; }
	return EQuestStatus_Values[_ivalue];
}
}

