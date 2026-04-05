package NPCommon.Util.RepeatExecutor;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Util.CallBack._ICallBackBoolT;

import java.util.concurrent.atomic.AtomicLong;

/*******
 * 这是一个重试执行器,用于执行一些可能会失败的操作,如果失败了,会重试执行,直到成功或者重试次数用完
 * 并且能返回执行的结果
 * @param <T>
 */
public class RepeatExecutor<T>
{
    private _IRunner<T> _m_runner;
    public AtomicLong _m_counter;
    public int _m_spanMs;

    public RepeatExecutor(int _repeatNum, int _span, _IRunner<T> _runner)
    {
        _m_counter = new AtomicLong(_repeatNum < 0 ? Long.MAX_VALUE : _repeatNum);
        _m_runner = _runner;
        _m_spanMs = _span;
    }

    public void repeat(_ICallBackBoolT<T> _handler)
    {
        _ICallBackBoolT<T> callBackT = new _ICallBackBoolT<T>()
        {
            @Override
            public void onRunOver(boolean _isSucc, T _obj)
            {
                if (!_isSucc)
                {//执行失败了
                    long count = _m_counter.decrementAndGet();
                    if (count <= 0)
                    {//重试次数已经用完，返回失败回调
                        if (null != _handler)
                        {
                            _handler.onRunOver(false, _obj);
                        }
                    } else
                    {//还有重试次数，再重试执行
                        ALSynTaskManager.getInstance().regTask(() -> _m_runner.run(this), _m_spanMs);
                    }
                } else
                {//执行成功，返回
                    if (null != _handler)
                    {
                        _handler.onRunOver(true, _obj);
                    }
                }
            }
        };
        _m_runner.run(callBackT);
    }

    /******
     * 重试执行，不返回结果
     * @param retryNum 重试次数
     * @param _span 重试间隔
     * @param _runner 执行器
     */
    public static <T> void repeatNoResult(int retryNum, int _span, _IRunner<T> _runner)
    {
        RepeatExecutor<T> repeatExecutor = new RepeatExecutor<>(retryNum, _span, _runner);
        repeatExecutor.repeat(null);
    }
}
