package NPCommon.ErrMain;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 倒计时事件相关错误
 ****/
 
public class CountdownEventErr implements _IErrHolder
{
    public static final Result NO_ONGOING_COUNTDOWN_EVENT = Result.constInit(420001,"没有进行中的倒计时事件");
    public static final Result COUNTDOWN_EVENT_RELATIVE_QUEST_NOT_FOUND = Result.constInit(420002,"倒计时事件相关的任务不存在");
    public static final Result COUNTDOWN_EVENT_IN_TIME = Result.constInit(420004,"倒计时事件未超时");
}
