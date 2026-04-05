package DinnerServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;

/**
 *
 */
@ACommander(comment = "服务器相关命令", name = "server")
public class CmdServer extends CmdClassBase
{
    @ACommand(comment = "显示服务器信息")
    public String info()
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("Pid:%d\n", CommonFunc.getPid()));

        String sInfo = sb.toString();
        CommLog.info(sInfo);
        return sInfo;
    }

}
