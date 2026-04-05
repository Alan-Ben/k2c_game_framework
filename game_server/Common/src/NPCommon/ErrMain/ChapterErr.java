package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 关卡系统错误
 ****/
 
public class ChapterErr implements _IErrHolder
{
    public static final Result CHAPTER_BOSS_NOT_DEFEAT = Result.constInit(30001,"关卡BOSS未击败");
    public static final Result CHAPTER_POINT_NOT_FIT = Result.constInit(30002,"关卡点位不符");
    public static final Result CHAPTER_BOSS_HAD_DEFEAT = Result.constInit(30003,"关卡BOSS已击败");
    public static final Result CHAPTER_ATTACK_BOSS_POWER_NOT_ENOUGH = Result.constInit(30004,"关卡攻击BOSS战力不足");
    public static final Result CHAPTER_NOT_ATTACK_BOSS = Result.constInit(30005,"关卡没有在攻击BOSS");
    public static final Result CHAPTER_NOT_UNLOCK = Result.constInit(30006,"关卡未解锁");
    public static final Result CHAPTER_EVENT_NOT_DONE = Result.constInit(30007,"关卡事件未完成");
    public static final Result CHAPTER_EVENT_NOT_FOUND = Result.constInit(30008,"没有可以处理的关卡事件");
    public static final Result CHAPTER_PLOT_REWARD_HAD_DRAW = Result.constInit(30009,"关卡剧情奖励已领取");
}
