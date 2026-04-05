package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityGetPlayerByCid;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * @description: 获取玩家详细信息
 * 

us_type_id	服务器号	是	int	1
cid	角色CID	否：CID&Name 二选1	string	精确查找
name	用户名	否：CID&Name 二选1	string	精确查找
 
 */
public class NPPlatFormGetPlayerByCidDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityGetPlayerByCid>
{
    //////单例的//////
    private static final NPPlatFormGetPlayerByCidDecoder _s_instance = new NPPlatFormGetPlayerByCidDecoder();

    public static NPPlatFormGetPlayerByCidDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormGetPlayerByCidDecoder()
    {
    }

    @Override
    public NPEntityGetPlayerByCid decode(String _data)
    {
        //需要解析成的对象数据
    	NPEntityGetPlayerByCid obj = new NPEntityGetPlayerByCid();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            //US服务器ID解析
            obj.setUsId(JsonUtil.getInt(jsonObject, "us_type_id", 0));
            if(obj.getUsId() <= 0)
            {
            	CommLog.error("NPEntityGetPlayerByCid Parse UsTypeId Error, data:{}", _data);
            	return null;
            }
            
            //参数解析
            obj.setCid(JsonUtil.getLong(jsonObject, "cid", 0));
            obj.setName(JsonUtil.getString(jsonObject, "name", ""));
        } 
        catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
