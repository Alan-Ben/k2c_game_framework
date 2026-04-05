package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 冲榜礼包错误
 ****/
 
public class RankGiftPackErr implements _IErrHolder
{
    public static final Result NO_ACTIVE_PACK = Result.constInit(580001,"当前没有激活的礼包");
    public static final Result PACK_EXPIRED = Result.constInit(580002,"礼包已过期,请刷新");
    public static final Result BUY_LIMIT_EXCEEDED = Result.constInit(580003,"购买次数超限");
}
