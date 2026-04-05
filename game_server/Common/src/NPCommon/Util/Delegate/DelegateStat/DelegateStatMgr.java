package NPCommon.Util.Delegate.DelegateStat;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.DelegateBase;
import NPCommon.Util.Delegate._IHandlerHolder;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicBoolean;

public class DelegateStatMgr implements _IHandlerHolder
{
    public static DelegateStatMgr getInstance()
    {
        return _instance;
    }

    private static DelegateStatMgr _instance = new DelegateStatMgr();

    private Map<Long, DelegateStat> _m_statMap = new ConcurrentHashMap<>();
    private AtomicBoolean _m_bInited = new AtomicBoolean(false);

    private DelegateStatMgr()
    {

    }

    /******
     * 检测到系统GC，清除无效的统计对象
     */
    @SuppressWarnings("unused")
    private void _listenOnSystemGc()
    {
        ALSynTaskManager.getInstance().regTask(() ->
        {
            _clearInvalidItems();
        }, 1000);
    }

    public int _clearInvalidItems()
    {
        List<Long> toRemove = null;
        for (DelegateStat stat : _m_statMap.values())
        {
            if (stat.isEmpty())
            {
                if (toRemove == null)
                {
                    toRemove = new ArrayList<>();
                }
                toRemove.add(stat.getId());
            }
        }
        if (null != toRemove)
        {
            for (Long id : toRemove)
            {
                _m_statMap.remove(id);
            }
            CommLog.info("DelegateStatMgr clear size:{}", toRemove.size());
        }
        return toRemove == null ? 0 : toRemove.size();
    }

    @SuppressWarnings("unlikely-arg-type")
    public void regist(DelegateBase _delegate)
    {
        if (null == _delegate)
            return;
        if (_m_statMap.get(_delegate) == null)
        {
            DelegateStat stat = new DelegateStat(_delegate);
            _m_statMap.put(_delegate.getId(), stat);
        }

    }

    public void init()
    {
        if (!_m_bInited.compareAndSet(false, true))
        {
            return;
        }
//        GcWatcher.getInstance().OnGC.addHandler(this,new HandlerOne<Long>() {
//            @Override
//            public void handle(Long aLong)
//            {
//                _listenOnSystemGc();
//            }
//        });
    }

    public List<DelegateStat> getStatList()
    {
        return new ArrayList<>(_m_statMap.values());
    }

    public DelegateStat lookupStat(long _id)
    {
        return _m_statMap.get(_id);
    }
}
