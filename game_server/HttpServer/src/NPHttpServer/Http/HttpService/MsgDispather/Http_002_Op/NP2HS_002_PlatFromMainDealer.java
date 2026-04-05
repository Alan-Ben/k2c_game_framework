package NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op;

import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpMainDealer;

/**
 * @description: 002协议族处理
 * @author: ricci
 * @date: 2023-03-25 15:17:24
 */
public class NP2HS_002_PlatFromMainDealer extends _ANPPlatFormHttpMainDealer
{
    public NP2HS_002_PlatFromMainDealer()
    {
        regist(new NP2HS_002_001_PlatFromServerMail());
        regist(new NP2HS_002_002_PlatFromUserMail());
        regist(new NP2HS_002_004_PlatFromServerMailDel());
    }

    @Override
    public int mainOrder()
    {
        return 2;
    }
}
