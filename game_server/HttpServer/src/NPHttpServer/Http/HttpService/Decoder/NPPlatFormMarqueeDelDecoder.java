package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityMarqueeDel;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * 删除跑马灯
 * 
 * *************************
{
    "phpId": 1001,
    "usIdList": [
        1
    ]
}
 * **************************************
 */
public class NPPlatFormMarqueeDelDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityMarqueeDel>
{
    //////单例的//////
    private static final NPPlatFormMarqueeDelDecoder _s_instance = new NPPlatFormMarqueeDelDecoder();

    public static NPPlatFormMarqueeDelDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormMarqueeDelDecoder()
    {
    }

    @Override
    public NPEntityMarqueeDel decode(String _data)
    {
        //需要解析成的对象数据
    	NPEntityMarqueeDel obj = new NPEntityMarqueeDel();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            //*必填，PHP后台跑马灯数据ID
            long phpId = JsonUtil.getLong(jsonObject, "phpId");
            if(phpId <= 0)
            {
            	CommLog.error("NPEntityMarqueeDel phpId error, str:{}", _data);
            	return null;
            }
            obj.setPHPId(phpId);

            //必填，US列表
            JsonArray usIdList = JsonUtil.getJsonArray(jsonObject, "usIdList", new JsonArray());
            for (JsonElement usIdItem : usIdList)
            {
                obj.addUsIdList(usIdItem.getAsInt());
            }
            if(obj.getUsIdList().isEmpty())
            {
            	CommLog.error("NPEntityMarqueeDel usIdList error, str:{}", _data);
            	return null;
            }
            
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
