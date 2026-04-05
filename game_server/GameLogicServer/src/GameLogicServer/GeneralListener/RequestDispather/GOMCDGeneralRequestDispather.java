package GameLogicServer.GeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class GOMCDGeneralRequestDispather extends NPRequestDispatcher
{
    private static GOMCDGeneralRequestDispather _g_instance = new GOMCDGeneralRequestDispather();
    public static GOMCDGeneralRequestDispather getInstance() {return _g_instance;}

    public GOMCDGeneralRequestDispather()
    {
        GOMCDGeneral_001_RequestDispatcher.init(this);

        //RPC
        GOMCDGeneral_255_RequestDispatcher.init(this);
    }
}
