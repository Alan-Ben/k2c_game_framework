package NPCommon.Util.RepeatExecutor;

import NPCommon.Util.CallBack._ICallBackBoolT;

public interface _IRunner<T>
{
    void run(_ICallBackBoolT<T> _callBack);
}
