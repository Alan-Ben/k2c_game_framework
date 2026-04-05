package NPCommon.CommonLoader;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerTwo;

public interface _ILoader<T>
{
    void load(HandlerTwo<Result, T> _handler);
}
