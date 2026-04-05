package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 活动相关错误
 ****/
 
public class ActivityErr implements _IErrHolder
{
    public static final Result ACTIVITY_RANK_REWARD_STILL_CALCULATE = Result.constInit(350001,"活动排行榜奖励还在结算中");
    public static final Result ACTIVITY_RANK_DONT_HAVE_REWARD = Result.constInit(350002,"活动排行榜没有奖励");
    public static final Result ACTIVITY_RANK_REWARD_HAD_DRAW = Result.constInit(350003,"活动排行榜奖励已领取");
    public static final Result ACTIVITY_STEP_REWARD_HAD_DRAW = Result.constInit(350004,"活动阶段奖励已领取");
    public static final Result ACTIVITY_STEP_REWARD_NOT_COMPLETE = Result.constInit(350005,"活动阶段奖励阶段未达成");
    public static final Result ACTIVITY_NOT_FOUND = Result.constInit(350006,"活动不存在");
    public static final Result ACTIVITY_NOT_IN_FROZEN = Result.constInit(350007,"活动不在领奖期");
    public static final Result ACTIVITY_RANK_NOT_FOUND = Result.constInit(350008,"活动排行榜不存在");
    public static final Result ACTIVITY_STEP_REWARD_NOT_FOUND = Result.constInit(350009,"活动阶段奖励不存在");
    public static final Result ACTIVITY_SHOP_NOT_FOUND = Result.constInit(350010,"活动商店不存在");
    public static final Result ACTIVITY_SHOP_REFRESH_LIMIT = Result.constInit(350011,"活动商店还没到刷新时间");
    public static final Result ACTIVITY_CRYSTAL_GIFT_PACK_REFRESH_LIMIT = Result.constInit(350012,"活动钻石礼包还没到刷新时间");
    public static final Result ACTIVITY_CRYSTAL_GIFT_BUY_LIMIT = Result.constInit(350013,"活动钻石礼包达到购买上限");
    public static final Result ACTIVITY_GIFT_PACK_GROUP_NOT_FOUND = Result.constInit(350014,"活动礼包组不存在");
    public static final Result ACTIVITY_SCHEDULE_NOT_FOUND = Result.constInit(350015,"活动关联排期不存在");
    public static final Result ACTIVITY_TYPE_ERROR = Result.constInit(350016,"活动类型错误");
    public static final Result TEAM_CANNOT_KICK_SELF = Result.constInit(350017,"不能踢出自己");
    public static final Result TEAM_PLAYER_NOT_IN_GROUP = Result.constInit(350018,"玩家退出群组");
}
