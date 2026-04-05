package Common.GuildDungeonEnum;

/*********
 * 联盟副本日志类型
 **/
public enum EGuildDungeon_LogType {
	NONE, //0 ==== 
	START, //1 ==== 开启
	ATTACK, //2 ==== 攻击
	KILL, //3 ==== 击杀
	AUTO_START, //4 ==== 自动开启
	;
public static final EGuildDungeon_LogType[]  EGuildDungeon_LogType_Values = EGuildDungeon_LogType.values();
public static final int EGuildDungeon_LogType_Length = EGuildDungeon_LogType_Values.length;
public static EGuildDungeon_LogType EGuildDungeon_LogType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildDungeon_LogType_Length){ return null; }
	return EGuildDungeon_LogType_Values[_ivalue];
}
}

