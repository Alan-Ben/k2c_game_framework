package NPUSServer.USGroup.LocalActivityController.LogicUnit;

import NPUSServer.NPUserServer;
import NPUSServer.USGroup.LocalActivityController._ALocalActivityLogicUnit;

/**
 * 切换服务器分组
 */
public class ChangeCrossServerGroupLogicUnit extends _ALocalActivityLogicUnit
{
    private NPUserServer _m_server;

    public ChangeCrossServerGroupLogicUnit(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public boolean needBreakAndRepeatLoopIfPass()
    {
        return true;
    }

    @Override
    public boolean tryExecuteLogic()
    {
        return getUSServer().getLocalCrossServerGroupMgr().tryChangeGroup();
    }

    @Override
    public int executePriority()
    {
        return 1;
    }
}
