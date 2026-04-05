package NPHttpServer.Http.HttpService.Decoder;

import NPHttpServer.Http.Entity.NPEntityWhiteAccount;
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
public class NPPlatFormWhiteAccountDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityWhiteAccount>
{
    //////单例的//////
    private static final NPPlatFormWhiteAccountDecoder _s_instance = new NPPlatFormWhiteAccountDecoder();

    public static NPPlatFormWhiteAccountDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormWhiteAccountDecoder()
    {
    }

    @Override
    public NPEntityWhiteAccount decode(String _data)
    {
        //需要解析成的对象数据
        NPEntityWhiteAccount obj = new NPEntityWhiteAccount();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();
            obj.setUid(jsonObject.get("uid").getAsString());
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
