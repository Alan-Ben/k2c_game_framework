package Common.GuildEnum;

/*********
 * 联盟火星互助类型
 **/
public enum EGuildMarsHelpObjType {
	NONE, //0 ==== 
	BUILDING_QUEUE, //1 ==== 火星建筑队列
	TECH_UP, //2 ==== 火星科技升级
	TEAM_REPAIR, //3 ==== 火星队伍修复
	;
public static final EGuildMarsHelpObjType[]  EGuildMarsHelpObjType_Values = EGuildMarsHelpObjType.values();
public static final int EGuildMarsHelpObjType_Length = EGuildMarsHelpObjType_Values.length;
public static EGuildMarsHelpObjType EGuildMarsHelpObjType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildMarsHelpObjType_Length){ return null; }
	return EGuildMarsHelpObjType_Values[_ivalue];
}
}

