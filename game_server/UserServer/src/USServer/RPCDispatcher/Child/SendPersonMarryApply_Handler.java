package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.SendPersonMarryApply;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class SendPersonMarryApply_Handler extends _ATBasicUSRpc_Handler<SendPersonMarryApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, SendPersonMarryApply _rpc)
	{
		boolean refuseMarry = _usServer.getRefuseMarryMgr().isRefuseMarry(_rpc.req().getApply().getTargetCid());
		if (refuseMarry)
		{
			_rpc.retObj().setIsPlayerRefuseAllRequest(true);

		} else
		{
			NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_SEND_PLAYER_APPLY);
			AdultMarrySystem.DealAcceptPlayerApply(_usServer, _rpc.req().getApply(), context);
		}

		_rpc.commit();
	}
}
