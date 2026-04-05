package Interface.UsServer;

import AllRpcData.US_Service.Player.UsPlayerGainItemList;
import Common.Common_Context;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import RPC.RpcSender;
import WCGBasicServer._AWCGBasicServer;
import WCGCommon.Enum.NPEnum;

import java.util.List;

public class UsPlayerApi
{
    public static void gainItemList(_AWCGBasicServer _server, long _cid, boolean _isPush, List<NPCommonCostItem> _gainItemList, Common_Context _context, HandlerTwo<Result,NPCommon_ItemList> _handler)
    {
        gainItemListP(_server, _cid,_isPush,CommonFunc.costItemListToProto(_gainItemList),_context, _handler);
    }

    public static void gainItemListP(_AWCGBasicServer _server, long _cid, boolean _isPush, List<NPCommon_ItemInfo> _gainItemList, Common_Context _context, HandlerTwo<Result, NPCommon_ItemList> _handler)
    {
        UsPlayerGainItemList rpc = new UsPlayerGainItemList();
        rpc.req().setCid(_cid);
        rpc.req().getItemList().getItemList().addAll(_gainItemList);
        rpc.req().setContext(_context);
        rpc.req().setIsPush(_isPush);
        RpcSender sender = new RpcSender(_server, NPEnum.EServerType.USER);
        int serverTypeId= CommonFunc.parseServerTypeIdFromCid(_cid);
        sender.requestTo(serverTypeId, rpc, new RPC._ARpcCallBack<UsPlayerGainItemList>() {
            @Override
            public void call_back(int _errCode, UsPlayerGainItemList _rpc)
            {
                if (_errCode != 0)
                {
                    _handler.handle(CommErr.RPC_CALL_ERR, null);
                    return;
                }
                _handler.handle(new Result(_rpc.retObj().getResult()), _rpc.retObj().getGainItemList());
            }
        });
    }
}
