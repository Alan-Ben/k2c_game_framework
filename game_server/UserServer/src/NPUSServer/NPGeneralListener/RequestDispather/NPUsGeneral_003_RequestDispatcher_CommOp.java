package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp.*;


public class NPUsGeneral_003_RequestDispatcher_CommOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_NP2US_R_003_001_ReqSendProtocol(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_002_ReqForbidPlayerChat(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_003_ReqLiftForbidPlayerChat(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_005_ReqRankFixedLikeScore(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_006_ReqRankFixedLike(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_007_ReqRankingEventTrigger(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_008_ReqGetBoxInfo(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_009_ReqAddBoxGainedCid(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_010_ReqRemoveBoxGainedCid(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_012_ReqBanCid(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_013_ReqUnBanCid(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_014_ReqSendUserMail(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_016_ReqNotifyRoleGameEvent(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_015_ReqSendMarquee(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_017_ReqSendMarqueeDel(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_018_ReqSendServerMailDel(_dispather.getUSServer()));
        _dispather.regHandler(new RequestDealer_NP2US_R_003_019_ReqPlayerDetailLike(_dispather.getUSServer()));
    }
}
