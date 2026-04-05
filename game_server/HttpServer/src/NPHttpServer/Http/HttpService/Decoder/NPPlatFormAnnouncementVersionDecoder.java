package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityAnnouncementVersion;
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
public class NPPlatFormAnnouncementVersionDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityAnnouncementVersion>
{
    //////单例的//////
    private static final NPPlatFormAnnouncementVersionDecoder _s_instance = new NPPlatFormAnnouncementVersionDecoder();

    public static NPPlatFormAnnouncementVersionDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormAnnouncementVersionDecoder()
    {
    }

    @Override
    public NPEntityAnnouncementVersion decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityAnnouncementVersion obj = new NPEntityAnnouncementVersion();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            JsonArray usList = JsonUtil.getJsonArray(jsonObject, "usTypeIdList", new JsonArray());
            for (JsonElement usIdElement : usList)
            {
                obj.addUsId(usIdElement.getAsInt());
            }
            String version = JsonUtil.getString(jsonObject, "version", "");
            obj.setVersion(version);
        } catch (Exception e)
        {
            CommLog.error("", e);
            return null;
        }
        return obj;
    }
}
