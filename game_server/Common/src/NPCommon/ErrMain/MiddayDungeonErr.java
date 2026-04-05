package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 午间副本相关错误
 ****/
 
public class MiddayDungeonErr implements _IErrHolder
{
    public static final Result HERO_USE_REACH_LIMIT = Result.constInit(380001,"大臣使用次数已达上限");
    public static final Result ALREADY_BORROW_GUILD_HERO = Result.constInit(380002,"已经借用过联盟大臣");
    public static final Result BORROW_HERO_REACH_LIMIT = Result.constInit(380003,"借用大臣次数已达上限");
    public static final Result ALREADY_DRAW_BOX = Result.constInit(380004,"已经领取过宝箱");
    public static final Result DRAW_BOX_REACH_LIMIT = Result.constInit(380005,"领取宝箱次数已达上限");
    public static final Result BOX_EXPIRED = Result.constInit(380006,"宝箱已过期");
    public static final Result BOX_EMPTY = Result.constInit(380007,"宝箱已领取完");
    public static final Result NOT_IN_FIGHT_TIME = Result.constInit(380008,"未在战斗时间内");
}
