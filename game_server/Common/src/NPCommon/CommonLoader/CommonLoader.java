package NPCommon.CommonLoader;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.ADelegateTwo;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;

/******
 * 通用的加载类，用于异步加载对象，重复加载都会回调。
 * @param <T>
 */
public class CommonLoader<T> implements _IHandlerHolder
{
    private boolean _m_isLoading=false;
    private MutexAtom _m_locker = new MutexAtom();
    private ADelegateTwo<Result, T> OnLoadOver = new ADelegateTwo<>(this);
    public void asyncLoad(_ILoader<T> _loader,HandlerTwo<Result,T> _handler)
    {
        _m_locker.lock();
        try
        {
            if(_m_isLoading)
            {
                OnLoadOver.addHandler(this,_handler);
                return;
            }
            else
            {
                _m_isLoading = true;
                OnLoadOver.addHandler(this,_handler);
            }
        }finally
        {
            _m_locker.unlock();
        }
        _loader.load(new HandlerTwo<Result, T>() {
            @Override
            public void handle(Result result, T _data)
            {
                onLoadOver(result, _data);
            }
        });

    }

    private void onLoadOver(Result result, T _data)
    {
        _m_locker.lock();
        try
        {
            _m_isLoading =false;
            OnLoadOver.onAsyncEvent(result,_data);
            OnLoadOver.clear();
        }finally
        {
            _m_locker.unlock();
        }

    }
}
