package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityBanCidList;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 玩家邮件json字符串解析器
 * {
 * "cidList": [7010001,7010002,7010003],
 * "hours": 1,
 * "reason": 1,
 * "tagName": "AAa",
 * "reason_str": "xxx",
 * "operator": "xxx"
 * }
 * @author: ricci
 * @date: 2023-03-25 00:12:35
 */
public class NPPlatFormBanCidListDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityBanCidList>
{
    //////单例的//////
    private static final NPPlatFormBanCidListDecoder _s_instance = new NPPlatFormBanCidListDecoder();

    public static NPPlatFormBanCidListDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormBanCidListDecoder()
    {
    }

    @Override
    public NPEntityBanCidList decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityBanCidList obj = new NPEntityBanCidList();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            JsonArray cidList = JsonUtil.getJsonArray(jsonObject, "cidList", new JsonArray());
            for (JsonElement cidElement : cidList)
            {
                obj.addBanCid(cidElement.getAsLong());
            }

            obj.setHours(JsonUtil.getInt(jsonObject, "hours", 0));
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
