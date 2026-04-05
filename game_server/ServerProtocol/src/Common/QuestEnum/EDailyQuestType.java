package Common.QuestEnum;

/*********
 * 日常任务类型枚举
 **/
public enum EDailyQuestType {
	NONE, //0 ==== 
	DAY, //1 ==== 每日任务
	INN, //2 ==== 旅店
	TREASURE_HUNT, //3 ==== 太空寻宝
	;
public static final EDailyQuestType[]  EDailyQuestType_Values = EDailyQuestType.values();
public static final int EDailyQuestType_Length = EDailyQuestType_Values.length;
public static EDailyQuestType EDailyQuestType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EDailyQuestType_Length){ return null; }
	return EDailyQuestType_Values[_ivalue];
}
}

