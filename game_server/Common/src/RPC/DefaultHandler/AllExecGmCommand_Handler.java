package RPC.DefaultHandler;

import AllRpcData.All_Service.AllExecGmCommand;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;
import RPC.RpcRequestHandler;
import RPC._IAutoRegistHandler;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class AllExecGmCommand_Handler extends RpcRequestHandler<AllExecGmCommand> implements _IAutoRegistHandler
{
    @Override
    public void deal(_IWCGBasicRequestCommiter _committer, AllExecGmCommand _rpc)
    {
        String command = _rpc.req().getCommand() + CommonFunc.joinString(null, _rpc.req().getExCommands());
        GmCommandMgr.getInstance().run(null, command, null, (_bSucc, _msg) ->
        {
            _rpc.retObj().setIsSucc(_bSucc);

            if (_msg.length() < 10 * 1024)
            {
                _rpc.retObj().getResultList().add(_msg);
            } else
            {
                _rpc.retObj().getResultList().addAll(StringFunc.splitString(_msg, 1024 * 10));
            }
            _rpc.commit();
        });
    }
}
