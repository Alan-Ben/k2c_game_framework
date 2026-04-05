package NPUSServer.GMCommand;

import NPCommon.GMCommand.CmdExecutorBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/*****
 * GM命令的执行者为某个玩家
 */
public class UsCmdPlayerExecutor extends CmdExecutorBase
{
    private NPUSUserData _m_userData;

    public UsCmdPlayerExecutor(NPUSUserData _userData)
    {
        _m_userData = _userData;
    }

    public NPUSUserData getUserData()
    {
        return _m_userData;
    }

    @Override
    public String toString()
    {
        return "Player:" + _m_userData.getCid();
    }
}
