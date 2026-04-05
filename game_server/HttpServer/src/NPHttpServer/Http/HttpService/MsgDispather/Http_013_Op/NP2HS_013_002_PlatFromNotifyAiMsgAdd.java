package NPHttpServer.Http.HttpService.MsgDispather.Http_013_Op;

import NPCommon.Log.CommLog;
import NPHttpServer.Http.Entity.EntityNotifyAiMsgAdd;
import NPHttpServer.Http.HttpService.AutoDecoder.JsonDecoderFactory;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.HttpAiService.HttpAiService;

public class NP2HS_013_002_PlatFromNotifyAiMsgAdd extends _ANPPlatFormHttpSubDealer<EntityNotifyAiMsgAdd>
{
    @Override
    public int subOrder()
    {
        return 2;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<EntityNotifyAiMsgAdd> getDecoder()
    {
        return JsonDecoderFactory.getDecoder(EntityNotifyAiMsgAdd.class);
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, EntityNotifyAiMsgAdd _decodeObj)
    {
        long requestId = _decodeObj.getRequestId();
        int code = _decodeObj.getCode();

        if (code == 200)
        {
            HttpAiService.getInstance().dealResponse(requestId, 0, _decodeObj.getData().getContent());
        }else
        {
            HttpAiService.getInstance().dealResponse(requestId, code, "");
            CommLog.info("HSAiChatNotifyController pushServerInfoChg /aichat/notify error: requestId={}, errCode={}, errMsg={}", requestId, code, _decodeObj.getMessage());
        }

        _commiter.commitSuc();
    }
}
