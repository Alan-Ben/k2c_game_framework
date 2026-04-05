package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 竞技场错误
 ****/
 
public class ArenaErr implements _IErrHolder
{
    public static final Result HERO_HAD_SELECTED = Result.constInit(230001,"英雄已经上场过");
    public static final Result OPPONENT_HAD_SELECTED = Result.constInit(230002,"已经选择对手");
    public static final Result OPPONENT_DATA_LOAD_FAIL = Result.constInit(230003,"对手数据加载失败");
    public static final Result OPPONENT_NOT_FOUND = Result.constInit(230004,"对手未找到");
    public static final Result OPPONENT_NOT_SELECTED = Result.constInit(230005,"对手未选择");
    public static final Result HAD_BUY_ROUND_BUFF = Result.constInit(230006,"已购买回合加成");
    public static final Result CANT_BUY_ROUND_BUFF = Result.constInit(230007,"不符合购买当前buff的条件");
    public static final Result OPPONENT_HERO_NOT_FOUND = Result.constInit(230008,"对手大臣数据不存在");
    public static final Result HP_EMPTY = Result.constInit(230009,"血量为空");
    public static final Result REPORT_NOT_FOUND = Result.constInit(230010,"战报未找到");
    public static final Result BUY_RANDOM_ATTACK_NUM_OVER_LIMIT = Result.constInit(230011,"购买随机攻击次数超上限");
    public static final Result SELECT_ATTACK_NUM_OVER_LIMIT = Result.constInit(230012,"指定攻击次数超上限");
    public static final Result RANDOM_ATTACK_NUM_OVER_LIMIT = Result.constInit(230013,"随机攻击次数超上限");
    public static final Result HERO_NUM_NOT_ENOUGH_TO_ENTER_ARENA = Result.constInit(230014,"英雄数量不足以进入竞技场");
}
