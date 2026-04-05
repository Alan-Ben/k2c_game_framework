package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import NP2US.p002_UserOp.NP2US_002_006_OnAnnouncementVersionChg;
import NPHttpServer.Http.Entity.NPEntityAnnouncementVersion;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormAnnouncementVersionDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_003_024_PlatFromAnnouncementVersion extends _ANPPlatFormHttpSubDealer<NPEntityAnnouncementVersion>
{
    @Override
    public int subOrder()
    {
        return 24;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityAnnouncementVersion> getDecoder()
    {
        return NPPlatFormAnnouncementVersionDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityAnnouncementVersion _decodeObj)
    {
        NP2US_002_006_OnAnnouncementVersionChg proto = new NP2US_002_006_OnAnnouncementVersionChg(_decodeObj.getVersion());

        for (Integer usId : _decodeObj.getUsIdList())
        {
            NPHttpServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), usId, proto);
        }

        _commiter.commitSuc();
    }
}
