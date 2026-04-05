package NPCommon.CommonCache;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.ADelegateTwo;
import NPCommon.Util.Delegate.HandlerTwo;


/*****
 * 数据加载器的基类
 * @param <K>
 * @param <T>
 */
public abstract class ComCacheLoaderBase<K, T extends ComCachedDataBase> implements _ITimeKeyData<K>
{
    private volatile ELoadState _m_loadState = ELoadState.eNotLoad;//当前加载状态
    private volatile T _m_cacheData;  //要加载的数据
    private ADelegateTwo<Boolean, T> OnLoadOver = new ADelegateTwo<>(this);//加载完成后的回调
    private final K _m_key; //键值
    private int _m_iTimeStamp;//最后更新的时间戳
    private final MutexAtom _m_locker = new MutexAtom();

    protected void __lock()
    {
        _m_locker.lock();
    }

    protected void __unlock()
    {
        _m_locker.unlock();
    }

    public ComCacheLoaderBase(K _key)
    {
        _m_key = _key;
        if (_m_key.getClass() == Long.class || _m_key.getClass() == long.class)
        {
            Long key = (Long) _m_key;
            if (key == 0)
            {
                CommLog.error("Key ==0", new Exception());
            }
        }
    }

    public ELoadState getLoadState()
    {
        return _m_loadState;
    }

    public T getCacheData()
    {
        return _m_cacheData;
    }

    public abstract boolean _isExpired(); //判断加载器是否失效，失效的加载器需要重新加载

    public abstract long _getLoadTimeoutMs();//加载超时，<=0永远不超时，超时的话会回调加载失败。

    /******
     * 异步加载,加锁
     * @param _handler
     */
    public void asyncLoad(HandlerTwo<Boolean, T> _handler)
    {
        __lock();
        try
        {

            boolean bHandlerDealed = false;
            if (_m_cacheData != null)
            {//如果数据不为空
                bHandlerDealed = true;//设置回调已经处理了
                if (null != _handler) //直接回调给调用方
                {
                    ALSynTaskManager.getInstance().regTask(() ->
                    {
                        _handler.handle(true, _m_cacheData);
                    });
                }
            }

            if (_isExpired() || _m_cacheData == null)//过期或未加载，需要加载
            {
                if (null != _handler && !bHandlerDealed)
                {//调用方回调未处理，先加入回调列表
                    OnLoadOver.addHandler(null, _handler);
                }
                if (getLoadState() != ELoadState.eLoading)
                {//加载还未开始,开始加载

                    setLoadState(ELoadState.eLoading);
                    _asyncLoad();
                }
            }
        } finally
        {
            __unlock();
        }


    }


    /******
     * 设置加载状态
     * @param _eState
     */
    private void setLoadState(ELoadState _eState)
    {
        _m_loadState = _eState;
    }

    protected abstract void _doAsyncLoad(ComLoadHandler<K, T> handler);

    /*******
     * 执行加载逻辑，包含超时处理
     */
    private void _asyncLoad()
    {
        //创建加载回调对象，用来保证正常回调和超时回调的互斥
        ComLoadHandler<K, T> handler = new ComLoadHandler<K, T>(this);

        //调用子类的加载函数
        _doAsyncLoad(handler);

        //超时回调判断
        if (_getLoadTimeoutMs() > 0)
        {
            ALSynTaskManager.getInstance().regTask(() ->
            {
                handler.handleTimeOut();

            }, _getLoadTimeoutMs());
        }

    }

    /**********
     * 加载回调后调用
     * @param _bSucc
     * @param _data
     */
    public void _dealAsyncLoadOver(Boolean _bSucc, T _data)
    {
        __lock();
        try
        {
            if (!_bSucc || _data == null)
            {
                setLoadState(ELoadState.eNotLoad);
                OnLoadOver.onAsyncEvent(false, _m_cacheData);//加载失败，使用旧的数据
                CommLog.info("Cahce loader:{},load data key:{},failed!", getClass(), getkey());
            } else
            {
                setLoadState(ELoadState.eLoaded);
                _m_cacheData = _data;
                OnLoadOver.onAsyncEvent(true, _data);
            }
            OnLoadOver.clear();

        } finally
        {
            __unlock();
        }
    }

    @Override
    public K getkey()
    {
        return _m_key;
    }

    @Override
    public int getTimeStamp()
    {
        return _m_iTimeStamp;
    }

    @Override
    public void setTimeStamp(int _timeStamp)
    {
        _m_iTimeStamp = _timeStamp;

    }


}
