package ActivitiesV02.Err;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 数字合并相关错误
 ****/
 
public class NumMergeErr implements _IErrHolder
{
    public static final Result NUM_MERGE_MODE_REF_NOT_FOUND = Result.constInit(570001,"数字合并模式配置不存在");
    public static final Result NUM_MERGE_INVALID_MOVE = Result.constInit(570002,"数字合并无效移动");
    public static final Result NUM_MERGE_GAME_OVER = Result.constInit(570003,"数字合并游戏结束（死局）");
    public static final Result NUM_MERGE_ACTIVITY_NOT_FOUND = Result.constInit(570004,"数字合并活动不存在");
    public static final Result NUM_MERGE_ACTIVITY_NOT_RUNNING = Result.constInit(570005,"数字合并活动未开启");
    public static final Result NUM_MERGE_MODE_UNLOCK_FAIL = Result.constInit(570006,"数字合并模式解锁失败");
    public static final Result NUM_MERGE_NOT_GAME_OVER = Result.constInit(570007,"数字合并未死局");
    public static final Result NUM_MERGE_INVALID_BLOCK_INDEX = Result.constInit(570008,"方块索引无效");
    public static final Result NUM_MERGE_BLOCK_EMPTY = Result.constInit(570009,"目标位置无方块");
    public static final Result NUM_MERGE_BOX_REF_NOT_FOUND = Result.constInit(570010,"宝箱配置不存在");
    public static final Result NUM_MERGE_BOX_SCORE_NOT_ENOUGH = Result.constInit(570011,"宝箱积分不足");
}
