package Common.GuildEnum;

/*********
 * 联盟权限类型
 **/
public enum EGuildPermissionType {
	NONE, //0 ==== 
	TRANSFER_LEADER, //1 ==== 盟主转让
	DISSOLVE_GUILD, //2 ==== 解散联盟
	CHANGE_GUILD_INFO, //3 ==== 更改联盟信息
	POSITION_APPOINT, //4 ==== 职位任命
	TOGGLE_FREE_JOIN, //5 ==== 开启/关闭自由加入
	PROCESS_JOIN_REQUEST, //6 ==== 处理入盟申请
	KICK_OUT, //7 ==== 踢出联盟
	BROADCAST_MESSAGE, //8 ==== 群发消息
	LEAVE_GUILD, //9 ==== 退出联盟
	GUILD_MANAGEMENT, //10 ==== 联盟管理
	DAILY_CONSTRUCTION, //11 ==== 每日建设
	VIEW_MEMBERS, //12 ==== 查看成员
	STORE_EXCHANGE, //13 ==== 商店兑换
	LEADERBOARD, //14 ==== 排行榜
	OPEN_RECRUITMENT, //15 ==== 开启招募
	SET_PVE_AUTO_STAR, //16 ==== 设置公会副本自动开启
	USE_GUILD_WEALTH, //17 ==== 使用联盟财富
	USE_ITEM_START_PVE, //18 ==== 使用物品开启公会副本
	SET_PVE_MONSTER_TAG, //19 ==== 设置公会怪物标签
	SET_GUILD_COOPERATE_RECOMMEND_REWARD_POINT, //20 ==== 设置联盟协作推荐奖励据点
	;
public static final EGuildPermissionType[]  EGuildPermissionType_Values = EGuildPermissionType.values();
public static final int EGuildPermissionType_Length = EGuildPermissionType_Values.length;
public static EGuildPermissionType EGuildPermissionType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EGuildPermissionType_Length){ return null; }
	return EGuildPermissionType_Values[_ivalue];
}
}

