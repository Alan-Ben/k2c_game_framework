package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_WebPayOrderInfo;
import Common.ServerObj.ServerObj_WebPayOrderList;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_002_RetCreateWebPayOrder;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityCreateGameOrder;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormCreateGameOrderDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_005_WebPayOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;

import java.util.ArrayList;
import java.util.List;

/**
 * 游戏下单 - 生成订单号（3-11）
 *
 * 接收后台推送的下单请求，通过cid路由到对应UserServer批量创建订单，返回订单列表
 */
public class NP2HS_003_011_PlatFromCreateGameOrder extends _ANPPlatFormHttpSubDealer<NPEntityCreateGameOrder>
{
    @Override
    public int subOrder()
    {
        return 11;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityCreateGameOrder> getDecoder()
    {
        return NPPlatFormCreateGameOrderDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityCreateGameOrder _decodeObj)
    {
        // 参数校验
        if (_decodeObj.getCid() <= 0 || _decodeObj.getProductIds().isEmpty())
        {
            _commiter.commitFail(CommErr.PARAM_ERROR, "cid or product_ids empty");
            return;
        }

        // 通过cid解析目标UserServer ID
        int targetUsId = CommonFunc.parseServerTypeIdFromCid(_decodeObj.getCid());
        if (targetUsId <= 0)
        {
            _commiter.commitFail(CommErr.PARAM_ERROR, "cid error");
            return;
        }

        // 将商品ID字符串列表转为long列表
        List<Long> goodsIdList = new ArrayList<>();
        try
        {
            for (String productId : _decodeObj.getProductIds())
                goodsIdList.add(Long.parseLong(productId));
        }
        catch (NumberFormatException e)
        {
            _commiter.commitFail(CommErr.PARAM_ERROR, "product_ids format error");
            return;
        }

        CommLog.info("NP2HS_003_011_PlatFromCreateGameOrder.createOrder - cid:{}, productCount:{}, channel:{}",
                _decodeObj.getCid(), goodsIdList.size(), _decodeObj.getChannelCode());

        // 向UserServer发送创建订单请求
        NPHttpServer.getInstance().sendRequestToBSServer(
                EServerType.USER.ordinal(),
                targetUsId,
                NP2US_R_Writer_005_WebPayOp.make_002_ReqCreateWebPayOrder(
                        _decodeObj.getCid(), goodsIdList, _decodeObj.getChannelCode()),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_005_002_RetCreateWebPayOrder();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2US_RB_005_002_RetCreateWebPayOrder ret = (NP2US_RB_005_002_RetCreateWebPayOrder) _retProto;

                        if (ret.getErrCode() != 0)
                        {
                            // 下单失败，返回错误码和原因
                            String errMsg = "";
                            if (ResultMgr.getInstance().lookupResult(ret.getErrCode()) != null)
                                errMsg = ResultMgr.getInstance().lookupResult(ret.getErrCode()).getMsg();
                            _commiter.commitFail(ret.getErrCode(), errMsg);
                            return;
                        }

                        // 构造返回data：uid + order列表
                        JsonObject data = new JsonObject();
                        data.addProperty("uid", ret.getUid());
                        data.add("order", _buildOrderArray(ret.getOrderList()));
                        _commiter.commitSuc("", data.toString());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("NP2HS_003_011_PlatFromCreateGameOrder.createOrder - dealFail, cid:{}, errCode:{}",
                                _decodeObj.getCid(), _errCode);
                        _commiter.commitFail(_errCode);
                    }
                });
    }

    /**
     * 将订单列表转为JSON数组
     */
    private JsonArray _buildOrderArray(ServerObj_WebPayOrderList _orderList)
    {
        JsonArray arr = new JsonArray();
        if (_orderList == null || _orderList.getOrderList() == null)
            return arr;

        for (ServerObj_WebPayOrderInfo order : _orderList.getOrderList())
        {
            JsonObject item = new JsonObject();
            item.addProperty("order_id", order.getOrderId());
            item.addProperty("product_id", String.valueOf(order.getGoodsId()));
            item.addProperty("sdk_id", order.getSdkPayId());
            item.addProperty("amount", String.valueOf(order.getAmount()));
            item.addProperty("ext", "");
            arr.add(item);
        }
        return arr;
    }
}
