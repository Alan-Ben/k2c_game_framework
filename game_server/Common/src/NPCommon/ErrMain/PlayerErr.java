package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 玩家相关错误
 ****/
 
public class PlayerErr implements _IErrHolder
{
    public static final Result PLAYER_HAD_UNLOCK_FUNC = Result.constInit(190002,"玩家已经解锁功能");
    public static final Result NOT_REWARD_GAN_GAIN = Result.constInit(190003,"没有可以领取的奖励");
    public static final Result ALREADY_GAINED_VISIT_REWARD = Result.constInit(190004,"已经领取拜访奖励");
    public static final Result PLAYER_NAME_EQUAL = Result.constInit(190005,"玩家用户名重复");
    public static final Result PLAYER_NAME_CONTAINS_ILLEGAL_CHARACTER = Result.constInit(190006,"玩家用户名包含非法字符");
    public static final Result PLAYER_CUTE_ACTOR_NOT_ENABLE = Result.constInit(190007,"玩家Q版形象不可用");
    public static final Result PLAYER_TITLE_NOT_ENABLE = Result.constInit(190008,"玩家头衔不可用");
    public static final Result PLAYER_ICON_NOT_ENABLE = Result.constInit(190009,"玩家头像不可用");
    public static final Result PLAYER_ICON_BGK_NOT_ENABLE = Result.constInit(190010,"玩家头像框不可用");
    public static final Result PLAYER_BUBBLE_NOT_ENABLE = Result.constInit(190011,"玩家聊天气泡框不可用");
    public static final Result PLAYER_ACCOUNT_IS_FREEZE = Result.constInit(190012,"玩家账号被冻结");
    public static final Result OFFLINE_REWARD_TAKE_FAIL = Result.constInit(190013,"领取离线奖励失败");
    public static final Result SHIELD_CID_EXPEND_LIMIT = Result.constInit(190014,"屏蔽玩家数量超过上限");
    public static final Result SHIELD_PLAYER_BAN = Result.constInit(190015,"屏蔽玩家禁止");
    public static final Result TARGET_PLAYER_SHIELD_BAN = Result.constInit(190016,"对方玩家屏蔽了当前玩家");
    public static final Result WEEK_CARD_FREE_TRIAL_HAD_USE = Result.constInit(190017,"周卡免费试用已使用");
    public static final Result TODAY_HAD_LIKE = Result.constInit(190018,"今日已点赞");
    public static final Result TODAY_LIKE_REACH_LIMIT = Result.constInit(190019,"今日点赞次数达到上限");
    public static final Result CANT_LIKE_SELF = Result.constInit(190020,"不能给自己点赞");
    public static final Result RECRUIT_ALREADY_DONE = Result.constInit(190021,"已经兑换过");
    public static final Result GACHA_CUMULATIVE_REWARD_POINT_NOT_ENOUGH = Result.constInit(190022,"抽奖累计奖励兑换积分不足");
    public static final Result STATION_IS_EMPTY = Result.constInit(190023,"竞技场贸易站没有产出");
    public static final Result STATION_MAX_LEVEL = Result.constInit(190024,"竞技场贸易站已达最高等级");
    public static final Result TARGET_REWARD_HAD_DRAW = Result.constInit(190025,"目标奖励已领取");
    public static final Result TARGET_REWARD_NOT_REACH = Result.constInit(190026,"目标奖励还没达成");
    public static final Result REFRESH_TIME_NOT_ARRIVED = Result.constInit(190027,"刷新时间未到");
    public static final Result DRAW_DAILY_REWARD_ALREADY_DRAW = Result.constInit(190028,"已经领取过每日奖励");
    public static final Result DRAW_DAILY_REWARD_NOT_EXIST = Result.constInit(190029,"没有每日奖励");
    public static final Result SEVEN_DAYS_LOGIN_NOT_REACH = Result.constInit(190031,"七日登录奖励未达成");
    public static final Result SEVEN_DAYS_LOGIN_REWARD_HAD_DRAW = Result.constInit(190032,"七日登录奖励已领取");
    public static final Result SEVEN_DAYS_LOGIN_REWARD_NOT_FOUND = Result.constInit(190033,"七日登录奖励不存在");
    public static final Result EARNING_GOAL_REWARD_HAD_DRAW = Result.constInit(190034,"赚速目标奖励已领取");
    public static final Result EARNING_GOAL_REWARD_NOT_MEET_REQUIRE = Result.constInit(190035,"赚速目标奖励未达成");
    public static final Result VIP_LEVEL_REWARD_HAD_DRAW = Result.constInit(190036,"VIP等级奖励已领取");
    public static final Result VIP_LEVEL_REWARD_NOT_MEET_REQUIRE = Result.constInit(190037,"VIP等级奖励未达成");
    public static final Result FIRST_RECHARGE_GIFT_PACK_NOT_BUY = Result.constInit(190038,"首充礼包未购买");
    public static final Result FIRST_RECHARGE_REWARD_HAD_DRAW = Result.constInit(190039,"首充奖励已领取");
    public static final Result FIRST_RECHARGE_REWARD_DAY_NUM_NOT_MEET = Result.constInit(190040,"首充奖励天数未达成");
    public static final Result FIRST_RECHARGE_REWARD_DRAW_FAIL = Result.constInit(190041,"首充奖励领取失败");
    public static final Result RECHARGE_REBATE_NOT_MEET_REQUIRE = Result.constInit(190042,"充值返利档位未达成");
    public static final Result RECHARGE_REBATE_ALREADY_DRAW = Result.constInit(190043,"充值返利奖励已领取");
    public static final Result RECHARGE_REBATE_STEP_NOT_FOUND = Result.constInit(190044,"充值返利档位不存在");
    public static final Result PRIVILEGE_CARD_NOT_FOUND = Result.constInit(190045,"权益卡不存在");
    public static final Result PRIVILEGE_CARD_NOT_EFFECT = Result.constInit(190046,"权益卡未激活");
    public static final Result PRIVILEGE_CARD_DAILY_REWARD_GAINED = Result.constInit(190047,"权益卡当日奖励已领取");
    public static final Result STORE_REVIEWS_DONE = Result.constInit(190048,"商店评价已设置");
    public static final Result PLAYER_ROOM_SKIN_NOT_ENABLE = Result.constInit(190050,"玩家房间皮肤不可用");
    public static final Result PLAYER_NOT_REPORT_SELF = Result.constInit(190051,"玩家不能举报自己");
    public static final Result PLAYER_REPORT_CONTENT_ERROR = Result.constInit(190052,"玩家举报内容");
}
