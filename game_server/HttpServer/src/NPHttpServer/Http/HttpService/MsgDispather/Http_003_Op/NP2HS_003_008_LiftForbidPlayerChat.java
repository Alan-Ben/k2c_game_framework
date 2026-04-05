package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityLiftForbidPlayerChat;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormLiftForbidPlayerChatDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_003_ReqLiftForbidPlayerChat;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_003_RetLiftForbidPlayerChat;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * @description: 玩家聊天禁言
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_003_008_LiftForbidPlayerChat extends _ANPPlatFormHttpSubDealer<NPEntityLiftForbidPlayerChat>
{
    @Override
    public int subOrder()
    {
        return 8;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityLiftForbidPlayerChat> getDecoder()
    {
        return NPPlatFormLiftForbidPlayerChatDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityLiftForbidPlayerChat _decodeObj)
    {
        HashMap<Integer, ArrayList<Long>> forbidUsCidMap = new HashMap<>();

        for (long cid : _decodeObj.getForbidCidList())
        {
            int usId = CommonFunc.parseServerTypeIdFromCid(cid);
            ArrayList<Long> cidList = forbidUsCidMap.computeIfAbsent(usId, k -> new ArrayList<>());
            cidList.add(cid);
        }

        forbidUsCidMap.forEach((usId, cidList) ->
        {
            NP2US_R_003_003_ReqLiftForbidPlayerChat proto = new NP2US_R_003_003_ReqLiftForbidPlayerChat();
            proto.getCidList().addAll(cidList);
            proto.setRoomType(_decodeObj.getChatRoomType());

            NPHttpServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.USER.ordinal(), usId, proto,
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2US_RB_003_003_RetLiftForbidPlayerChat();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure ialProtocolStructure)
                        {
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            CommLog.error("NP2HS_003_008_LiftForbidPlayerChat usId:{} errCode:{}", usId, _errCode);
                        }
                    });
        });

        _commiter.commitSuc();
    }
}
