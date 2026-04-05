package NPHttpServer.Http.HttpService.MsgDispather.Http_009_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2US_R.p001_BasicOp.NP2US_R_001_010_PHPGetPlayerInfo;
import NP2US_RB.p001_BasicOp.NP2US_RB_001_010_PHPGetPlayerInfo;
import NPCommon.ErrMain.Result.ResultMgr;
import NPHttpServer.Http.Entity.NPEntityGetPlayerByCid;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormGetPlayerByCidDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

public class NP2HS_009_003_PlatFromGetPlayerInfo extends _ANPPlatFormHttpSubDealer<NPEntityGetPlayerByCid>
{
    @Override
    public int subOrder()
    {
        return 3;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityGetPlayerByCid> getDecoder()
    {
        return NPPlatFormGetPlayerByCidDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityGetPlayerByCid _decodeObj)
    {
    	NP2US_R_001_010_PHPGetPlayerInfo proto = new NP2US_R_001_010_PHPGetPlayerInfo();
    	proto.setCid(_decodeObj.getCid());
    	proto.setName(_decodeObj.getName());
    	
    	NPHttpServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _decodeObj.getUsId(), proto
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public void dealSuc(_IALProtocolStructure arg0)
                    {
                    	NP2US_RB_001_010_PHPGetPlayerInfo ret = (NP2US_RB_001_010_PHPGetPlayerInfo) arg0;
                    	
                        _commiter.commitSuc("", NPEntityGetPlayerByCid.toJson(ret.getPlayer()).toString());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _commiter.commitFail(ResultMgr.getInstance().lookupResult(_errCode), "not find us, cid:" + _decodeObj.getCid());
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_001_010_PHPGetPlayerInfo();
                    }
                });
    }
}
