package PayCenter.Http.HttpContorller;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Http.HttpService.annotation.RequestMapping;
import NPCommon.Http.HttpService.server.HttpMethod;
import NPCommon.Http.HttpService.server.HttpRequest;
import NPCommon.Http.HttpService.server.HttpResponse;
import NPCommon.Log.CommLog;
import NPCommon.Util.OrderIdParser;
import PayCenter.Conf.PayCenterConf;
import PayCenter.PayCallback.PayCallbackMgr;
import WCGCommon.Security.MD5;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import java.util.Map;
import java.util.TreeMap;

public class PayCallbackController
{
    @RequestMapping(uri = "/game/pay/callback", method = HttpMethod.POST)
    public void callback(HttpRequest _request, final HttpResponse _response)
    {
        String postBody = _request.getPostBody();

        try
        {
            // 解析JSON数据
            JsonObject jsonObject = new JsonParser().parse(postBody).getAsJsonObject();
            // 验证签名
            if (!verifySign(jsonObject))
            {
                CommLog.error("PayCallbackController sign verification failed for callback: {}", postBody);
                respondError(_response, CommErr.SING_CHECK_FAIL);
                return;
            }

            // 验证订单号格式
            if (!OrderIdParser.isValidFormat(jsonObject.get("app_order_id").getAsString()))
            {
                CommLog.error("PayCallbackController invalid app_order_id format: {}", jsonObject.get("app_order_id").getAsString());
                respondError(_response, CommErr.PARAM_ERROR);
                return;
            }

            // 构造回调数据对象
            PayCallbackData callbackData;
            try
            {
                callbackData = new PayCallbackData(jsonObject);
            } catch (IllegalArgumentException e)
            {
                CommLog.error("PayCallbackController invalid callback data: {}, error: {}", postBody, e.getMessage());
                respondError(_response, CommErr.PARAM_ERROR);
                return;
            }

            CommLog.info("PayCallbackController processing order: {}", callbackData);

            // 根据订单类型处理
            Result result;
            if (callbackData.isPaymentSuccess())
            {
                // 普通订单付款成功
                result = PayCallbackMgr.getInstance().addPayCallbackInfo(callbackData);
            } else
            {
                CommLog.error("PayCallbackController unsupported order type: {}", callbackData.getType());
                respondError(_response, CommErr.PARAM_ERROR);
                return;
            }

            // 回包通知结果
            if (!result.isSucc())
            {
                respondError(_response, result);
                return;
            }

            respondSuccess(_response);

        } catch (Exception e)
        {
            CommLog.error("PayCallbackController callback /game/pay/callback error Exception:{}", e);
            respondError(_response, CommErr.SYS_ERR);
        }
    }

    /**
     * 验证签名
     * 签名规则: md5('MJ' + md5(paramsString + 密钥))
     * paramsString: 排除sign参数，按参数名升序排序后连接参数值
     */
    private boolean verifySign(JsonObject jsonObject)
    {
        try
        {
            // 获取传入的签名
            if (!jsonObject.has("sign"))
            {
                CommLog.error("PayCallbackController missing sign parameter");
                return false;
            }

            String receivedSign = jsonObject.get("sign").getAsString();

            // 构建参数Map并排序
            TreeMap<String, String> params = new TreeMap<>();

            for (Map.Entry<String, com.google.gson.JsonElement> entry : jsonObject.entrySet())
            {
                String key = entry.getKey();
                if (!"sign".equals(key)) // 排除sign参数
                {
                    String value;
                    if (entry.getValue().isJsonNull())
                    {
                        value = "";
                    } else if (entry.getValue().isJsonPrimitive())
                    {
                        // 对于数字类型，保持原始格式
                        if (entry.getValue().getAsJsonPrimitive().isNumber())
                        {
                            value = entry.getValue().getAsString();
                        } else
                        {
                            value = entry.getValue().getAsString();
                        }
                    } else if (entry.getValue().isJsonArray() || entry.getValue().isJsonObject())
                    {
                        // 对于数组或对象，使用JSON格式（按文档要求）
                        value = entry.getValue().toString();
                    } else
                    {
                        value = entry.getValue().getAsString();
                    }
                    params.put(key, value);
                }
            }

            // 连接所有参数值
            StringBuilder paramsString = new StringBuilder();
            for (String value : params.values())
            {
                paramsString.append(value);
            }

            // 生成签名: md5('MJ' + md5(paramsString + 密钥))
            String paySecret = PayCenterConf.getInstance().getPaySecret();
            if (paySecret.isEmpty())
            {
                CommLog.error("PayCallbackController PaySecret not configured");
                return false;
            }

            String step1 = MD5.md5Encryption(paramsString + paySecret);
            String calculatedSign = MD5.md5Encryption("MJ" + step1);

            return receivedSign.equals(calculatedSign);
        } catch (Exception e)
        {
            CommLog.error("PayCallbackController sign verification error: {}", e);
            return false;
        }
    }

    /**
     * 处理退款
     */
    private Result processRefund(PayCallbackData callbackData)
    {
        // TODO: 实现具体的退款逻辑
        // 1. 查找原订单记录
        // 2. 回收对应的游戏物品/货币
        // 3. 记录退款处理日志

        CommLog.sys("PayCallbackController processing refund: orderId={}, uid={}, roleId={}, refundSource={}, refundTime={}",
                callbackData.getOrderId(), callbackData.getUid(), callbackData.getRoleId(),
                callbackData.getRefundSource(), callbackData.getRefundTime());

        // 这里应该调用具体的退款业务逻辑
        // 暂时返回成功表示处理成功
        return Result.SUCC;
    }

    /**
     * 响应成功
     */
    private void respondSuccess(HttpResponse _response)
    {
        JsonObject response = new JsonObject();
        response.addProperty("code", 1);
        response.addProperty("msg", "");
        _response.response(200, response.toString());
    }

    /**
     * 响应错误
     */
    private void respondError(HttpResponse _response, Result _result)
    {
        JsonObject response = new JsonObject();
        response.addProperty("code", _result.getCode());
        response.addProperty("msg", _result.getMsg());
        _response.response(200, response.toString());
    }
}
