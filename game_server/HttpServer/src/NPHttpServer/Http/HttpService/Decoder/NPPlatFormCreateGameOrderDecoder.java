package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityCreateGameOrder;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * 游戏下单请求解析器
 * {
 *   "cid": "50010001",
 *   "product_ids": "30118",
 *   "channel_code": "soha",
 *   "reason": "111"
 * }
 */
public class NPPlatFormCreateGameOrderDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityCreateGameOrder>
{
    private static final NPPlatFormCreateGameOrderDecoder _s_instance = new NPPlatFormCreateGameOrderDecoder();

    public static NPPlatFormCreateGameOrderDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormCreateGameOrderDecoder() {}

    @Override
    public NPEntityCreateGameOrder decode(String _data)
    {
        NPEntityCreateGameOrder obj = new NPEntityCreateGameOrder();
        try
        {
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            // cid以string传入，转long
            String cidStr = JsonUtil.getString(jsonObject, "cid", "");
            if (cidStr.isEmpty())
                return null;
            obj.setCid(Long.parseLong(cidStr));

            // product_ids为逗号分隔字符串，拆分成列表
            String productIdsStr = JsonUtil.getString(jsonObject, "product_ids", "");
            if (productIdsStr.isEmpty())
                return null;
            for (String id : productIdsStr.split(","))
            {
                String trimmed = id.trim();
                if (!trimmed.isEmpty())
                    obj.addProductId(trimmed);
            }

            obj.setChannelCode(JsonUtil.getString(jsonObject, "channel_code", ""));
            obj.setReason(JsonUtil.getString(jsonObject, "reason", ""));
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
