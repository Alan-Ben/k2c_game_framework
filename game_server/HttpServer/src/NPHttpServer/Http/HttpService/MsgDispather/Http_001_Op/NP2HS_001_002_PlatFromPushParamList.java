package NPHttpServer.Http.HttpService.MsgDispather.Http_001_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_014_RetUpdatePHPParamList;
import NPHttpServer.Http.Entity.NPEntityPHPParamList;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormPushParamListDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_001_002_PlatFromPushParamList extends _ANPPlatFormHttpSubDealer<NPEntityPHPParamList>
{
    @Override
    public int subOrder()
    {
        return 2;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityPHPParamList> getDecoder()
    {
        return NPPlatFormPushParamListDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityPHPParamList _decodeObj)
    {
        //将变更推送到CS
        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_014_ReqUpdatePHPParamList(NPEntityPHPParamList.buildDataSerial(), _commiter.getPHPReqSerial(), _decodeObj.getPHPParamListObj()), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_014_RetUpdatePHPParamList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        _commiter.commitSuc();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _commiter.commitFail(_errCode);
                    }
                });
    }
}
