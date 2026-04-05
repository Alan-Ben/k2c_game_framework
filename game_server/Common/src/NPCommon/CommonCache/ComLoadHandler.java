package NPCommon.CommonCache;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;


/******
 * 加载回调对象，可以保证回调成功和超时不会二次调用。
 * @param <K>
 * @param <T>
 */
public class ComLoadHandler<K, T extends ComCachedDataBase>
{
    private ComCacheLoaderBase<K, T> _m_loader;
    private MutexAtom _m_locker = new MutexAtom();

    public ComLoadHandler(ComCacheLoaderBase<K, T> _loader)
    {
        _m_loader = _loader;
    }

    public void handle(Boolean _bSucc, T _data)
    {
        ComCacheLoaderBase<K, T> loader = takeLoader();
        if (null == loader)
            return;
        loader._dealAsyncLoadOver(_bSucc, _data);
    }


    public void handleTimeOut()
    {
        ComCacheLoaderBase<K, T> loader = takeLoader();
        if (null == loader)
            return;
        CommLog.error("Loader:{} load data expired time {}ms for key:{} ", loader.getClass().getSimpleName(), loader._getLoadTimeoutMs(), loader.getkey());
        loader._dealAsyncLoadOver(false, null);
    }

    private ComCacheLoaderBase<K, T> takeLoader()
    {
        ComCacheLoaderBase<K, T> loader = null;
        _m_locker.lock();
        try
        {
            if (null != _m_loader)
            {
                loader = _m_loader;
                _m_loader = null;
            }
        } finally
        {
            _m_locker.unlock();
        }
        return loader;
    }
}
