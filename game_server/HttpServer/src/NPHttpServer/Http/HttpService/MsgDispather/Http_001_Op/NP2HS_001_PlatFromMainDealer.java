package NPHttpServer.Http.HttpService.MsgDispather.Http_001_Op;

import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpMainDealer;

/**
 * @description: 001协议族处理
 * @author: ricci
 * @date: 2023-03-25 00:02:55
 */
public class NP2HS_001_PlatFromMainDealer extends _ANPPlatFormHttpMainDealer
{

    public NP2HS_001_PlatFromMainDealer()
    {
        regist(new NP2HS_001_001_PlatFromPushServerList());
        regist(new NP2HS_001_002_PlatFromPushParamList());
    }

    @Override
    public int mainOrder()
    {
        return 1;
    }
}
