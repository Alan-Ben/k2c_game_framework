package NPHttpServer.Http.HttpService.Decoder;

import Common.ServerObj.ServerObj_PHPMarqueeContent;
import CommonEnum.EMarqueeCanDelType;
import NPCommon.Log.CommLog;
import NPCommon.Util.JsonUtil;
import NPEnum.EMarqueeOfflineNeedShowType;
import NPHttpServer.Http.Entity.NPEntityMarqueeAdd;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * 下发跑马灯
 * 
 * *************************
{
    "phpId": 1001,
    "usIdList": [
        1
    ],
    "channelList": [
        "channel1",
        "channel2"
    ],
    "marqueeId": 0,
    "showPos": 1,
    "priority": 1,
    "uiResId": 4001,
    "durationSec": 60,
    "durationCount": 3,
    "lifeSec": 30,
    "canDelType": "READ_REF",
    "offlineNeedShowType": "READ_REF",
    "defaultLang": "lang1",
    "info": [
        {
            "lang": "lang1",
            "params": [
                "param1",
                "param2"
            ],
            "content": "content"
        },
        {
            "lang": "lang2",
            "params": [
                "param1",
                "param2"
            ],
            "content": "content"
        }
    ]
}
 * **************************************
 */
public class NPPlatFormMarqueeAddDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityMarqueeAdd>
{
    //////单例的//////
    private static final NPPlatFormMarqueeAddDecoder _s_instance = new NPPlatFormMarqueeAddDecoder();

    public static NPPlatFormMarqueeAddDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormMarqueeAddDecoder()
    {
    }

    @Override
    public NPEntityMarqueeAdd decode(String _data)
    {
        //需要解析成的对象数据
    	NPEntityMarqueeAdd obj = new NPEntityMarqueeAdd();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            //*必填，PHP后台跑马灯数据ID
            long phpId = JsonUtil.getLong(jsonObject, "phpId");
            if(phpId <= 0)
            {
            	CommLog.error("NPEntityMarquee phpId error, str:{}", _data);
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
            	CommLog.error("NPEntityMarquee usIdList error, str:{}", _data);
            	return null;
            }
            
            //选填，渠道列表
            JsonArray channelList = JsonUtil.getJsonArray(jsonObject, "channelList", new JsonArray());
            for (JsonElement channelItem : channelList)
            {
                obj.addChannelList(channelItem.getAsString());
            }
            
            //选填，跑马灯配置ID
            obj.setMarqueeId(JsonUtil.getLong(jsonObject, "marqueeId", 0));
            
            //选填，窗口展示位置
            obj.setShowPos(JsonUtil.getInt(jsonObject, "showPos", 0));
            
            //选填，优先级（越大越优先）
            obj.setPriority(JsonUtil.getInt(jsonObject, "priority", 0));
            
            //选填，预制体ID（展示使用）
            obj.setUiResId(JsonUtil.getLong(jsonObject, "uiResId", 0));
            
            //选填，循环播放时长秒（优先于次数）
            obj.setDurationSec(JsonUtil.getInt(jsonObject, "durationSec", 0));
            
            //选填，循环播放次数
            obj.setDurationCount(JsonUtil.getInt(jsonObject, "durationCount", 0));
            
            //选填，生存时间秒
            obj.setLifeSec(JsonUtil.getInt(jsonObject, "lifeSec", 0));

        	//选填，跑马灯是否可删除类型，字符串
            String canDelType = JsonUtil.getString(jsonObject, "canDelType", EMarqueeCanDelType.READ_REF.toString()).toUpperCase();
            obj.setCanDelType(EMarqueeCanDelType.valueOf(canDelType));
        	
        	//选填，玩家离线期间是否需要展示
            String offlineNeedShowType = JsonUtil.getString(jsonObject, "offlineNeedShowType", EMarqueeOfflineNeedShowType.READ_REF.toString()).toUpperCase();
            obj.setOfflineNeedShowType(EMarqueeOfflineNeedShowType.valueOf(offlineNeedShowType));
        	
        	//选填，默认语言（不填写则使用跑马灯信息列表第1条），使用字符串
            obj.setDefaultLang(JsonUtil.getString(jsonObject, "defaultLang"));
        	
        	//选填，跑马灯信息，包括语言，参数列表，内容
            JsonArray infoList = JsonUtil.getJsonArray(jsonObject, "info", new JsonArray());
            for (JsonElement infoItem : infoList)
            {
            	JsonObject infoObj = infoItem.getAsJsonObject();
            	
            	ServerObj_PHPMarqueeContent contentObj = new ServerObj_PHPMarqueeContent();
            	contentObj.setLang(infoObj.get("lang").getAsString());
            	contentObj.setContent(infoObj.get("content").getAsString());
            	
            	JsonArray paramList = infoObj.get("params").getAsJsonArray();
            	for(JsonElement paramItem : paramList)
            	{
            		contentObj.addParamList(paramItem.getAsString());
            	}
            	
            	obj.addLangContentList(contentObj);
            }

        	//选填，展示条件（仅客户端使用）
            obj.setShowCondition(JsonUtil.getString(jsonObject, "showConditon"));
            
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    }
}
