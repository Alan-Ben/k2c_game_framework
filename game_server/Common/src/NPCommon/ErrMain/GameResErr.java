package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 服务器资源相关报错
 ****/
 
public class GameResErr implements _IErrHolder
{
    public static final Result GAME_RES_GM_CHG_FAIL = Result.constInit(260001,"游戏资源GM修改失败");
    public static final Result GAME_RES_REF_TABLE_NOT_FOUND = Result.constInit(260002,"游戏资源配表找不到");
}
