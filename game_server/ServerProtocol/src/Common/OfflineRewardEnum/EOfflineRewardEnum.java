package Common.OfflineRewardEnum;

/*********
 * 玩家离线数据类型，Hopen特别强调：需要推送客户端需要加C_
 **/
public enum EOfflineRewardEnum {
	GM, //0 ==== 玩家GM命令
	DINNER_OWNER_RESULT, //1 ==== 宴会主人结算数据
	DINNER_BE_JOINED, //2 ==== 玩家宴会有其他玩家加入
	NO_USE_3, //3 ==== 未使用3
	FRIEND_ACCEPT_APPLY, //4 ==== 收到好友申请
	FRIEND_AGREE_APPLY, //5 ==== 同意好友申请
	FRIEND_REMOVE, //6 ==== 移除好友
	ADULT_MARRY_ACCEPT_PERSON_APPLY, //7 ==== 子嗣联姻收到个人邀请
	ADULT_MARRY_CANCEL_PERSON_APPLY, //8 ==== 子嗣联姻取消个人邀请
	ADULT_MARRY_REFUSE_PERSON_APPLY, //9 ==== 子嗣联姻拒绝个人邀请
	ADULT_MARRY_AGREE_PERSON_APPLY, //10 ==== 子嗣联姻同意个人邀请
	ADULT_MARRY_AGREE_SERVER_APPLY, //11 ==== 子嗣联姻同意全服联姻邀请
	JOIN_GUILD, //12 ==== 加入联盟
	QUIT_GUILD, //13 ==== 离开联盟
	CHG_PLAYER_RECORD, //14 ==== 变更玩家记录
	ADULT_MARRY_REWARD, //15 ==== 子嗣联姻奖励展示（注：只有展示，实际奖励通过邮件领取）
	ORDER_PAY, //16 ==== 订单支付
	MARS_MINE_TEAM_SETTLE, //17 ==== 火星矿产队伍结算
	C_ORDER_DELIVERY, //18 ==== 订单发货
	MARS_EXPLORE_PVP_LOG, //19 ==== 火星探索-PVP日志
	MARS_TEAM_OCCUPY_RESULT, //20 ==== 火星探索-占领火星矿结果
	GUILD_MARS_HELP_SUC, //21 ==== 公会火星互助-成功互助
	GUILD_BOX_DISPATCH, //22 ==== 联盟宝箱-玩家下发领取邮件
	GUILD_MARS_HELP_BE_AUTO_DEALED, //23 ==== 公会火星互助-被自动帮助
	GUILD_MARS_HELP_AUTO_DEAL, //24 ==== 公会火星互助-自动帮助求助
	FORBID_CHAT, //25 ==== 禁言
	LIFT_FORBID_CHAT, //26 ==== 解除禁言
	MARS_RALLY_JOIN_TO_WAIT, //27 ==== 火星集结-加入到达后切换等待状态
	MARS_RALLY_JOIN_FAIL_BACK, //28 ==== 火星集结-加入失败后遣返状态
	;
public static final EOfflineRewardEnum[]  EOfflineRewardEnum_Values = EOfflineRewardEnum.values();
public static final int EOfflineRewardEnum_Length = EOfflineRewardEnum_Values.length;
public static EOfflineRewardEnum EOfflineRewardEnum_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= EOfflineRewardEnum_Length){ return null; }
	return EOfflineRewardEnum_Values[_ivalue];
}
}

