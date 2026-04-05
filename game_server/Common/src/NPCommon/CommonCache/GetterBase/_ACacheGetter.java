package NPCommon.CommonCache.GetterBase;

import NPCommon.CommonCache.ComCachedDataBase;
import NPCommon.CommonCache._IListAdapter;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import WCGBasicServer._AWCGBasicServer;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public abstract class _ACacheGetter<CacheData extends ComCachedDataBase, Env extends _ICacheGetterEnv<CacheData>>
{
    //服务器对象
    private _AWCGBasicServer _m_sServer;
    private Env _m_env;
    protected Map<Class<?>, _ACacheGetterDealer<CacheData, Env, ?>> _m_dealerMap;

    public _ACacheGetter(_AWCGBasicServer _server)
    {
        _m_sServer = _server;
        _m_dealerMap = new HashMap<>();

        //开始注册处理器
        _startRegDealer();
    }

    public _ACacheGetter(_AWCGBasicServer _server, Env _env)
    {
        this(_server);
        _m_env = _env;

        //开始注册处理器
        _startRegDealer();
    }

    protected abstract void _startRegDealer();

    protected void _regDealer(Class<?> _clazz, _ACacheGetterDealer<CacheData, Env, ?> _dealer)
    {
        _m_dealerMap.put(_clazz, _dealer);
    }

    public _AWCGBasicServer getServerObj()
    {
        return _m_sServer;
    }

    public Env getEnv()
    {
        return _m_env;
    }

    @SuppressWarnings("unchecked")
    public <Y> _ACacheGetterDealer<CacheData, Env, Y> getDealer(Class<Y> _clazz)
    {
        return (_ACacheGetterDealer<CacheData, Env, Y>) _m_dealerMap.get(_clazz);
    }

    /**
     * 获取信息
     * @param _clazz   所需要的数据类名
     * @param _cid     玩家cid
     * @param _handler 回调
     * @param <T>      目标数据类
     */
    public <Y> void getInfo(Class<Y> _clazz, long _cid, HandlerTwo<Boolean, Y> _handler)
    {
        _ACacheGetterDealer<CacheData, Env, Y> dealer = getDealer(_clazz);
        if (dealer == null)
        {
            _handler.handle(false, null);
            return;
        }

        dealer.getInfo(_cid, _handler);
    }

    /**
     * 获取信息
     * @param _clazz   所需要的数据类名
     * @param _adapter 玩家cid列表的适配器
     * @param _handler 回调
     * @param <T>      目标数据类
     */
    public <Y> void getInfoList(Class<Y> _clazz, _IListAdapter<Long> _adapter, HandlerOne<Map<Long, Y>> _handler)
    {
        _ACacheGetterDealer<CacheData, Env, Y> dealer = getDealer(_clazz);
        if (dealer == null)
        {
            _handler.handle(new HashMap<>());
            return;
        }

        dealer.getInfoList(_adapter, _handler);
    }

    /**
     * 获取信息
     * @param _clazz   所需要的数据类名
     * @param _cidList 玩家cid列表
     * @param _handler 回调
     * @param <T>      目标数据类
     */
    public <Y> void getInfoListA(Class<Y> _clazz, List<Long> _cidList, HandlerOne<Map<Long, Y>> _handler)
    {
        _ACacheGetterDealer<CacheData, Env, Y> dealer = getDealer(_clazz);
        if (dealer == null)
        {
            _handler.handle(new HashMap<>());
            return;
        }

        dealer.getInfoListA(_cidList, _handler);
    }
}
