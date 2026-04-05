package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 大臣推荐系统错误
 ****/
 
public class HeroRecommandErr implements _IErrHolder
{
    public static final Result HERO_RECOMMEND_NOT_FIND = Result.constInit(340001,"大臣推荐事件不存在");
    public static final Result HERO_RECOMMEND_NOT_IN_POOL = Result.constInit(340002,"推荐大臣不在池子中");
    public static final Result HERO_RECOMMEND_COND_ERROR = Result.constInit(340003,"推荐大臣获得条件不满足");
}
