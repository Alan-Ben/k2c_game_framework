package NPHttpServer.Http.HttpService.MsgDispather.Http_013_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityNotifyRoleGameEvent;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormNotifyRoleGameEventDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_016_ReqNotifyRoleGameEvent;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_016_RetNotifyRoleGameEvent;
import WCGCommon.Enum.NPEnum.EServerType;

public class NP2HS_013_001_PlatFromNotifyRoleGameEvent extends _ANPPlatFormHttpSubDealer<NPEntityNotifyRoleGameEvent>
{
    @Override
    public int subOrder()
    {
        return 1;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityNotifyRoleGameEvent> getDecoder()
    {
        return NPPlatFormNotifyRoleGameEventDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityNotifyRoleGameEvent _decodeObj)
    {
        long cid = _decodeObj.getCid();
        int usId = CommonFunc.parseServerTypeIdFromCid(cid);

        //发送US
        NP2US_R_003_016_ReqNotifyRoleGameEvent proto = new NP2US_R_003_016_ReqNotifyRoleGameEvent();
        proto.setCid(cid);
        proto.setType(_decodeObj.getType());
        proto.setActivityCode(_decodeObj.getActivityCode());

        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), usId, proto
                , new _IWCGCallbackDealer()
                {

                    @Override
                    public void dealSuc(_IALProtocolStructure arg0)
                    {
                        _commiter.commitSuc();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _commiter.commitFail(ResultMgr.getInstance().lookupResult(_errCode));
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_003_016_RetNotifyRoleGameEvent();
                    }
                });
    }
}
