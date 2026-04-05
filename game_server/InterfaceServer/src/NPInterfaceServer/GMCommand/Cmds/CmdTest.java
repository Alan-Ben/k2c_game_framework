package NPInterfaceServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPInterfaceServer.NPInterfaceServer;

/**
 *
 */
@ACommander(comment = "服务器相关命令", name = "test")
public class CmdTest extends CmdClassBase
{
    @ACommand(comment = "发送钉钉预警消息")
    public String testSendDD()
    {
    	NPInterfaceServer.getInstance().getDDAlert().err("chat_connect_fail", "聊天服务器连接失败，次数：100");
    	
    	return "ok";
    }
}
