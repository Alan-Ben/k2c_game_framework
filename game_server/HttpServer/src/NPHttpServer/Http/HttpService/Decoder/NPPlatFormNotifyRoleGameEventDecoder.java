package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Log.CommLog;
import NPHttpServer.Http.Entity.NPEntityNotifyRoleGameEvent;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

public class NPPlatFormNotifyRoleGameEventDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityNotifyRoleGameEvent>
{
    //////单例的//////
    private static final NPPlatFormNotifyRoleGameEventDecoder _s_instance = new NPPlatFormNotifyRoleGameEventDecoder();

    public static NPPlatFormNotifyRoleGameEventDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormNotifyRoleGameEventDecoder()
    {
    }

    @Override
    public NPEntityNotifyRoleGameEvent decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityNotifyRoleGameEvent obj = new NPEntityNotifyRoleGameEvent();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            obj.setActivityCode(jsonObject.get("activityCode").getAsString());
            obj.setCid(jsonObject.get("cid").getAsLong());
            obj.setType(jsonObject.get("type").getAsString());
        } catch (Exception e)
        {
            CommLog.error("NPPlatFormNotifyRoleGameEventDecoder decode fail, data:{}", _data, e);
            return null;
        }
        return obj;
    }
}
