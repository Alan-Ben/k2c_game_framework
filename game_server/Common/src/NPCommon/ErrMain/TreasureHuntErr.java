package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 太空寻宝相关错误
 ****/
 
public class TreasureHuntErr implements _IErrHolder
{
    public static final Result TREASURE_HUNT_ORE_RECORD_REWARD_HAD_DRAW = Result.constInit(460001,"太空寻宝矿石记录奖励已领取");
    public static final Result TREASURE_HUNT_ORE_RECORD_REWARD_NOT_REACHED = Result.constInit(460002,"太空寻宝矿石记录奖励未达到");
    public static final Result TREASURE_HUNT_SKILL_HAD_ACTIVE = Result.constInit(460003,"太空寻宝技能已激活");
    public static final Result TREASURE_HUNT_SKILL_ACTIVE_FAIL = Result.constInit(460004,"太空寻宝技能激活失败");
    public static final Result TREASURE_HUNT_TREASURE_NO_OUTPUT = Result.constInit(460005,"太空寻宝奇物无产出");
    public static final Result TREASURE_HUNT_CANT_DRAW_GEM = Result.constInit(460007,"太空寻宝无法领取钻石奖励");
    public static final Result TREASURE_HUNT_NO_CAPTURE_ITEM_CAN_DRAW = Result.constInit(460008,"太空寻宝没有可领取的捕获物品");
    public static final Result TREASURE_HUNT_CAPTURE_ITEM_NUM_REACH_LIMIT = Result.constInit(460009,"太空寻宝捕获物品数量已达上限");
    public static final Result TREASURE_HUNT_ORE_NOT_FOUND = Result.constInit(460010,"太空寻宝矿石不存在");
    public static final Result TREASURE_HUNT_TREASURE_NOT_FOUND = Result.constInit(460011,"太空寻宝奇物不存在");
    public static final Result TREASURE_HUNT_SKILL_NOT_EXIST = Result.constInit(460012,"太空寻宝技能不存在");
    public static final Result TREASURE_HUNT_COMPOSITE_NOT_FOUND = Result.constInit(460013,"太空寻宝矿石组合不存在");
    public static final Result TREASURE_HUNT_NO_PENDING_ORE_CAN_TRANS = Result.constInit(460014,"太空寻宝没有待转换的矿石");
    public static final Result TREASURE_HUNT_ORE_RECORD_REWARD_NOT_EXIST = Result.constInit(460015,"太空寻宝矿石记录奖励不存在");
    public static final Result TREASURE_HUNT_PENDING_ORE_LIMIT = Result.constInit(460016,"太空寻宝待处理矿石数量已达上限");
}
