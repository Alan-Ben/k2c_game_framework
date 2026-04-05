package NPHttpServer.Http.HttpService.MsgDispather.Http_009_Op;

import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpMainDealer;

/**
 * @description: 002协议族处理
 * @author: ricci
 * @date: 2023-03-25 15:17:24
 */
public class NP2HS_009_PlatFromMainDealer extends _ANPPlatFormHttpMainDealer
{
    public NP2HS_009_PlatFromMainDealer()
    {
        regist(new NP2HS_009_003_PlatFromGetPlayerInfo());
    }

    @Override
    public int mainOrder()
    {
        return 9;
    }
}
