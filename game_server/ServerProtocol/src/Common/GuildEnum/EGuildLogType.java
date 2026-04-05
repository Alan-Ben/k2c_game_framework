package Common.GuildEnum;

/*********
 * 联盟日志类型
 **/
public enum EGuildLogType {
	NONE, //0 ==== 
	GUILD_CREATION, //1 ==== 联盟创建
	MEMBER_JOIN, //2 ==== 新成员加入
	MEMBER_LEAVE, //3 ==== 老成员离开
	GUILD_CONSTRUCTION, //4 ==== 联盟建设
	KICK_OUT_MEMBER, //5 ==== 踢出成员
	POSITION_CHANGE, //6 ==== 职位变更
	ANNOUNCEMENT_CHANGE, //7 ==== 公告变更
	GUILD_RENAME, //8 ==== 联盟改名
	GUILD_FLAG_CHANGE, //9 ==== 联盟旗帜变更
	ENABLE_FREE_JOIN, //10 ==== 开启自由加入
	DISABLE_FREE_JOIN, //11 ==== 关闭自由加入
	GUILD_UPGRADE, //12 ==== 联盟升级
	LEADER_TRANSFER, //13 ==== 盟主转让
	;
public static final EGuildLogType[]  EGuildLogType_Values = EGuildLogType.values();
public static final int EGuildLogType_Length = EGuildLogType_Values.length;
public static EGuildLogType EGuildLogType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildLogType_Length){ return null; }
	return EGuildLogType_Values[_ivalue];
}
}

