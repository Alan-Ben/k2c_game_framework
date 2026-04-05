package NPCommon.DDAlert;

import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Util.DelayExecutor.DelayExecutor;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class DDAlertDelayExecutor
{
    private static DDAlertDelayExecutor _m_instance = new DDAlertDelayExecutor();

    public static DDAlertDelayExecutor getInstance()
    {
        return _m_instance;
    }

    private Map<String, DelayExecutor> _m_executerMap = new ConcurrentHashMap<>();

    public synchronized DelayExecutor ensure(String _key, int _delayMs)
    {
        DelayExecutor executor = _m_executerMap.get(_key);
        if (null == executor)
        {
            executor = new DelayExecutor(_delayMs);
        }
        _m_executerMap.put(_key, executor);
        return executor;
    }

    public void delayExecute(String _key, _IALSynTask _task)
    {
        DelayExecutor executor = ensure(_key, 1000);
        executor.delayExecute(_task);
    }
}
