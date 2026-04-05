package ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;
import ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.p001_ShareCodeOp.RequestDealer_ToSCS_R_001_001_GenShareCode;
import ShareCodeCenter.ShareCodeServer.GeneralListener.RequestDispather.p001_ShareCodeOp.RequestDealer_ToSCS_R_001_002_GetShareData;

public class SCSGeneral_001_RequestDispatcher_ShareCodeOp extends NPRequestDispatcher
{
    public static void init(SCSGeneralRequestDispather _dispatcher)
    {
        _dispatcher.regHandler(new RequestDealer_ToSCS_R_001_001_GenShareCode());
        _dispatcher.regHandler(new RequestDealer_ToSCS_R_001_002_GetShareData());
    }
}
