package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityGMCommandServer;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 设置白名单json字符串解析器
 * {
 * "uid": "a0003"
 * }
 * @author: ricci
 * @date: 2023-03-25 00:12:35
 */
public class NPPlatFormGMCommandServerDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityGMCommandServer>
{
    //////单例的//////
    private static final NPPlatFormGMCommandServerDecoder _s_instance = new NPPlatFormGMCommandServerDecoder();

    public static NPPlatFormGMCommandServerDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormGMCommandServerDecoder()
    {
    }

    @Override
    public NPEntityGMCommandServer decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityGMCommandServer obj = new NPEntityGMCommandServer();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            JsonArray usTypeIdList = JsonUtil.getJsonArray(jsonObject, "usTypeIdList", new JsonArray());
            for (JsonElement serverIdElement : usTypeIdList)
            {
                obj.addServerId(serverIdElement.getAsInt());
            }

            obj.setCommand(JsonUtil.getString(jsonObject, "command", ""));
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
