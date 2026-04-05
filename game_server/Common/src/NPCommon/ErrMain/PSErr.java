package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 平台服务器相关报错
 ****/
 
public class PSErr implements _IErrHolder
{
    public static final Result PS_NO_AREA = Result.constInit(300001,"未找到区域信息");
    public static final Result PS_NO_GS = Result.constInit(300002,"未找到GS");
}
