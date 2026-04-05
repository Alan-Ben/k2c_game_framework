package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 晚间副本相关错误
 ****/
 
public class EveningDungeonErr implements _IErrHolder
{
    public static final Result HERO_USE_REACH_LIMIT = Result.constInit(390001,"大臣使用次数已达上限");
    public static final Result BOSS_DEAD = Result.constInit(390002,"BOSS已死亡");
    public static final Result NOT_IN_FIGHT_TIME = Result.constInit(390003,"未在战斗时间内");
}
