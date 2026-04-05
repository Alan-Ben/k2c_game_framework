package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 博物馆相关错误
 ****/
 
public class MuseumErr implements _IErrHolder
{
    public static final Result MUSEUM_ITEM_ALREADY_ACTIVE = Result.constInit(440001,"博物馆藏品已激活");
    public static final Result MUSEUM_ITEM_NOT_ACTIVE = Result.constInit(440002,"博物馆藏品未激活");
    public static final Result MUSEUM_ITEM_NOT_FOUND = Result.constInit(440003,"博物馆藏品未找到");
    public static final Result MUSEUM_ITEM_LEVEL_REACH_MAX = Result.constInit(440004,"博物馆藏品等级已达上限");
}
