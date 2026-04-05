package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 妃子系统错误
 ****/
 
public class ConsortErr implements _IErrHolder
{
    public static final Result CONSORT_NOT_EXISTS = Result.constInit(40001,"家人不存在");
    public static final Result CONSORT_FETTERS_UP_NOT_ENABLE = Result.constInit(40002,"家人资质升级条件未满足");
    public static final Result CONSORT_SKILL_NOT_EXISTS = Result.constInit(40003,"家人技能不存在");
    public static final Result CONSORT_SKILL_NOT_UNLOCK = Result.constInit(40004,"家人技能未解锁");
    public static final Result CONSORT_SKILL_LVL_FULL = Result.constInit(40005,"家人技能已经满级");
    public static final Result CONSORT_SKIN_NOT_EXISTS = Result.constInit(40006,"家人皮肤不存在");
    public static final Result CONSORT_CG_NOT_UNLOCK = Result.constInit(40007,"家人CG未解锁");
    public static final Result CONSORT_CG_HAD_REWARDED = Result.constInit(40008,"家人CG已领奖");
    public static final Result CONSORT_SKIN_HAD_UNLOCKED = Result.constInit(40009,"家人皮肤已解锁");
    public static final Result CONSORT_HALO_HAD_UNLOCKED = Result.constInit(40010,"家人星辉已解锁");
    public static final Result CONSORT_EXISTED = Result.constInit(40011,"家人已经存在");
    public static final Result CHAT_DIALOGUE_NOT_TRIGGERED = Result.constInit(40012,"家人对话未触发");
    public static final Result CHAT_DIALOGUE_REWARD_HAD_DRAW = Result.constInit(40013,"家人对话奖励已领取");
    public static final Result CONSORT_MOMENT_CHAT_OVER_LIMIT = Result.constInit(40014,"家人朋友圈聊天次数超过上限");
    public static final Result CONSORT_INITIATE_CHAT_OVER_LIMIT = Result.constInit(40015,"家人主动发起聊天次数超过上限");
    public static final Result CONSORT_EVALUATE_OVER_LIMIT = Result.constInit(40016,"评价回复次数已达上限");
}
