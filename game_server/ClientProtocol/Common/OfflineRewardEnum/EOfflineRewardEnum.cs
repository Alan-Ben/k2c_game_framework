using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.OfflineRewardEnum
{

/// <summary>
/// 玩家离线数据类型，Hopen特别强调：需要推送客户端需要加C_
/// </summary>
public enum EOfflineRewardEnum {
	[InspectorName("GM - idx[0] - 玩家GM命令")]
	GM, //0 ==== 玩家GM命令
	[InspectorName("DINNER_OWNER_RESULT - idx[1] - 宴会主人结算数据")]
	DINNER_OWNER_RESULT, //1 ==== 宴会主人结算数据
	[InspectorName("DINNER_BE_JOINED - idx[2] - 玩家宴会有其他玩家加入")]
	DINNER_BE_JOINED, //2 ==== 玩家宴会有其他玩家加入
	[InspectorName("NO_USE_3 - idx[3] - 未使用3")]
	NO_USE_3, //3 ==== 未使用3
	[InspectorName("FRIEND_ACCEPT_APPLY - idx[4] - 收到好友申请")]
	FRIEND_ACCEPT_APPLY, //4 ==== 收到好友申请
	[InspectorName("FRIEND_AGREE_APPLY - idx[5] - 同意好友申请")]
	FRIEND_AGREE_APPLY, //5 ==== 同意好友申请
	[InspectorName("FRIEND_REMOVE - idx[6] - 移除好友")]
	FRIEND_REMOVE, //6 ==== 移除好友
	[InspectorName("ADULT_MARRY_ACCEPT_PERSON_APPLY - idx[7] - 子嗣联姻收到个人邀请")]
	ADULT_MARRY_ACCEPT_PERSON_APPLY, //7 ==== 子嗣联姻收到个人邀请
	[InspectorName("ADULT_MARRY_CANCEL_PERSON_APPLY - idx[8] - 子嗣联姻取消个人邀请")]
	ADULT_MARRY_CANCEL_PERSON_APPLY, //8 ==== 子嗣联姻取消个人邀请
	[InspectorName("ADULT_MARRY_REFUSE_PERSON_APPLY - idx[9] - 子嗣联姻拒绝个人邀请")]
	ADULT_MARRY_REFUSE_PERSON_APPLY, //9 ==== 子嗣联姻拒绝个人邀请
	[InspectorName("ADULT_MARRY_AGREE_PERSON_APPLY - idx[10] - 子嗣联姻同意个人邀请")]
	ADULT_MARRY_AGREE_PERSON_APPLY, //10 ==== 子嗣联姻同意个人邀请
	[InspectorName("ADULT_MARRY_AGREE_SERVER_APPLY - idx[11] - 子嗣联姻同意全服联姻邀请")]
	ADULT_MARRY_AGREE_SERVER_APPLY, //11 ==== 子嗣联姻同意全服联姻邀请
	[InspectorName("JOIN_GUILD - idx[12] - 加入联盟")]
	JOIN_GUILD, //12 ==== 加入联盟
	[InspectorName("QUIT_GUILD - idx[13] - 离开联盟")]
	QUIT_GUILD, //13 ==== 离开联盟
	[InspectorName("CHG_PLAYER_RECORD - idx[14] - 变更玩家记录")]
	CHG_PLAYER_RECORD, //14 ==== 变更玩家记录
	[InspectorName("ADULT_MARRY_REWARD - idx[15] - 子嗣联姻奖励展示（注：只有展示，实际奖励通过邮件领取）")]
	ADULT_MARRY_REWARD, //15 ==== 子嗣联姻奖励展示（注：只有展示，实际奖励通过邮件领取）
	[InspectorName("ORDER_PAY - idx[16] - 订单支付")]
	ORDER_PAY, //16 ==== 订单支付
	[InspectorName("MARS_MINE_TEAM_SETTLE - idx[17] - 火星矿产队伍结算")]
	MARS_MINE_TEAM_SETTLE, //17 ==== 火星矿产队伍结算
	[InspectorName("C_ORDER_DELIVERY - idx[18] - 订单发货")]
	C_ORDER_DELIVERY, //18 ==== 订单发货
	[InspectorName("MARS_EXPLORE_PVP_LOG - idx[19] - 火星探索-PVP日志")]
	MARS_EXPLORE_PVP_LOG, //19 ==== 火星探索-PVP日志
	[InspectorName("MARS_TEAM_OCCUPY_RESULT - idx[20] - 火星探索-占领火星矿结果")]
	MARS_TEAM_OCCUPY_RESULT, //20 ==== 火星探索-占领火星矿结果
	[InspectorName("GUILD_MARS_HELP_SUC - idx[21] - 公会火星互助-成功互助")]
	GUILD_MARS_HELP_SUC, //21 ==== 公会火星互助-成功互助
	[InspectorName("GUILD_BOX_DISPATCH - idx[22] - 联盟宝箱-玩家下发领取邮件")]
	GUILD_BOX_DISPATCH, //22 ==== 联盟宝箱-玩家下发领取邮件
	[InspectorName("GUILD_MARS_HELP_BE_AUTO_DEALED - idx[23] - 公会火星互助-被自动帮助")]
	GUILD_MARS_HELP_BE_AUTO_DEALED, //23 ==== 公会火星互助-被自动帮助
	[InspectorName("GUILD_MARS_HELP_AUTO_DEAL - idx[24] - 公会火星互助-自动帮助求助")]
	GUILD_MARS_HELP_AUTO_DEAL, //24 ==== 公会火星互助-自动帮助求助
	[InspectorName("FORBID_CHAT - idx[25] - 禁言")]
	FORBID_CHAT, //25 ==== 禁言
	[InspectorName("LIFT_FORBID_CHAT - idx[26] - 解除禁言")]
	LIFT_FORBID_CHAT, //26 ==== 解除禁言
	[InspectorName("MARS_RALLY_JOIN_TO_WAIT - idx[27] - 火星集结-加入到达后切换等待状态")]
	MARS_RALLY_JOIN_TO_WAIT, //27 ==== 火星集结-加入到达后切换等待状态
	[InspectorName("MARS_RALLY_JOIN_FAIL_BACK - idx[28] - 火星集结-加入失败后遣返状态")]
	MARS_RALLY_JOIN_FAIL_BACK, //28 ==== 火星集结-加入失败后遣返状态
}

public class EOfflineRewardEnumComparer : IEqualityComparer<EOfflineRewardEnum>{
	public bool Equals(EOfflineRewardEnum x, EOfflineRewardEnum y) { return x == y; }
	public int GetHashCode(EOfflineRewardEnum obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 29;
}
}

