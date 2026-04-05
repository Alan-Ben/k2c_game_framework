package NPHttpServer.Http.HttpService.MsgDispather.Http_005_Op;

import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpMainDealer;

/**
 * 活动查询协议族处理器（协议包5）
 *
 * 功能：管理活动查询相关的HTTP协议
 */
public class NP2HS_005_PlatFromMainDealer extends _ANPPlatFormHttpMainDealer
{
    public NP2HS_005_PlatFromMainDealer()
    {
        regist(new NP2HS_005_001_PlatFromAddActivitySchedule());
        regist(new NP2HS_005_002_PlatFromQueryActivityList());
        regist(new NP2HS_005_004_PlatFromQueryMultiServerActivity());
    }

    @Override
    public int mainOrder()
    {
        return 5;
    }
}
