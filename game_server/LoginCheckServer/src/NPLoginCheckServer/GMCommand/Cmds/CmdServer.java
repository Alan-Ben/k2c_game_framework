package NPLoginCheckServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;

/**
 *
 */
@ACommander(comment = "服务器相关命令", name = "server")
public class CmdServer extends CmdClassBase
{
    @ACommand(comment = "显示服务器信息")
    public String info()
    {
        return "this is lcs";
    }
}
