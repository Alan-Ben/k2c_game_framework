package NPPlatServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

public class NPPSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static NPPSGeneralMsgDispather _g_instance = new NPPSGeneralMsgDispather();

    public static NPPSGeneralMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected NPPSGeneralMsgDispather()
    {
        NPPSGeneral_001_MsgDispather.init(this);
        NPPSGeneral_002_MsgDispather.init(this);
    }
}
