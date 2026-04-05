package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 邮件错误
 ****/
 
public class MailErr implements _IErrHolder
{
    public static final Result MAIL_NOT_FOUND = Result.constInit(130001,"邮件不存在");
    public static final Result MAIL_TAKE_NOT_HAS_ITEM = Result.constInit(130002,"没有可领取的物品");
    public static final Result MAIL_TAKE_ALREADY_TAKEN = Result.constInit(130003,"邮件物品已经领取");
    public static final Result MAIL_LOCK_MAX = Result.constInit(130004,"已达收藏邮件的上限");
    public static final Result MAIL_DEL_HAS_ITEM = Result.constInit(130005,"有物品的邮件不能删除");
    public static final Result MAIL_DEL_LOCKED = Result.constInit(130006,"收藏邮件不能删除");
    public static final Result MAIL_DEL_MUST_READ = Result.constInit(130007,"必读未读的邮件不能删除");
    public static final Result MAIL_DEL_MUST_READ_OVER = Result.constInit(130008,"必读未读完的邮件不能删除");
    public static final Result MAIL_TAKE_HAS_ERROR = Result.constInit(130009,"邮件不能领取");
}
