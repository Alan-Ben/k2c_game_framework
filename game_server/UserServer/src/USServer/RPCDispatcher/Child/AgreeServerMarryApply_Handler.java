package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.AgreeServerMarryApply;
import Common.ServerObj.ServerObj_AdultMarriedInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.AdultMarrySystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class AgreeServerMarryApply_Handler extends _ATBasicUSRpc_Handler<AgreeServerMarryApply> implements _IAutoRegistHandler
{
    @Override
	protected void _deal(NPUserServer _usServer, AgreeServerMarryApply _rpc)
	{
    	//本服池子不接受跨服请求
    	if(_usServer.getMatchAdultPool().isLocal())
    	{
    		_rpc.commitFail(CommErr.NOT_CROSS_GROUP_ERR.getCode());
			return;
    	}
    	
    	//处理跨服请求
    	NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ADULT_AGREE_POOL_APPLY);
    	AdultMarrySystem.DealAgreeGroupApply(_usServer, _rpc.req().getMarryInfo(), _rpc.req().getMatchId(), context, new _ICallBackIntT<ServerObj_AdultMarriedInfo>() 
    	{
			@Override
			public void onRunOver(int _errCode, ServerObj_AdultMarriedInfo _beMarried) 
			{
				if(_errCode > 0)
				{
					_rpc.commitFail(_errCode);
				}
				else
				{
					_rpc.retObj().setBeMarriedInfo(_beMarried);
					_rpc.commit();
				}
			}
		});
	}
}
