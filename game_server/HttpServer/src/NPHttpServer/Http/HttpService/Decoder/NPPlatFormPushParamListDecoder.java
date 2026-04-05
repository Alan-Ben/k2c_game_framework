package NPHttpServer.Http.HttpService.Decoder;

import Common.ServerObj.ServerObj_PHPParam;
import CommonEnum.EPlatParamType;
import NPHttpServer.Http.Entity.NPEntityPHPParamList;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * 平台参数字符串解析器

[
    {
        "key": "CLIENT_VERSION",
        "value": "1.0.0.1"
    },
    {
        "key": "HOT_REF_URL_BASE",
        "value": "http://10.0.0.33/cdn/hot_refdata/"
    }
]

 */
public class NPPlatFormPushParamListDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityPHPParamList>
{
    //////单例的//////
    private static final NPPlatFormPushParamListDecoder _s_instance = new NPPlatFormPushParamListDecoder();

    public static NPPlatFormPushParamListDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormPushParamListDecoder()
    {
    }

    @Override
    public NPEntityPHPParamList decode(String _data)
    {
        //需要解析成的对象数据
    	NPEntityPHPParamList obj = new NPEntityPHPParamList();
        try
        {
            //解析字符串
        	JsonArray jsonArr = new JsonParser().parse(_data).getAsJsonArray();

            for (JsonElement param : jsonArr)
            {
            	JsonObject paramObj = param.getAsJsonObject();
            	
            	String pKey = paramObj.get("key").getAsString();
            	String pValue = paramObj.get("value").getAsString();
            	
                obj.getPHPParamListObj().addPList(new ServerObj_PHPParam(EPlatParamType.valueOf(pKey), pValue));
            }
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        
        return obj;
    }
}
