package NPUSServer.GMCommand;

import NPCommon.GMCommand.CmdExecutorBase;
import NPUSServer.NPUserServer;

/*****
 * GM命令的执行者为US服务器
 */
public class UsCmdServerExecutor extends CmdExecutorBase
{
    private NPUserServer _m_userServer;

    public UsCmdServerExecutor(NPUserServer _server)
    {
    	_m_userServer = _server;
    }
    
    public NPUserServer getUsServer() {return _m_userServer;}

    @Override
    public String toString()
    {
        return "US:" + _m_userServer.getServerTypeId();
    }
}
