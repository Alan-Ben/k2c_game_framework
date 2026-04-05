package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityGMCommandPlayer;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 发送玩家GM命令 json字符串解析器
 * {
 * "cidList": [
 * "5010999","6010998"
 * ],
 * "command": "quests setstep 50 9"
 * }
 * @author: ricci
 * @date: 2023-03-25 00:12:35
 */
public class NPPlatFormGMCommandPlayerDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityGMCommandPlayer>
{
    //////单例的//////
    private static final NPPlatFormGMCommandPlayerDecoder _s_instance = new NPPlatFormGMCommandPlayerDecoder();

    public static NPPlatFormGMCommandPlayerDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormGMCommandPlayerDecoder()
    {
    }

    @Override
    public NPEntityGMCommandPlayer decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityGMCommandPlayer obj = new NPEntityGMCommandPlayer();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            JsonArray cidList = JsonUtil.getJsonArray(jsonObject, "cidList", new JsonArray());
            for (JsonElement cidElement : cidList)
            {
                obj.addCid(cidElement.getAsLong());
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
