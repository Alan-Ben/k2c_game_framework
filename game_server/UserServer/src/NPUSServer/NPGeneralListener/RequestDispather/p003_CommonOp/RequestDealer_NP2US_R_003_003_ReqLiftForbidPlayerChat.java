package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_LiftForbidChat;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUSUserMgr.UserComp.OfflineRewardComp.OfflineRewardFunc;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_003_ReqLiftForbidPlayerChat;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_003_RetLiftForbidPlayerChat;

public class RequestDealer_NP2US_R_003_003_ReqLiftForbidPlayerChat extends _ABasicGeneralRequestDealer<NP2US_R_003_003_ReqLiftForbidPlayerChat>
{
    public RequestDealer_NP2US_R_003_003_ReqLiftForbidPlayerChat(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_003_ReqLiftForbidPlayerChat _msg)
    {
        Offline_LiftForbidChat offlineData = new Offline_LiftForbidChat();
        offlineData.setRoomType(_msg.getRoomType().ordinal());

        for(int i = 0; i < _msg.getCidList().size(); i++)
        {
            long cid = _msg.getCidList().get(i);
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.LIFT_FORBID_CHAT);
            OfflineRewardFunc.addPlayerOfflineReward(getUSServer(), cid, EOfflineRewardEnum.LIFT_FORBID_CHAT,
                    offlineData, null, null, context);
        }

        _committer.commitSucRes(new NP2US_RB_003_003_RetLiftForbidPlayerChat());
    }
}
