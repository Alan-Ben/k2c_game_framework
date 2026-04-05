package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 跨服游戏服务器相关报错
 ****/
 
public class CGSErr implements _IErrHolder
{
    public static final Result NO_CROSSGAME_CATEGORY_ERROR = Result.constInit(310001,"跨服游戏模块不存在");
    public static final Result NO_CROSSGAME_INSTANCE_ERROR = Result.constInit(310002,"跨服游戏实例不存在");
}
