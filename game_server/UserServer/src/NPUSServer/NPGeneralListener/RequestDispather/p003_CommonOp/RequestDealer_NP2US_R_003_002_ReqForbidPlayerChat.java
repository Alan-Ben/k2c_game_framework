package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_ForbidChat;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_002_ReqForbidPlayerChat;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_002_RetForbidPlayerChat;

public class RequestDealer_NP2US_R_003_002_ReqForbidPlayerChat extends _ABasicGeneralRequestDealer<NP2US_R_003_002_ReqForbidPlayerChat>
{
    public RequestDealer_NP2US_R_003_002_ReqForbidPlayerChat(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_002_ReqForbidPlayerChat _msg)
    {
        long endMs = (_msg.getHours() == 0 ? -1 : CommonFunc.getNowTimeMS() + _msg.getHours() * 3600000);

        Offline_ForbidChat offlineData = new Offline_ForbidChat();
        offlineData.setEndMs(endMs);
        offlineData.setRoomType(_msg.getRoomType().ordinal());

        for(int i = 0; i < _msg.getCidList().size(); i++)
        {
            long cid = _msg.getCidList().get(i);
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.FORBID_CHAT);
            OfflineRewardFunc.addPlayerOfflineReward(getUSServer(), cid, EOfflineRewardEnum.FORBID_CHAT,
                    offlineData, null, null, context);
        }

        _committer.commitSucRes(new NP2US_RB_003_002_RetForbidPlayerChat());
    }
}
