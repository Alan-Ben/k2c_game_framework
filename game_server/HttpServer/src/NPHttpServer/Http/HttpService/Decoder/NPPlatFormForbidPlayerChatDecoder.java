package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPEnum.ENPChatRoomType;
import NPHttpServer.Http.Entity.NPEntityForbidPlayerChat;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 玩家禁言json字符串解析器
 * {
 * "cidList": [7010001,7010002,7010003],
 * "hours": 1,
 * "channel": 1
 * }
 * @author: mark
 */
public class NPPlatFormForbidPlayerChatDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityForbidPlayerChat>
{
    //////单例的//////
    private static final NPPlatFormForbidPlayerChatDecoder _s_instance = new NPPlatFormForbidPlayerChatDecoder();

    public static NPPlatFormForbidPlayerChatDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormForbidPlayerChatDecoder()
    {
    }

    @Override
    public NPEntityForbidPlayerChat decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityForbidPlayerChat obj = new NPEntityForbidPlayerChat();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            JsonArray cidList = JsonUtil.getJsonArray(jsonObject, "cidList", new JsonArray());
            for (JsonElement cidElement : cidList)
            {
                obj.addForbidCid(cidElement.getAsLong());
            }

            obj.setHours(JsonUtil.getInt(jsonObject, "hours", 0));

            ENPChatRoomType roomType = ENPChatRoomType.NONE;
            if(jsonObject.has("channel"))
            {
                roomType = ENPChatRoomType.ENPChatRoomType_FromInt(JsonUtil.getInt(jsonObject, "channel", 0));
            }
            if(null == roomType)
            {
                CommLog.error("NPEntityForbidPlayerChat Parse ChatRoomType Error, data:{}", _data);
                return null;
            }
            obj.setChatRoomType(roomType);
        }
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
