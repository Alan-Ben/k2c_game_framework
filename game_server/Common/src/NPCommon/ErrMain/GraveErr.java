package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 杰出者系统错误
 ****/
 
public class GraveErr implements _IErrHolder
{
    public static final Result GRAVE_RECORD_NOT_FOUND = Result.constInit(550001,"无法找到记录");
    public static final Result GRAVE_CELE_TODAY_ERROR = Result.constInit(550002,"今日无膜拜次数");
    public static final Result GRAVE_CONG_NOT_NEW_INFO = Result.constInit(550003,"无可以祝贺的新晋杰出者");
}
