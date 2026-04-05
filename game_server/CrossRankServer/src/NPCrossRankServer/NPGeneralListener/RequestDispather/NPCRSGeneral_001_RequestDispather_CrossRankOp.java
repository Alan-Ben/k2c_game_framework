package NPCrossRankServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp.*;

public class NPCRSGeneral_001_RequestDispather_CrossRankOp extends NPRequestDispatcher
{
    public static void init(NPCRSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer_001_001_RegCrossInstance());
        _dispather.regHandler(new NPRequestDealer_001_002_UnregCrossInstance());
        _dispather.regHandler(new NPRequestDealer_001_003_RegCrossRank());
        _dispather.regHandler(new NPRequestDealer_001_004_UnregCrossRank());
        _dispather.regHandler(new NPRequestDealer_001_005_RequestCrossInstance());
        _dispather.regHandler(new NPRequestDealer_001_006_DiscardCrossInstance());
        _dispather.regHandler(new NPRequestDealer_001_007_ReqCrossRankServerInfo());
        _dispather.regHandler(new RequestDealer_NP2CRS_R_001_008_RegUploadRankData());
        _dispather.regHandler(new NPRequestDealer_001_010_SetCrossRankScore());
        _dispather.regHandler(new NPRequestDealer_001_011_SetCrossRankSubScore());
        _dispather.regHandler(new NPRequestDealer_001_020_GetCrossRankListSize());
        _dispather.regHandler(new NPRequestDealer_001_021_GetCrossRankBaseList());
        _dispather.regHandler(new NPRequestDealer_001_022_GetCrossRankBaseByRank());
        _dispather.regHandler(new NPRequestDealer_001_023_GetCrossRankBaseByKey());
        _dispather.regHandler(new NPRequestDealer_001_024_GetCrossRankBaseListByUs());
        _dispather.regHandler(new NPRequestDealer_001_025_GetCrossRankBaseByKey2());
        _dispather.regHandler(new NPRequestDealer_001_026_GetCrossRankBaseSubListByKey());
    }
}
