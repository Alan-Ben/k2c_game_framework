package NPGateServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGateServer.GMCommand.GSCmdBase;
import NPGateServer.NPClientReloginMgr;
import NPGateServer.USServerInfoListMgr.USServerIndexInfoListMgr;

/**
 *
 */
@ACommander(comment = "服务器相关命令", name = "server")
public class CmdServer extends GSCmdBase
{
    @ACommand(comment = "US服务器列表")
    public String USList()
    {
        return USServerIndexInfoListMgr.getInstance().toString();
    }

    @ACommand(comment = "清空所有重连会话")
    public String clearRelogin()
    {
        NPClientReloginMgr.getInstance().clear();
        return "clear relogin session done.";
    }
}
