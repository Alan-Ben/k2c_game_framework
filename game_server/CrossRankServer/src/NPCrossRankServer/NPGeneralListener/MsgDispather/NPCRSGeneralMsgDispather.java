package NPCrossRankServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

public class NPCRSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static NPCRSGeneralMsgDispather _g_instance = new NPCRSGeneralMsgDispather();

    public static NPCRSGeneralMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCRSGeneralMsgDispather()
    {
    }
}
