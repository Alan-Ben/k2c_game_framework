package NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityForbidPlayerChat;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormForbidPlayerChatDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_002_ReqForbidPlayerChat;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_002_RetForbidPlayerChat;
import WCGCommon.Enum.NPEnum;

import java.util.ArrayList;
import java.util.HashMap;

/**
 * @description: 玩家聊天禁言
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_003_007_ForbidPlayerChat extends _ANPPlatFormHttpSubDealer<NPEntityForbidPlayerChat>
{
    @Override
    public int subOrder()
    {
        return 7;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityForbidPlayerChat> getDecoder()
    {
        return NPPlatFormForbidPlayerChatDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityForbidPlayerChat _decodeObj)
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
            NP2US_R_003_002_ReqForbidPlayerChat proto = new NP2US_R_003_002_ReqForbidPlayerChat();
            proto.getCidList().addAll(cidList);
            proto.setHours(_decodeObj.getHours());
            proto.setRoomType(_decodeObj.getChatRoomType());

            NPHttpServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.USER.ordinal(), usId, proto,
                    new _IWCGCallbackDealer()
                    {
                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2US_RB_003_002_RetForbidPlayerChat();
                        }

                        @Override
                        public void dealSuc(_IALProtocolStructure ialProtocolStructure)
                        {
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            CommLog.error("NP2HS_003_007_ForbidPlayerChat usId:{} errCode:{}", usId, _errCode);
                        }
                    });
        });

        _commiter.commitSuc();
    }
}
