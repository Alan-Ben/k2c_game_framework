package NPUSServer.NPGeneralListener.RequestDispather;

import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPUSServer.NPUserServer;

public abstract class _ABasicGeneralRequestDealer<T extends ALBasicProtocolPack._IALProtocolStructure> extends NPRequestDealer<T>
{
    private final NPUserServer _m_server;

    public _ABasicGeneralRequestDealer(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer() {return _m_server;}
}
