package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_012_RetBanUid;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityBanCidList;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormBanCidListDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2US.Request.NP2US_R_Writer_003_CommOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_003_005_PlatFromBanCid extends _ANPPlatFormHttpSubDealer<NPEntityBanCidList>
{
    @Override
    public int subOrder()
    {
        return 5;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityBanCidList> getDecoder()
    {
        return NPPlatFormBanCidListDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityBanCidList _decodeObj)
    {
        long FreezeTimeMs = _decodeObj.getTimeMs();

        for (Long cid : _decodeObj.getBanCidList())
        {
            int usId = CommonFunc.parseServerTypeIdFromCid(cid);
            NPHttpServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), usId
                    , NP2US_R_Writer_003_CommOp.make_012_ReqBanCid(cid, FreezeTimeMs)
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
                            CommLog.error("NP2HS_003_005_PlatFromBanCid cid:{} errCode:{}", cid, _errCode);
                        }
                    });
        }

        _commiter.commitSuc();
    }
}
