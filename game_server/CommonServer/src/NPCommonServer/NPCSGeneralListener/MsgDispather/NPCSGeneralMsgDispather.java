package NPCommonServer.NPCSGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;

public class NPCSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static NPCSGeneralMsgDispather _g_instance = new NPCSGeneralMsgDispather();

    public static NPCSGeneralMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected NPCSGeneralMsgDispather()
    {
        NPCSGeneral_001_MsgDispather_BasicOp.init(this);
        NPCSGeneral_255_MsgDispather.init(this);
    }
}
