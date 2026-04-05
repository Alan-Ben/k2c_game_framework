package NPCommon.TickTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
public class TickTask implements _IALSynTask
{
    private _ITickingObj _m_tickingObj;
    private long _m_spanMs;

    public TickTask(_ITickingObj _obj,long _span)
    {
        _m_spanMs = _span;
        _m_tickingObj = _obj;
    }

    public void start()
    {
        ALSynTaskManager.getInstance().regTask(this, _m_spanMs);
    }

    @Override
    public void run()
    {
        try
        {
            long nowMs = CommonFunc.getNowTimeMS();
            _m_tickingObj.tick(nowMs);
            ALSynTaskManager.getInstance().regTask(this, _m_spanMs);
        }catch (Throwable e)
        {
            CommLog.error("",e);

        }
    }
}
