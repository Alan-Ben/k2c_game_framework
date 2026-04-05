package NPCommon.Util.CallBack;

import NPCommon.ErrMain.Result.Result;

/******
 * 带Result类型的回调
 */
@FunctionalInterface
public interface _ICallBackResultT<T>
{
    void onRunOver(Result _result, T _t);
}
