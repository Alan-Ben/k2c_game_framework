package NPHttpServer.Http.HttpService.MsgDispather.Http_007_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ScheduleObj.Schedule_ResUpdateInfo;
import NP2SS_R.p001_ScheduleOp.ToSS_R_001_017_CmdPushResUpdateInfo;
import NP2SS_RB.p001_ScheduleOp.ToSS_RB_001_017_CmdPushResUpdateInfo;
import NPHttpServer.Http.HttpService.Decoder.PlatFormPushResUpdateInfoDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 后台推送资源更新信息处理器
 *
 * 功能：
 * 1. 接收后台推送的资源更新配置信息
 * 2. 解析 JSON 数据为 Schedule_ResUpdateInfo 对象
 * 3. 转发到 ScheduleServer 进行处理
 */
public class NP2HS_007_004_PlatFromPushResUpdateInfo extends _ANPPlatFormHttpSubDealer<Schedule_ResUpdateInfo>
{

    /**
     * 子协议号
     *
     * @return 协议号 4
     */
    @Override
    public int subOrder()
    {
        return 4;
    }

    /**
     * 获取解码器
     *
     * @return 资源更新信息解码器
     */
    @Override
    public _ANPPlatFormHttpDataDecoder<Schedule_ResUpdateInfo> getDecoder()
    {
        return PlatFormPushResUpdateInfoDecoder.getInstance();
    }

    /**
     * 处理消息 - 转发到 ScheduleServer
     *
     * @param _commiter  HTTP 响应提交器
     * @param _decodeObj 解码后的资源更新信息对象
     */
    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, Schedule_ResUpdateInfo _decodeObj)
    {
        // 发送到 ScheduleServer
        NPHttpServer.getInstance().sendRequestToBSServer(
            EServerType.SINGLE.ordinal(),
            ENPSingleServerType.SCHEDULE.ordinal(),
            new ToSS_R_001_017_CmdPushResUpdateInfo(_decodeObj),
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new ToSS_RB_001_017_CmdPushResUpdateInfo();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retMsg)
                {
                    _commiter.commitSuc();
                }

                @Override
                public void dealFail(int _errCode)
                {
                    _commiter.commitFail(_errCode);
                }
            });
    }
}
