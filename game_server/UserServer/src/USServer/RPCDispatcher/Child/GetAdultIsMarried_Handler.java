package USServer.RPCDispatcher.Child;

import AllRpcData.US_Service.Child.GetAdultIsMarried;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult._AAdultInfo;
import NPUSServer.NPUserServer;
import NPUSServer.UserOfflineTmpDataMgr.AdultInfo.UserOfflineTmpDataInfo_AdultInfo;
import NPUSServer.UserOfflineTmpDataMgr.Const_UsOfflineTmpData;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑： 获取联姻池指定子嗣数据
 */
public class GetAdultIsMarried_Handler extends _ATBasicUSRpc_Handler<GetAdultIsMarried>  implements _IAutoRegistHandler
{
    @Override
    public void _deal(NPUserServer _usServer, GetAdultIsMarried _rpc)
	{
		//查询玩家信息
		NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_rpc.req().getCid());
		if(null != userData)
		{
			userData.safeCall(()->
			{
				//查询数据直接返回
				_AAdultInfo adultInfo = userData.getChildComponent().getAdultMgr().lookupAdult(_rpc.req().getAdultId());
				if(null == adultInfo) {
					_rpc.commitFail(CommErr.OBJ_ERR.getCode());
				}
				else {
					_rpc.retObj().setIsGiftde(adultInfo.getIsGiftde());
					_rpc.retObj().setIsMarried(adultInfo.isMarried());
					_rpc.commit();
				}
			});
		}
		else
		{
			//调用离线处理数据
			_usServer.getOfflineTmpDataCore().localDoOfflineData(Const_UsOfflineTmpData.C_TmpDataType_AdultInfo, _rpc.req().getCid(), _rpc.req().getAdultId()
					, new _ICallBackResultT<UserOfflineTmpDataInfo_AdultInfo>() {
						@Override
						public void onRunOver(Result _result, UserOfflineTmpDataInfo_AdultInfo _adultInfo) {
							if(!_result.isSucc())
							{
								_rpc.commitFail(_result.getCode());
								return ;
							}

							//返回子嗣是否已婚的处理
							_rpc.retObj().setIsGiftde(_adultInfo.getIsGifted());
							_rpc.retObj().setIsMarried(_adultInfo.getIsMarried());
							_rpc.commit();
						}
					});
		}
	}
}
