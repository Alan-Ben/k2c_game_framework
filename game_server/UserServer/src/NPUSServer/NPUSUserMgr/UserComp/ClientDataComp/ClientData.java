package NPUSServer.NPUSUserMgr.UserComp.ClientDataComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import GS2GC.p011_ClientDataOp.GS2GC_011_001_RetQueryClientData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerClientDataBO;

import java.nio.ByteBuffer;

public class ClientData
{
    private NPUserServer _m_server;

    private long _m_lDbid;
    private int _m_iKey;
    private ByteBuffer _m_buffData;

    public ClientData(NPUserServer _server, PlayerClientDataBO _bo)
    {
        _m_server = _server;

        _m_lDbid = _bo.getId();
        _m_iKey = _bo.getKey();

        if (null != _bo.getClientData())
        {
            _m_buffData = ByteBuffer.wrap(_bo.getClientData());
        }
    }

    public long getDbid()
    {
        return _m_lDbid;
    }

    public int getKey()
    {
        return _m_iKey;
    }

    public void fillProto(GS2GC_011_001_RetQueryClientData retMsg)
    {
        retMsg.setIndex(getKey());

        if (null != _m_buffData)
        {
            retMsg.setData(_m_buffData);
        }
    }

    ByteBuffer getBuffData()
    {
        return _m_buffData;
    }

    public void saveClientData(byte[] _data)
    {
        _m_buffData = ByteBuffer.wrap(_data);

        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("client_data", _m_buffData);
        _m_server.getBM().getBM(PlayerClientDataBO.class).update("id", _m_lDbid, updateValue);
    }
}
