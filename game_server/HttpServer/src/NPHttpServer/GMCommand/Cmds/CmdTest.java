package NPHttpServer.GMCommand.Cmds;

import HttpServer.RPCDispatcher.Common.HSDDAlert_Handler;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPEnum.ENPDDAlertType;

/**
 * 专用测试的GM命令
 */
@ACommander(comment = "服务器相关命令", name = "test")
public class CmdTest extends CmdClassBase
{
    @ACommand(comment = "发送钉钉预警消息[消息类型，消息等级，消息标题，消息内容]")
    public String sendDDAlert(String _robotType, int _logLvl, String _title, String _content)
    {
    	ENPDDAlertType robotType = ENPDDAlertType.valueOf(_robotType);
    	
        HSDDAlert_Handler.getInstance().sendAlertToDD(robotType, _logLvl, _title, _content);
        
        return "ok";
    }
}
