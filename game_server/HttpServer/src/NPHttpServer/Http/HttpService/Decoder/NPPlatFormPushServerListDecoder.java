package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.NP_SYS_ServerItem;
import NPHttpServer.Http.Entity.NPEntityPushServerList;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 推送的服务器列表json字符串解析器
 * <p>
 * {
 * "serverTag": "1.0.0.17",
 * "serverList": [
 * {
 * "serverId": 2,
 * "usTypeId": 2,
 * "serverName": "欧洲2",
 * "onlineState": 0,
 * "showState": 0,
 * "startDate": "2020-09-25",
 * "test": ""
 * },
 * {
 * "serverId": 3,
 * "usTypeId": 3,
 * "serverName": "欧洲3",
 * "onlineState": 0,
 * "showState": 0,
 * "startDate": "2020-09-24",
 * "test": ""
 * }
 * ]
 * }
 * @author: ricci
 * @date: 2023-03-25 00:12:35
 */
public class NPPlatFormPushServerListDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityPushServerList>
{
    //////单例的//////
    private static final NPPlatFormPushServerListDecoder _s_instance = new NPPlatFormPushServerListDecoder();

    public static NPPlatFormPushServerListDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormPushServerListDecoder()
    {
    }

    @Override
    public NPEntityPushServerList decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityPushServerList obj = new NPEntityPushServerList();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            obj.setServerTag(jsonObject.get("serverTag").getAsString());
            for (JsonElement serverList : jsonObject.get("serverList").getAsJsonArray())
            {
                JsonObject innerJsonObj = serverList.getAsJsonObject();

                NP_SYS_ServerItem serverItem = new NP_SYS_ServerItem();
                //这两个字段暂时没有
                //serverItem.setAreaTag("");
                //serverItem.setGroupId(0);
                serverItem.setServerLogicId(innerJsonObj.get("serverId").getAsInt());
                serverItem.setServerTypeId(innerJsonObj.get("usTypeId").getAsInt());
                serverItem.setServerName(innerJsonObj.get("serverName").getAsString());
                serverItem.setOnlineStateTypeId(innerJsonObj.get("onlineState").getAsInt());
                serverItem.setShowStateTypeId(innerJsonObj.get("showState").getAsInt());
                //暂时没有是否新服
                //serverItem.setIsNew(false);
                serverItem.setStartDate(innerJsonObj.get("startDate").getAsString());
                //serverItem.setExt(innerJsonObj.get("test").getAsString());

                obj.addServerList(serverItem);
            }

        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
