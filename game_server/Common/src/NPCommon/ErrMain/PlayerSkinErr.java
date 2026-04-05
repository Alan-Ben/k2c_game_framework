package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 玩家形象相关错误
 ****/
 
public class PlayerSkinErr implements _IErrHolder
{
    public static final Result PLAYER_TITLE_NOT_FOUND = Result.constInit(180001,"未找到玩家称号");
    public static final Result PLAYER_TITLE_EXISTED = Result.constInit(180002,"玩家称号已存在");
    public static final Result PLAYER_SKIN_NOT_FOUND = Result.constInit(180003,"未找到玩家皮肤");
    public static final Result PLAYER_SKIN_EXISTED = Result.constInit(180004,"玩家皮肤已存在");
    public static final Result PLAYER_SKIN_LVL_FULLL = Result.constInit(180005,"玩家皮肤已满级");
}
