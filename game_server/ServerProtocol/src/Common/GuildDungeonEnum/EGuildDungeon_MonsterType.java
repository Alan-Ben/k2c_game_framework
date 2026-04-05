package Common.GuildDungeonEnum;

/*********
 * 联盟副本怪物类型
 **/
public enum EGuildDungeon_MonsterType {
	NONE, //0 ==== 
	COMMON, //1 ==== 普通怪物
	SENIOR, //2 ==== 高级怪物
	BOSS, //3 ==== boss
	;
public static final EGuildDungeon_MonsterType[]  EGuildDungeon_MonsterType_Values = EGuildDungeon_MonsterType.values();
public static final int EGuildDungeon_MonsterType_Length = EGuildDungeon_MonsterType_Values.length;
public static EGuildDungeon_MonsterType EGuildDungeon_MonsterType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildDungeon_MonsterType_Length){ return null; }
	return EGuildDungeon_MonsterType_Values[_ivalue];
}
}

