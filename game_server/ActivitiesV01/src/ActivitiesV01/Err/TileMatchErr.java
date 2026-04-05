package ActivitiesV01.Err;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 三消相关错误
 ****/
 
public class TileMatchErr implements _IErrHolder
{
    public static final Result TILE_MATCH_SWITCH_ILLEGAL = Result.constInit(410001,"三消交换不合法");
    public static final Result TILE_MATCH_MODE_UNLOCK_FAIL = Result.constInit(410002,"三消模式解锁失败");
    public static final Result TILE_MATCH_STEP_REWARD_EMPTY = Result.constInit(410003,"三消阶段奖励为空");
    public static final Result TILE_MATCH_NOT_GAME_OVER = Result.constInit(410004,"三消未死局");
}
