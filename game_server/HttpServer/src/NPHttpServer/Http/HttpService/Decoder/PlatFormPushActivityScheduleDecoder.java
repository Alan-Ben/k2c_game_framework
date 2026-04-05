package NPHttpServer.Http.HttpService.Decoder;

import Common.ScheduleObj.Schedule_PhpInfo;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.Util.Schedule.ActivityScheduleJsonParser;

public class PlatFormPushActivityScheduleDecoder extends _ANPPlatFormHttpDataDecoder<Schedule_PhpInfo>
{
    private static final PlatFormPushActivityScheduleDecoder _s_instance = new PlatFormPushActivityScheduleDecoder();

    public static PlatFormPushActivityScheduleDecoder getInstance()
    {
        return _s_instance;
    }

    @Override
    public Schedule_PhpInfo decode(String _data)
    {
        ResultOne<Schedule_PhpInfo> result = ActivityScheduleJsonParser.validateAndParseScheduleJson(_data);
        if (!result.isSucc())
        {
            CommLog.error("PlatFormPushActivitySchedule decode failed! data:{} error:{}", _data, result.getResult().getCode());
            return null;
        }
        return result.getData();
    }
}
