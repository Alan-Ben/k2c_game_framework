package CrossDataServer.GeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class GOMCDGeneralRequestDispather extends NPRequestDispatcher
{
    private static GOMCDGeneralRequestDispather _g_instance = new GOMCDGeneralRequestDispather();
    public static GOMCDGeneralRequestDispather getInstance()
    {
        if(null == _g_instance)
            _g_instance = new GOMCDGeneralRequestDispather();
        return _g_instance;
    }

    public GOMCDGeneralRequestDispather()
    {
        GOMCDGeneral_001_RequestDispatcher_DataOp.init(this);
    }
}
