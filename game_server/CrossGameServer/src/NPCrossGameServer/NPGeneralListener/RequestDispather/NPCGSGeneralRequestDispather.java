package NPCrossGameServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher;

public class NPCGSGeneralRequestDispather extends NPRequestDispatcher
{
    private static NPCGSGeneralRequestDispather _g_instance = new NPCGSGeneralRequestDispather();

    public static NPCGSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCGSGeneralRequestDispather()
    {
        NPCGSGeneral_001_RequestDispather_BasicOp.init(this);
        NPCGSGeneral_002_RequestDispatcher_CommGameOp.init(this);
        NPCGSGeneral_255_RequestDispatcher.init(this);
    }
}
