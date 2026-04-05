package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_012_RetBanUid;
import NPHttpServer.Http.Entity.NPEntityBanUidList;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormBanUidDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2CS.Msg.NP2CS_Writer_002_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_003_014_PlatFromBanUid extends _ANPPlatFormHttpSubDealer<NPEntityBanUidList>
{
    @Override
    public int subOrder()
    {
        return 14;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityBanUidList> getDecoder()
    {
        return NPPlatFormBanUidDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityBanUidList _decodeObj)
    {
        for (String uid : _decodeObj.getUidSet())
        {
            NPHttpServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.COMMON.ordinal()
                    , NP2CS_Writer_002_BasicOp.make_012_ReqBanUid(uid, _decodeObj.getTimeMs())
                    , new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2CS_RB_002_012_RetBanUid();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _ret)
                        {
                            //NP2CS_RB_002_012_RetBanUid retProto = (NP2CS_RB_002_012_RetBanUid) _ret;
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {

                        }
                    });
        }

        _commiter.commitSuc();
    }
}
