package NPUSServer.NPGeneralListener.MsgDispather;

import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPUSServer.NPUserServer;

public class NPUSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private NPUserServer _m_server;

    public NPUSGeneralMsgDispather(NPUserServer _server)
    {
        _m_server = _server;

        NPUSGeneral_001_MsgDispather.init(this);
        NPUSGeneral_002_MsgDispather_UserOp.init(this);
        USGeneral_003_MsgDispather_AiChatOp.init(this);
    }

    public NPUserServer getUSServer() {return _m_server;}

}
