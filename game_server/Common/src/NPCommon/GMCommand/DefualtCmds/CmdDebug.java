package NPCommon.GMCommand.DefualtCmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.ServerGMUtil;
import NPCommon.Util.StringFunc;

@ACommander(comment = "调试命令", name = "debug")
public class CmdDebug extends CmdClassBase
{
    @ACommand(comment = "设置调试玩家(cid)")
    public void setDebugger(long _cid)
    {
        ServerGMUtil.routeGmCommand("cs server setDebugger " + _cid, new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _isSucc, String s)
            {
                takeCallBack().onRunOver(_isSucc, s);
            }
        });
    }

    @ACommand(comment = "显示调试玩家")
    public void get()
    {
        ServerGMUtil.routeGmCommand("cs server getDebugger ", new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _isSucc, String s)
            {
                takeCallBack().onRunOver(_isSucc, s);
            }
        });
    }

    @ACommand(comment = "发送调试信息(调试内容)")
    public void send(String _msg)
    {
        String encodeStr = StringFunc.encodeString(_msg);
        ServerGMUtil.routeGmCommand("cs server sendDebugMsg " + encodeStr, new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _isSucc, String s)
            {
                takeCallBack().onRunOver(_isSucc, s);
            }
        });
    }
}
