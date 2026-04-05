package NPHttpServer.Http.HttpService.MsgDispather.Http_007_Op;

import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpMainDealer;

/**
 * @description: 007协议族处理
 */
public class NP2HS_007_PlatFromMainDealer extends _ANPPlatFormHttpMainDealer
{
    public NP2HS_007_PlatFromMainDealer()
    {
        regist(new NP2HS_007_002_PlatFromPushActivitySchedule());
        regist(new NP2HS_007_004_PlatFromPushResUpdateInfo());
    }

    @Override
    public int mainOrder()
    {
        return 7;
    }
}
