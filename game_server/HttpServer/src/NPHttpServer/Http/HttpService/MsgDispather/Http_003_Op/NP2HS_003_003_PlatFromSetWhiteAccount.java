package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import NPCommon.ErrMain.CommErr;
import NPHttpServer.Http.Entity.NPEntityWhiteAccount;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormWhiteAccountDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPWhiteAccMgr.NPWhiteAccMgr;

public class NP2HS_003_003_PlatFromSetWhiteAccount extends _ANPPlatFormHttpSubDealer<NPEntityWhiteAccount>
{
    @Override
    public int subOrder()
    {
        return 3;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityWhiteAccount> getDecoder()
    {
        return NPPlatFormWhiteAccountDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityWhiteAccount _decodeObj)
    {
        boolean isSuc = NPWhiteAccMgr.getInstance().addAcc(_decodeObj.getUid());
        if (isSuc)
        {
            _commiter.commitSuc();
        } else
        {
            _commiter.commitFail(CommErr.SYS_ERR, "white list account add fail!");
        }
    }
}
