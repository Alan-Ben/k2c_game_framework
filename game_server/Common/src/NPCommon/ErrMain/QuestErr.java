package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 任务错误
 ****/
 
public class QuestErr implements _IErrHolder
{
    public static final Result DAILY_QUEST_NOT_EXIST = Result.constInit(140001,"日常任务不存在");
    public static final Result DAILY_QUEST_HAS_FINISH = Result.constInit(140002,"日常任务已经完成");
    public static final Result DAILY_QUEST_NOT_REACH_SCORE_REQUIRE = Result.constInit(140003,"日常任务领取不满足分数需要");
    public static final Result DAILY_QUEST_NOT_UNLOCK = Result.constInit(140004,"日常任务尚未解锁");
    public static final Result DAILY_QUEST_ACTIVE_POINT_NOT_ENOUGH = Result.constInit(140005,"日常任务活跃度不够");
    public static final Result DAILY_QUEST_ACTIVE_REWARD_HAS_DRAW = Result.constInit(140006,"日常任务活跃奖励已被领取");
    public static final Result DAILY_QUEST_SERIAL_NOT_EQUAL = Result.constInit(140007,"日常任务刷新序列号不匹配");
    public static final Result DAILY_QUEST_A_KEY_DRAW_FUNC_NOT_UNLOCK = Result.constInit(140008,"日常任务一键完成功能未解锁");
    public static final Result QUEST_NOT_EXIST = Result.constInit(140009,"任务不存在");
    public static final Result QUEST_DONE_EXCEED_ERROR = Result.constInit(140010,"任务完成次数超过限制错误");
    public static final Result QUEST_START_COND_ERROR = Result.constInit(140011,"任务开启条件未通过");
    public static final Result QUEST_START_COST_ITEM_ERROR = Result.constInit(140012,"任务开启消耗物品不足");
    public static final Result QUEST_START_COST_ITEM_FAIL = Result.constInit(140013,"任务开启消耗物品失败");
    public static final Result QUEST_FINISH_FAIL = Result.constInit(140014,"任务完成失败");
    public static final Result QUEST_DROP_FAIL = Result.constInit(140015,"任务放弃失败");
    public static final Result QUEST_CLIENT_CHG_COUNT_FAIL = Result.constInit(140016,"修改任务计数失败");
}
