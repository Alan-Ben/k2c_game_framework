package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpMainDealer;

/**
 * @description: 002协议族处理
 * @author: ricci
 * @date: 2023-03-25 15:17:24
 */
public class NP2HS_003_PlatFromMainDealer extends _ANPPlatFormHttpMainDealer
{
    public NP2HS_003_PlatFromMainDealer()
    {
        regist(new NP2HS_003_001_PlatFromPlayerGMCommand());
        regist(new NP2HS_003_002_PlatFromServerGMCommand());
        regist(new NP2HS_003_003_PlatFromSetWhiteAccount());
        regist(new NP2HS_003_004_PlatFromUnSetWhiteAccount());
        regist(new NP2HS_003_005_PlatFromBanCid());
        regist(new NP2HS_003_006_PlatFromUnBanCid());
        regist(new NP2HS_003_007_ForbidPlayerChat());
        regist(new NP2HS_003_008_LiftForbidPlayerChat());
        regist(new NP2HS_003_011_PlatFromCreateGameOrder());
        regist(new NP2HS_003_014_PlatFromBanUid());
        regist(new NP2HS_003_015_PlatFromUnBanUid());
        regist(new NP2HS_003_016_PlatFromMarquee());
        regist(new NP2HS_003_017_PlatFromMarqueeDel());
        regist(new NP2HS_003_024_PlatFromAnnouncementVersion());
    }

    @Override
    public int mainOrder()
    {
        return 3;
    }
}
