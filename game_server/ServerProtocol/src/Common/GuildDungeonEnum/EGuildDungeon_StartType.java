package Common.GuildDungeonEnum;

/*********
 * 联盟副本开启类型
 **/
public enum EGuildDungeon_StartType {
	COMMON_ITEM, //0 ==== 通用物品
	ALLIANCE_WEALTH, //1 ==== 联盟财富
	;
public static final EGuildDungeon_StartType[]  EGuildDungeon_StartType_Values = EGuildDungeon_StartType.values();
public static final int EGuildDungeon_StartType_Length = EGuildDungeon_StartType_Values.length;
public static EGuildDungeon_StartType EGuildDungeon_StartType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildDungeon_StartType_Length){ return null; }
	return EGuildDungeon_StartType_Values[_ivalue];
}
}

