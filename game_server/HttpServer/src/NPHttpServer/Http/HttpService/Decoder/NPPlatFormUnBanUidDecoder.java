package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityBanUidList;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 玩家邮件json字符串解析器
 * {
 * "uid": "2032010001",
 * "hours": 100
 * "reason": 1
 * }
 * @author: ricci
 * @date: 2023-03-25 00:12:35
 */
public class NPPlatFormUnBanUidDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityBanUidList>
{
    //////单例的//////
    private static final NPPlatFormUnBanUidDecoder _s_instance = new NPPlatFormUnBanUidDecoder();

    public static NPPlatFormUnBanUidDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormUnBanUidDecoder()
    {
    }

    @Override
    public NPEntityBanUidList decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityBanUidList obj = new NPEntityBanUidList();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            JsonArray cidList = JsonUtil.getJsonArray(jsonObject, "uidList", new JsonArray());
            for (JsonElement cidElement : cidList)
            {
                obj.addUid(cidElement.getAsString());
            }

        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
