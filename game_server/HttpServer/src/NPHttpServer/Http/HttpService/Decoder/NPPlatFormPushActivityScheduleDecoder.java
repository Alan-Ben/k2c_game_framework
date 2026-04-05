package NPHttpServer.Http.HttpService.Decoder;

import NPCommon.Util.JsonUtil;
import NPHttpServer.Http.Entity.NPEntityPushActivitySchedule;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

/**
 * 推送的活动排期配表json字符串解析器
 * @author mj
 * 
 * 特别说明
   1. 运营后台推送文件名
   2. HS收到文件即通报运营平台成功
   3. HS将文件名转到SS
   4. SS下载文件并检查
   5. 检查成功，再次通报运营平台该活动排期生效
 *
 * 接口数据（json格式）：
  {
   "opSerial": 20240815000000001,
   "fileName": "activity_schedule_test.txt"
  }
 *
 * 文件案例
运营后台传递内容：排期文件名
例子：activity_schedule_test.txt
下载 activity_schedule_test.txt 的内容案例：
 {
  "serial": 20240815000000001,
  "zoneId": 1001,
  "scheduleList": [
    {
      "scheduleId": 1001,
      "prePushTime": "2024-06-22 15:00:00",
      "activity": {
        "activityId": 101,
        "startTime": "2024-06-24 00:00:00",
        "endTime": "2024-06-24 00:00:00",
        "closeTime": "2024-06-25 00:00:00"
      },
      "usIdList": [],
      "usGroupList": [
        {
          "groupId": 1,
          "usIdList": [1]
        }
      ]
    }
  ]
}
 *
 */
public class NPPlatFormPushActivityScheduleDecoder extends _ANPPlatFormHttpDataDecoder<NPEntityPushActivitySchedule>
{
    //////单例的//////
    private static final NPPlatFormPushActivityScheduleDecoder _s_instance = new NPPlatFormPushActivityScheduleDecoder();

    public static NPPlatFormPushActivityScheduleDecoder getInstance()
    {
        return _s_instance;
    }

    private NPPlatFormPushActivityScheduleDecoder()
    {
    }

    @Override
    public NPEntityPushActivitySchedule decode(String _data)
    {
    	if(null == _data || _data.isEmpty())
    	{
    		return null;
    	}
    	
        //需要解析成的对象数据
    	NPEntityPushActivitySchedule obj = new NPEntityPushActivitySchedule();
        try
        {
            //解析字符串
            JsonObject jsonObject = new JsonParser().parse(_data).getAsJsonObject();

            //后台操作序列号
            obj.setPHPOpSerial(JsonUtil.getLong(jsonObject, "opSerial"));
            //下载文件名
            obj.setFileName(JsonUtil.getString(jsonObject, "fileName"));
            
        } catch (Exception e)
        {
            e.printStackTrace();
            return null;
        }
        return obj;
    
    }
}
