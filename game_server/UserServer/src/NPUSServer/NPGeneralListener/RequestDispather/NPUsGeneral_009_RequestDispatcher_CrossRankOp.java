package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPUSServer.NPGeneralListener.RequestDispather.p009_CrossRankOp.RequestDealer_ToUS_R_009_001_PushCrossRankScoreChg;

/**
 * 跨服排行榜相关协议分发器
 */
public class NPUsGeneral_009_RequestDispatcher_CrossRankOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new RequestDealer_ToUS_R_009_001_PushCrossRankScoreChg(_dispather.getUSServer()));
    }
}