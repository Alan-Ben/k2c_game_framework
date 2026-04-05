package NPCrossRankServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class NPCRSGeneralRequestDispather extends NPRequestDispatcher
{
    private static NPCRSGeneralRequestDispather _g_instance = new NPCRSGeneralRequestDispather();

    public static NPCRSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCRSGeneralRequestDispather()
    {
        NPCRSGeneral_001_RequestDispather_CrossRankOp.init(this);
    }
}
