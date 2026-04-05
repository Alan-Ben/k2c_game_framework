package NPPlatServer.NPGeneralListener.RequsetDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class NPPSGeneralRequestDispather extends NPRequestDispatcher
{
    private static NPPSGeneralRequestDispather _g_instance = new NPPSGeneralRequestDispather();

    public static NPPSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPPSGeneralRequestDispather()
    {
        NPPSGeneral_001_RequestDispather.init(this);
        NPPSGeneral_003_RequestDispather.init(this);
        NPPSGeneral_100_RequestDispather.init(this);
        NPPSGeneral_255_RequestDispatcher.init(this);
    }
}
