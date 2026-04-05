package ShareCodeCenter.ShareCodeServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import ShareCodeCenter.ShareCodeServer.GMCommand.SCSCmdBase;

@ACommander(comment = "服务器相关命令", name = "server")
public class CmdServer extends SCSCmdBase
{
    @ACommand(comment = "显示服务器信息")
    public String info()
    {
        return "Share Code Server";

    }
}
