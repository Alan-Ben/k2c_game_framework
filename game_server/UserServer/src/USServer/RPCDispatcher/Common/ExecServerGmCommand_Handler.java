package USServer.RPCDispatcher.Common;

import AllRpcData.US_Service.Common.ExecServerGmCommand;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.NPCmdLineContext;
import NPCommon.Util.ServerGMUtil;
import NPEnum.ENPGameEvent;
import NPUSServer.GMCommand.UsCmdServerExecutor;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class ExecServerGmCommand_Handler extends _ATBasicUSRpc_Handler<ExecServerGmCommand> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, ExecServerGmCommand _rpc)
    {
        //先检查是否路由到别的服务器处理
        boolean isOp = ServerGMUtil.routeGmCommand(_usServer, _rpc.req().getCommand(), new HandlerTwo<Boolean, String>()
        {
            @Override
            public void handle(Boolean _isSucc, String _result)
            {
                _rpc.retObj().setIsSucc(_isSucc);
                _rpc.retObj().setResult(_result);
                _rpc.commit();
            }
        });

        //如果已经路由到别的服务器则不处理
        if (isOp)
        {
            return;
        }

        //尝试在本服处理数据
        NPCmdLineContext context = NPCmdLineContext.createNew(ENPGameEvent.GM_CMD);
        GmCommandMgr.getInstance().run(new UsCmdServerExecutor(_usServer), _rpc.req().getCommand(), context, new _IRunCallBack()
        {
            @Override
            public void onRunOver(boolean _isSucc, String _result)
            {
                _rpc.retObj().setIsSucc(_isSucc);
                _rpc.retObj().setResult(_result);
                _rpc.commit();
            }
        });
    }
}
