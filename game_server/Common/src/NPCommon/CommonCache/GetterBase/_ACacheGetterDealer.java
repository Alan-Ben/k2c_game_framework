package NPCommon.CommonCache.GetterBase;

import NPCommon.CommonCache.ComCachedDataBase;
import NPCommon.CommonCache._IListAdapter;
import NPCommon.Promise.Promise;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import WCGBasicServer._AWCGBasicServer;

import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public abstract class _ACacheGetterDealer<CacheData extends ComCachedDataBase, Env extends _ICacheGetterEnv<CacheData>, Y>
{
    private _ACacheGetter<CacheData, Env> _m_getter;

    public _ACacheGetterDealer(_ACacheGetter<CacheData, Env> _getter)
    {
        _m_getter = _getter;
    }

    public _AWCGBasicServer getServer()
    {
        return _m_getter.getServerObj();
    }

    public _ACacheGetter<CacheData, Env> getGetter()
    {
        return _m_getter;
    }

    public Env getEnv()
    {
        return _m_getter.getEnv();
    }

    /**
     * 获取信息列表
     * @param _adapter 玩家cid列表的适配器
     * @param _handler 回调
     */
    public void getInfoList(_IListAdapter<Long> _adapter, HandlerOne<Map<Long, Y>> _handler)
    {
        Map<Long, Y> infoMap = new ConcurrentHashMap<>();
        Promise promise = new Promise();

        for (int i = 0; i < _adapter.size(); i++)
        {
            int finalIndex = i;
            Long key = _adapter.get(finalIndex);
            promise.then(i, p -> getInfo(key, new HandlerTwo<Boolean, Y>()
            {
                @Override
                public void handle(Boolean _suc, Y _showInfo)
                {
                    if (!_suc || _showInfo == null)
                    {
                        promise.commit(finalIndex);
                        return;
                    }

                    infoMap.put(key, _showInfo);
                    promise.commit(finalIndex);
                }
            }));
        }

        promise.over(p ->
        {
            _handler.handle(infoMap);
        });
    }

    /**
     * 获取信息列表
     * @param _cidList 玩家cid列表
     * @param _handler 回调
     */
    public void getInfoListA(List<Long> _cidList, HandlerOne<Map<Long, Y>> _handler)
    {
        Map<Long, Y> infoMap = new ConcurrentHashMap<>();
        Promise promise = new Promise();

        for (int i = 0; i < _cidList.size(); i++)
        {
            int finalIndex = i;
            Long key = _cidList.get(finalIndex);
            promise.then(i, p -> getInfo(key, new HandlerTwo<Boolean, Y>()
            {
                @Override
                public void handle(Boolean _suc, Y _showInfo)
                {
                    if (!_suc || _showInfo == null)
                    {
                        promise.commit(finalIndex);
                        return;
                    }

                    infoMap.put(key, _showInfo);
                    promise.commit(finalIndex);
                }
            }));
        }

        promise.over(p ->
        {
            _handler.handle(infoMap);
        });
    }

    /**
     * 获取信息
     * @param _cid     玩家cid
     * @param _handler 回调
     */
    public abstract void getInfo(long _cid, HandlerTwo<Boolean, Y> _handler);
}
