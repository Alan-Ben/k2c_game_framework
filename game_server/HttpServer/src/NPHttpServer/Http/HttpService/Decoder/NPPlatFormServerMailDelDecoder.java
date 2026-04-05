package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityServerMailDel;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 召回全服邮件
 * {
 * "usTypeIdList": [
 * 51,
 * 52,
 * 53
 * ],
 * phpMailId": 505
 * }
 */
public class NPPlatFormServerMailDelDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityServerMailDel>
{
    //////单例的//////
    private static final NPPlatFormServerMailDelDecoder _s_instance = new NPPlatFormServerMailDelDecoder();

    public static NPPlatFormServerMailDelDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormServerMailDelDecoder()
    {
    }

    @Override
    public NPEntityServerMailDel decode(String _data)
    {
        //需要解析成的对象数据
    	NPEntityServerMailDel obj = new NPEntityServerMailDel();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            
        	//邮件平台ID
            long phpMailId = JsonUtil.getLong(jsonObject, "phpMailId");
            if(phpMailId <= 0)
            {
            	CommLog.error("NPEntityServerMailRevoke phpMailId error, str:{}", _data);
            	return null;
            }
            obj.setPHPMailId(phpMailId);
        	
            //读取服务器列表
            for (JsonElement jsonElement : jsonObject.get("usTypeIdList").getAsJsonArray())
            {
                int usTypeId = jsonElement.getAsInt();
                obj.addUsTypeId(usTypeId);
            }
            
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
