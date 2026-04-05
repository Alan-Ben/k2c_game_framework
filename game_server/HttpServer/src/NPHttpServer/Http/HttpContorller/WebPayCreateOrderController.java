package NPHttpServer.Http.HttpContorller;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_WebPayOrderInfo;
import Common.ServerObj.ServerObj_WebPayOrderList;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_002_RetCreateWebPayOrder;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;
import NPCommon.Util.HttpUtils;
import NPHttpServer.HttpServerConf;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_005_WebPayOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import org.apache.http.NameValuePair;
import org.apache.http.client.utils.URLEncodedUtils;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import static org.apache.http.Consts.UTF_8;

/**
 * 创建游戏订单接口
 *
 * 功能：批量创建游戏订单，用于购物车
 *
 * 请求参数：
 * - game_id: 游戏ID（必填）
 * - region_id: 平台区域ID（必填）
 * - cid: 角色ID（必填）
 * - server_id: 服务器ID（必填）
 * - product_ids: 商品ID列表（必填，格式：id1,id2,id3，可能重复，理论不超过20个）
 * - channel_code: 站点标识（可选）
 * - lang: 语言标识（必填，如 CN、EN）
 * - timestamp: 时间戳（必填，10位）
 * - sign: 签名（必填）
 *
 * 成功返回格式：
 * {
 *   "code": 1,
 *   "result": {
 *     "uid": "角色uid",
 *     "order": [
 *       {
 *         "order_id": "订单ID",
 *         "product_id": "商品ID",
 *         "sdk_id": "SDK档位ID",
 *         "amount": "美元价格",
 *         "ext": "透传参数"
 *       }
 *     ]
 *   },
 *   "msg": "success"
 * }
 *
 * 错误返回格式：
 * {
 *   "code": -3,
 *   "result": [6001, 6002],  // 异常商品ID列表
 *   "msg": "商品未定义"
 * }
 *
 * 注意事项：
 * - 部分商品无法下单时，整个事务直接异常返回，不需要下单
 * - 限购商品需要判断是否超过限制额度，给予异常返回
 * - 异常的商品通过result返回
 *
 * @author: system
 * @date: 2025-01-07
 */
public class WebPayCreateOrderController
{
    @RequestMapping(uri = "/webPay/createOrder", method = HttpMethod.POST)
    public void createOrder(HttpRequest _request, final HttpResponse _response)
    {
        try
        {
            // 解析POST表单数据
            List<NameValuePair> parseList = URLEncodedUtils.parse(_request.getPostBody(), UTF_8);
            HashMap<String, String> keyValMap = new HashMap<>();
            for (NameValuePair nameValuePair : parseList)
            {
                keyValMap.put(nameValuePair.getName(), nameValuePair.getValue());
            }

            // 签名验证
            if (!HttpUtils.checkWebPaySign(parseList, HttpServerConf.getInstance().getPlatFromClientKey()))
            {
                _response.response(401, "Sign check fail!");
                CommLog.error("WebPayCreateOrderController signCheck fail, data:{}", _request.getPostBody());
                return;
            }

            // 参数解析和验证
            String gameIdStr = keyValMap.get("game_id");
            String regionIdStr = keyValMap.get("region_id");
            String cidStr = keyValMap.get("cid");
            String serverIdStr = keyValMap.get("server_id");
            String productIdsStr = keyValMap.get("product_ids");
            String channelCode = keyValMap.get("channel_code");
            String lang = keyValMap.get("lang");
            String timestampStr = keyValMap.get("timestamp");

            // 必填参数校验
            if (gameIdStr == null || regionIdStr == null || cidStr == null ||
                serverIdStr == null || productIdsStr == null || lang == null || timestampStr == null)
            {
                _response.response(400, "Missing required parameters!");
                CommLog.error("WebPayCreateOrderController missing parameters, data:{}", _request.getPostBody());
                return;
            }

            // 参数类型转换
            int gameId = Integer.parseInt(gameIdStr);
            int regionId = Integer.parseInt(regionIdStr);
            long cid = Long.parseLong(cidStr);
            int serverId = Integer.parseInt(serverIdStr);
            long timestamp = Long.parseLong(timestampStr);

            // 解析商品ID列表
            List<String> productIds = __parseProductIds(productIdsStr);
            if (productIds == null || productIds.isEmpty())
            {
                _response.response(400, "Invalid product_ids format!");
                CommLog.error("WebPayCreateOrderController invalid product_ids:{}", productIdsStr);
                return;
            }

            // 理论不超过20个商品
            if (productIds.size() > 20)
            {
                _response.response(400, "Too many products! Max 20 allowed.");
                CommLog.error("WebPayCreateOrderController too many products:{}", productIds.size());
                return;
            }

            // 将 String 列表转换为 long 列表
            List<Long> goodsIdList = new ArrayList<>();
            for (String productId : productIds)
            {
                goodsIdList.add(Long.parseLong(productId));
            }

            // 调用UserServer创建订单
            NPHttpServer.getInstance().sendRequestToBSServer(
                EServerType.USER.ordinal(),
                serverId,
                NP2US_R_Writer_005_WebPayOp.make_002_ReqCreateWebPayOrder(cid, goodsIdList, channelCode != null ? channelCode : ""),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2US_RB_005_002_RetCreateWebPayOrder ret = (NP2US_RB_005_002_RetCreateWebPayOrder) _retProto;

                        // 根据errCode判断成功或失败
                        if (ret.getErrCode() == 0)
                        {
                            // 构造成功返回数据
                            JsonObject result = new JsonObject();
                            result.addProperty("code", 1);
                            result.addProperty("msg", "success");

                            // 将订单数据转换为 JSON 格式
                            JsonObject resultData = new JsonObject();
                            resultData.addProperty("uid", ret.getUid());
                            resultData.add("order", convertOrderListToJson(ret.getOrderList()));
                            result.add("result", resultData);

                            _response.response(200, result.toString());
                            CommLog.info("WebPayCreateOrderController createOrder success, cid:{}, productCount:{}", cid, productIds.size());
                        }
                        else
                        {
                            // 构造错误返回数据,包含失败的商品ID列表
                            Result errorResult = ResultMgr.getInstance().lookupResult(ret.getErrCode());
                            JsonObject response = new JsonObject();
                            response.addProperty("code", ret.getErrCode());
                            response.addProperty("msg", errorResult == null ? "Unknown error" : errorResult.getMsg());

                            // 将失败的商品ID数组拼接成字符串格式 [6001,6002]
                            StringBuilder failedGoodsStr = new StringBuilder("[");
                            if (ret.getFailedGoodsIds() != null && !ret.getFailedGoodsIds().isEmpty())
                            {
                                for (int i = 0; i < ret.getFailedGoodsIds().size(); i++)
                                {
                                    if (i > 0) failedGoodsStr.append(",");
                                    failedGoodsStr.append(ret.getFailedGoodsIds().get(i));
                                }
                            }
                            failedGoodsStr.append("]");
                            response.addProperty("result", failedGoodsStr.toString());

                            _response.response(200, response.toString());
                        }
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        // 网络通信失败等异常情况,返回通用错误
                        Result result = ResultMgr.getInstance().lookupResult(_errCode);

                        JsonObject response = new JsonObject();
                        response.addProperty("code", _errCode);
                        response.addProperty("msg", result == null ? "Unknown error" : result.getMsg());
                        _response.response(200, response.toString());

                        CommLog.error("WebPayCreateOrderController dealFail error, cid:{}, errCode:{}", cid, _errCode);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_005_002_RetCreateWebPayOrder();
                    }
                });

        }
        catch (NumberFormatException e)
        {
            _response.response(400, "Invalid parameter format!");
            CommLog.error("WebPayCreateOrderController parameter format error, exception:{}", e);
        }
        catch (Exception e)
        {
            _response.response(500, "Internal server error!");
            CommLog.error("WebPayCreateOrderController exception:{}", e);
        }
    }

    /**
     * 解析商品ID列表
     *
     * 格式：id1,id2,id3
     * 商品ID可能重复，需要生成多笔订单
     *
     * @param _productIdsStr 商品ID字符串
     * @return 商品ID列表
     */
    private List<String> __parseProductIds(String _productIdsStr)
    {
        if (_productIdsStr == null || _productIdsStr.trim().isEmpty())
        {
            return null;
        }

        List<String> productIds = new ArrayList<>();
        String[] ids = _productIdsStr.split(",");

        for (String id : ids)
        {
            String trimmedId = id.trim();
            if (!trimmedId.isEmpty())
            {
                productIds.add(trimmedId);
            }
        }

        return productIds;
    }

    /**
     * 将订单列表转换为 JSON 数组
     *
     * @param _orderList 订单列表数据
     * @return JSON 数组
     */
    private static JsonArray convertOrderListToJson(ServerObj_WebPayOrderList _orderList)
    {
        JsonArray jsonArray = new JsonArray();

        if (_orderList == null || _orderList.getOrderList() == null)
        {
            return jsonArray;
        }

        for (ServerObj_WebPayOrderInfo order : _orderList.getOrderList())
        {
            JsonObject orderJson = new JsonObject();
            orderJson.addProperty("order_id", order.getOrderId());
            orderJson.addProperty("product_id", order.getGoodsId());
            orderJson.addProperty("sdk_id", order.getSdkPayId());
            orderJson.addProperty("amount", order.getAmount());
            // ext 字段可以在这里添加透传参数
            orderJson.addProperty("ext", "");

            jsonArray.add(orderJson);
        }

        return jsonArray;
    }
}
