package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_013_RetUnBanUid;
import NPHttpServer.Http.Entity.NPEntityBanUidList;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormUnBanUidDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2CS.Msg.NP2CS_Writer_002_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_003_015_PlatFromUnBanUid extends _ANPPlatFormHttpSubDealer<NPEntityBanUidList>
{
    @Override
    public int subOrder()
    {
        return 15;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityBanUidList> getDecoder()
    {
        return NPPlatFormUnBanUidDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityBanUidList _decodeObj)
    {
        for (String uid : _decodeObj.getUidSet())
        {
            NPHttpServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal()
                    , NP2CS_Writer_002_BasicOp.make_013_ReqUnBanUid(uid)
                    , new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2CS_RB_002_013_RetUnBanUid();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure _ret)
                        {
                            //NP2CS_RB_002_013_RetUnBanUid retProto = (NP2CS_RB_002_013_RetUnBanUid) _ret;
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
