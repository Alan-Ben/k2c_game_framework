package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 SS服务器活动排期错误
 ****/
 
public class SSActivityScheduleErr implements _IErrHolder
{
    public static final Result PARSE_ERR = Result.constInit(500001,"解析内容错误");
    public static final Result INVALID_ZONEID_ERR = Result.constInit(500002,"无效分区ID");
    public static final Result NOT_LATEST_ERR = Result.constInit(500003,"不是最新数据");
    public static final Result BUILD_FILE_FAIL = Result.constInit(500004,"生成文件失败");
    public static final Result DOWNLOAD_FAIL = Result.constInit(500005,"下载文件失败");
    public static final Result SCHEDULE_NOT_FOUND = Result.constInit(500006,"排期不存在");
    public static final Result SUBMIT_COUNT_TOO_LOW = Result.constInit(500007,"版本号过低");
    public static final Result SCHEDULE_NOT_ACTIVE = Result.constInit(500008,"排期未激活");
    public static final Result GROUP_LIST_MISMATCH = Result.constInit(500009,"分组列表不匹配");
    public static final Result GROUP_LIST_REPEAT = Result.constInit(500010,"分组列表重复");
}
