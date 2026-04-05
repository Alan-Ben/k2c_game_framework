package NPCrossGameServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

public class NPCGSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static NPCGSGeneralMsgDispather _g_instance = new NPCGSGeneralMsgDispather();

    public static NPCGSGeneralMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCGSGeneralMsgDispather()
    {
    }
}
