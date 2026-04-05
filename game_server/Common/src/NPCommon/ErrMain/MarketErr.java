package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 集市相关错误
 ****/
 
public class MarketErr implements _IErrHolder
{
    public static final Result MARKET_NOT_UNLOCK = Result.constInit(200001,"集市尚未解锁");
    public static final Result MARKET_LVL_FULL = Result.constInit(200002,"集市已经满级");
    public static final Result MARKET_COUNT_EMPTY = Result.constInit(200003,"集市经营次数不足");
    public static final Result MARKET_CD_NOT_RECOVER = Result.constInit(200004,"集市CD未回复");
    public static final Result MARKET_AKEY_OPERATE_NOT_UNLOCK = Result.constInit(200005,"集市一键经营未解锁");
}
