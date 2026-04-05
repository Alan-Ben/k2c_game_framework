package NPHttpServer.Http.HttpContorller;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_WebPayGoodsInfo;
import Common.ServerObj.ServerObj_WebPayGoodsList;
import NP2US_RB.p005_WebPayOp.NP2US_RB_005_001_RetGetWebPayGoodsList;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.HttpUtils;
import NPEnum.ENPItemType;
import NPHttpServer.HttpServerConf;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_005_WebPayOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import org.apache.http.NameValuePair;
import org.apache.http.client.utils.URLEncodedUtils;

import java.util.HashMap;
import java.util.List;

import static org.apache.http.Consts.UTF_8;

/**
 * 可购商品列表接口
 *
 * 功能：查询角色可购买的商品列表
 *
 * 请求参数：
 * - game_id: 游戏ID（必填）
 * - region_id: 平台区域ID（必填）
 * - cid: 角色ID（必填）
 * - server_id: 服务器ID（必填）
 * - channel_code: 渠道标识（可选，用于区分赠送物品）
 * - lang: 语言标识（必填，如 CN、EN）
 * - timestamp: 时间戳（必填，10位）
 * - sign: 签名（必填）
 *
 * 返回格式：
 * {
 *   "code": 1,
 *   "result": [商品列表数组],
 *   "msg": "success"
 * }
 *
 * @author: system
 * @date: 2025-01-07
 */
public class WebPayGetGoodsListController
{
    @RequestMapping(uri = "/webPay/getGoodsList", method = HttpMethod.POST)
    public void getGoodsList(HttpRequest _request, final HttpResponse _response)
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
                CommLog.error("WebPayGetGoodsListController signCheck fail, data:{}", _request.getPostBody());
                return;
            }

            // 参数解析和验证
            String gameIdStr = keyValMap.get("game_id");
            String regionIdStr = keyValMap.get("region_id");
            String cidStr = keyValMap.get("cid");
            String serverIdStr = keyValMap.get("server_id");
            String channelCode = keyValMap.get("channel_code");
            String lang = keyValMap.get("lang");
            String timestampStr = keyValMap.get("timestamp");

            // 必填参数校验
            if (gameIdStr == null || regionIdStr == null || cidStr == null ||
                serverIdStr == null || lang == null || timestampStr == null)
            {
                _response.response(400, "Missing required parameters!");
                CommLog.error("WebPayGetGoodsListController missing parameters, data:{}", _request.getPostBody());
                return;
            }

            // 参数类型转换
            int gameId = Integer.parseInt(gameIdStr);
            int regionId = Integer.parseInt(regionIdStr);
            long cid = Long.parseLong(cidStr);
            int serverId = Integer.parseInt(serverIdStr);
            long timestamp = Long.parseLong(timestampStr);

            // 调用UserServer获取商品列表
            NPHttpServer.getInstance().sendRequestToBSServer(
                EServerType.USER.ordinal(),
                serverId,
                NP2US_R_Writer_005_WebPayOp.make_001_ReqGetWebPayGoodsList(cid, channelCode != null ? channelCode : ""),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2US_RB_005_001_RetGetWebPayGoodsList ret = (NP2US_RB_005_001_RetGetWebPayGoodsList) _retProto;

                        // 构造返回数据
                        JsonObject result = new JsonObject();
                        result.addProperty("code", 1);
                        result.addProperty("msg", "success");

                        // 将商品列表转换为 JSON 格式
                        result.add("result", convertGoodsListToJson(ret.getGoodsList()));

                        _response.response(200, result.toString());
                        CommLog.info("WebPayGetGoodsListController getGoodsList success, cid:{}", cid);
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        // 构造错误返回数据
                        Result result = ResultMgr.getInstance().lookupResult(_errCode);

                        JsonObject response = new JsonObject();
                        response.addProperty("code", _errCode);
                        response.addProperty("msg", result == null ? "Unknown error" : result.getMsg());
                        _response.response(200, response.toString());

                        CommLog.error("WebPayGetGoodsListController getGoodsList fail, cid:{}, errCode:{}", cid, _errCode);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_005_001_RetGetWebPayGoodsList();
                    }
                });

        }
        catch (NumberFormatException e)
        {
            _response.response(400, "Invalid parameter format!");
            CommLog.error("WebPayGetGoodsListController parameter format error, exception:{}", e);
        }
        catch (Exception e)
        {
            _response.response(500, "Internal server error!");
            CommLog.error("WebPayGetGoodsListController exception:{}", e);
        }
    }

    /**
     * 将商品列表转换为 JSON 数组
     *
     * @param _goodsList 商品列表数据
     * @return JSON 数组
     */
    private static JsonArray convertGoodsListToJson(ServerObj_WebPayGoodsList _goodsList)
    {
        JsonArray jsonArray = new JsonArray();

        if (_goodsList == null || _goodsList.getGoodsList() == null)
        {
            return jsonArray;
        }

        for (ServerObj_WebPayGoodsInfo goods : _goodsList.getGoodsList())
        {
            JsonObject goodsJson = new JsonObject();
            goodsJson.addProperty("product_id", goods.getGoodsId());
            goodsJson.addProperty("sdk_id", goods.getSdkPayId());
            goodsJson.addProperty("amount", goods.getAmount());

            // 限购信息
            if (goods.getLimit() != null && goods.getLimit().getMaxPurchase() > 0)
            {
                JsonObject limitJson = new JsonObject();
                limitJson.addProperty("max_purchase", goods.getLimit().getMaxPurchase());
                limitJson.addProperty("purchased", goods.getLimit().getPurchased());
                limitJson.addProperty("end_time", goods.getLimit().getEndTime());
                goodsJson.add("limit", limitJson);
            }
            else
            {
                // 非限购商品，limit 为空数组
                goodsJson.add("limit", new JsonArray());
            }

            // 物品信息对象（包含 base 和 gift）
            JsonObject itemObj = new JsonObject();

            // 基础物品列表
            JsonArray baseArray = new JsonArray();
            if (goods.getItem() != null)
            {
                for (NPCommon_ItemInfo item : goods.getItem())
                {
                    JsonObject itemJson = new JsonObject();
                    ENPItemType itemType = ENPItemType.ENPItemType_FromInt(item.getItemType());
                    if (itemType != null)
                    {
                        String lowerCase = itemType.toString().toLowerCase();
                        itemJson.addProperty("id", lowerCase + "-" + item.getSubId());
                        itemJson.addProperty("type", lowerCase);
                    }else
                    {
                        itemJson.addProperty("id", ("unknown-" + item.getSubId()).toLowerCase());
                        itemJson.addProperty("type", item.getItemType());
                    }
                    itemJson.addProperty("num", item.getCount());
                    baseArray.add(itemJson);
                }
            }
            itemObj.add("base", baseArray);

            // 赠品列表
            JsonArray giftArray = new JsonArray();
            if (goods.getGift() != null)
            {
                for (NPCommon_ItemInfo gift : goods.getGift())
                {
                    JsonObject giftJson = new JsonObject();
                    ENPItemType itemType = ENPItemType.ENPItemType_FromInt(gift.getItemType());
                    if (itemType != null)
                    {
                        String lowerCase = itemType.toString().toLowerCase();
                        giftJson.addProperty("id", lowerCase + "-" + gift.getSubId());
                        giftJson.addProperty("type", lowerCase);
                    }else
                    {
                        giftJson.addProperty("id", ("unknown-" + gift.getSubId()).toLowerCase());
                        giftJson.addProperty("type", gift.getItemType());
                    }
                    giftJson.addProperty("num", gift.getCount());
                    giftArray.add(giftJson);
                }
            }
            itemObj.add("gift", giftArray);

            goodsJson.add("item", itemObj);

            jsonArray.add(goodsJson);
        }

        return jsonArray;
    }

}
