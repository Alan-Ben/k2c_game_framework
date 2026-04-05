package NPHttpServer.Http.HttpService.Decoder;

import Common.ScheduleObj.Schedule_ResUpdateInfo;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPCommon.Util.Schedule.ResUpdateInfoJsonParser;

/**
 * 后台推送资源更新信息解码器
 *
 * 功能：
 * 1. 解析后台推送的资源更新 JSON 数据
 * 2. 使用 ResUpdateInfoJsonParser 进行格式校验和数据解析
 * 3. 返回解析后的 Schedule_ResUpdateInfo 对象
 */
public class PlatFormPushResUpdateInfoDecoder extends _ANPPlatFormHttpDataDecoder<Schedule_ResUpdateInfo>
{
    // 单例实例
    private static final PlatFormPushResUpdateInfoDecoder _s_instance = new PlatFormPushResUpdateInfoDecoder();

    /**
     * 获取单例实例
     *
     * @return 解码器单例
     */
    public static PlatFormPushResUpdateInfoDecoder getInstance()
    {
        return _s_instance;
    }

    /**
     * 解码后台推送的资源更新 JSON 数据
     *
     * @param _data JSON 字符串
     * @return 解析成功返回 Schedule_ResUpdateInfo 对象，失败返回 null
     */
    @Override
    public Schedule_ResUpdateInfo decode(String _data)
    {
        // 使用 ResUpdateInfoJsonParser 进行解析和校验
        ResultOne<Schedule_ResUpdateInfo> result = ResUpdateInfoJsonParser.validateAndParseResUpdateJson(_data);

        if (!result.isSucc())
        {
            CommLog.error("PlatFormPushResUpdateInfoDecoder.decode - decode failed: validation or parse error, data={}, errorCode={}",
                         _data, result.getResult().getCode());
            return null;
        }

        return result.getData();
    }
}
