package NPCommonServer.NPCSGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class NPCSGeneralRequestDispather extends NPRequestDispatcher
{
    private static NPCSGeneralRequestDispather _g_instance = new NPCSGeneralRequestDispather();

    public static NPCSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCSGeneralRequestDispather()
    {
        NPCSGeneral_002_RequestDispather_ServerInfoOp.init(this);
        NPCSGeneral_003_RequestDispather_CommonOp.init(this);
        NPCSGeneral_006_RequestDispather_CrossRankOp.init(this);
        NPCSGeneral_007_RequestDispather_GameLogicOp.init(this);
        NPCSGeneral_255_RequestDispatcher.init(this);
    }
}
