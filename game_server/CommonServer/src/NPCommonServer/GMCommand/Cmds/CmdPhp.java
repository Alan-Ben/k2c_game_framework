package NPCommonServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommonServer.GMCommand.CSCmdBase;
import NPCommonServer.NPCommonServer;
import NPCommonServer.PHPParmMgr.CSPHPParamMgr;
import NPServerProtocolWriter.NP2LS.Msg.NP2LS_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2PS.Msg.NP2PS_Writer_001_BasicOp;

@ACommander(comment = "php相关命令", name = "php")
public class CmdPhp extends CSCmdBase
{
    @ACommand(comment = "禁止登录(bool是否禁止)")
    public void forbidLogin(boolean _isforbid)
    {
        //发送消息给plat广播LS消息
        NPCommonServer.getInstance().sendCustomMsgToPlat(
                NP2PS_Writer_001_BasicOp.make_002_BroadcastLS(
                        NP2LS_Writer_001_BasicOp.make_002_SetForbidenLogin(_isforbid)));
    }

    @ACommand(comment = "平台参数")
    public String phpParamInfo()
    {
        return CSPHPParamMgr.getInstance().toString();
    }
}
