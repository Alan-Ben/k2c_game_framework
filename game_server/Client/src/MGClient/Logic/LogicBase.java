package MGClient.Logic;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import MGClient.ClientPlayer.ClientPlayer;
import NPCommon.Log.CommLog;

public abstract class LogicBase implements _IALSynTask
{

    private ClientPlayer _m_Owner;
    private boolean _m_bStoped = false;
    private int _m_tickCount = 0;
    private ClinetStateBase _m_curState = null;

    public void Init(ClientPlayer _player)
    {
        _m_Owner = _player;
        _initLogic();
    }

    public ClientPlayer getOwner()
    {
        return _m_Owner;
    }

    public void startRun()
    {
        ALSynTaskManager.getInstance().regTask(this, 1000);
    }

    public void stop()
    {
        this._m_bStoped = true;
        _clear();
        _m_Owner = null;

    }

    public int getTickCount()
    {
        return _m_tickCount;
    }

    private void tick()
    {
        _m_tickCount++;
        if (null != _m_curState)
        {
            _m_curState.tick();
        }
    }

    protected abstract void _initLogic();

    protected abstract void _clear();

    @Override
    public void run()
    {
        if (null == getOwner()) return;
        if (_m_bStoped)
        {
            return;
        }
        try
        {
            tick();
        } catch (Exception e)
        {
            CommLog.error("{} tick exception:", getClass().getName(), e);
        }

        ALSynTaskManager.getInstance().regTask(this, 1000);

    }

    public void changeState(ClinetStateBase newState)
    {
        if (null != _m_curState)
        {
            _m_curState.onLeaveState();
        }
        _m_curState = newState;
        _m_curState.onEnterState();
    }

}
