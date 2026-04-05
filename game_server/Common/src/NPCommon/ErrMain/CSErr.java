package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 通用服务器相关报错
 ****/
 
public class CSErr implements _IErrHolder
{
    public static final Result HANDLE_SERVER_NOT_FOUND = Result.constInit(290001,"找不到合适的负载对象");
    public static final Result HANDLE_OBJ_NOT_FOUND = Result.constInit(290002,"服务器对象不存在");
    public static final Result US_INFO_LIST_NOT_READY = Result.constInit(290003,"玩家服务器信息列表未就绪");
    public static final Result US_INFO_NOT_FOUND = Result.constInit(290004,"玩家服务器信息不存在");
    public static final Result US_INFO_SET_FFOM_FILE = Result.constInit(290006,"US信息设置从文件获取");
}
