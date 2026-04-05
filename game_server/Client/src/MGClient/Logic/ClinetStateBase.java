package MGClient.Logic;


import NPCommon.Log.CommLog;

public abstract class ClinetStateBase
{
    private LogicBase _m_logic;
    private int _m_tickCount = 0;

    public ClinetStateBase(LogicBase _logic)
    {
        _m_logic = _logic;
    }

    public void tick()
    {
        processState();
        _m_tickCount++;
    }

    public int getTickCount()
    {
        return _m_tickCount;
    }

    public LogicBase getLogic()
    {
        return _m_logic;
    }

    public abstract void processState();

    public void onLeaveState()
    {
        CommLog.info("===leave State[{}]", getClass().getSimpleName());
    }

    public void onEnterState()
    {
        CommLog.info("===Enter State[{}]", getClass().getSimpleName());
    }
}
