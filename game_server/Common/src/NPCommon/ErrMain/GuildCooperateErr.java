package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 联盟协作相关错误
 ****/
 
public class GuildCooperateErr implements _IErrHolder
{
    public static final Result PROPERTY_POINT_NOT_FOUND = Result.constInit(480001,"属性据点不存在");
    public static final Result REWARD_POINT_NOT_FOUND = Result.constInit(480002,"奖励据点不存在");
    public static final Result REWARD_POINT_ALREADY_DEFEATED = Result.constInit(480003,"奖励据点已击败");
    public static final Result AREA_NOT_UNLOCKED = Result.constInit(480004,"区域未解锁");
    public static final Result HERO_ALREADY_USED = Result.constInit(480006,"大臣已使用");
    public static final Result HERO_NOT_USED = Result.constInit(480007,"大臣未使用");
    public static final Result REWARD_ALREADY_DRAWN = Result.constInit(480008,"奖励已领取");
    public static final Result REWARD_POINT_NOT_DEFEATED = Result.constInit(480009,"奖励据点未击败");
    public static final Result PROPERTY_POINT_ALREADY_DEFEATED = Result.constInit(480010,"属性据点已击败");
}
