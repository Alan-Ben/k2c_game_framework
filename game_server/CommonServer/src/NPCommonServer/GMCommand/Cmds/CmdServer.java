package NPCommonServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.ServerGMUtil;
import NPCommonServer.GMCommand.CSCmdBase;
import NPCommonServer.NPCommParams;
import NPCommonServer.PHPParmMgr.CSPHPParamMgr;
import NPCommonServer.USServerListMgr.USServerListMgrCSInstance;
import WCGCommon.Enum.ECommParam;

/**
 *
 */
@ACommander(comment = "服务器相关命令", name = "server")
public class CmdServer extends CSCmdBase
{
    @ACommand(comment = "显示服务器信息")
    public String info()
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("Pid:%d\n", CommonFunc.getPid()));

        sb.append("Comm Params:\n");
        sb.append(NPCommParams.getInstance().toString());
        String sInfo = sb.toString();
        CommLog.info(sInfo);
        return sInfo;

    }

    @ACommand(comment = "设置调试玩家(cid)")
    public String setDebugger(long _cid)
    {
        NPCommParams.getInstance().setParam(ECommParam.SERVER_DEBUGGER_CID, _cid);
        return "set debugger to " + _cid + " ok";
    }

    @ACommand(comment = "显示调试玩家")
    public String getDebugger()
    {
        long cid = NPCommParams.getInstance().getParam(ECommParam.SERVER_DEBUGGER_CID);
        return "cur debugger is " + cid;
    }

    @ACommand(comment = "发送调试信息")
    public void sendDebugMsg(String _msg)
    {
        long cid = NPCommParams.getInstance().getParam(ECommParam.SERVER_DEBUGGER_CID);
        if (cid == 0)
        {
            takeCallBack().onRunOver(false, "no debugger");
            return;
        }
        int serverTypeId = 1;

        String command = String.format("us %d server sendMsg2Client %d %s", serverTypeId, cid, _msg);
        ServerGMUtil.routeGmCommand( command, new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _isSucc, String s)
            {
                takeCallBack().onRunOver(_isSucc, s);
            }
        });

    }

    @ACommand(comment = "再次发起后台请求US列表")
    public String reqUSServerListFromPHP()
    {
    	USServerListMgrCSInstance.getInstance().reqUSServerListFromPHP();
        return "send ok.";
    }
    
    @ACommand(comment = "平台参数列表")
    public String getPHPParams()
    {
    	return CSPHPParamMgr.getInstance().toString();
    }
}
