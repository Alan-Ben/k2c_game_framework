package NPHttpServer.HsClientListener;

import ALBasicServer.ALSocket._AALBasicServerSocketListener;
import NPCommon.Log.CommLog;

import java.nio.ByteBuffer;

public class HsClientListener extends _AALBasicServerSocketListener
{
    //监听序列号
    private static long _g_ClientSessionId = 1;

    private synchronized long _GetNewSerialize()
    {
        if (Long.MAX_VALUE == _g_ClientSessionId)
        {
            CommLog.error("PHP Client Sessionid -> Long MaxValue!");
            _g_ClientSessionId = 1;
        }
        return _g_ClientSessionId++;
    }

    ///////////////////////////////////// 实例方法 /////////////////////////////////////

    private long _m_lClientSessionId;

    public HsClientListener()
    {
        _m_lClientSessionId = _GetNewSerialize();

    }

    public long getSessionId()
    {
        return _m_lClientSessionId;
    }

    @Override
    public void disconnect()
    {

        CommLog.sys("Local Client Disconnect! [{}]", _m_lClientSessionId);
    }

    @Override
    public void login()
    {
        CommLog.sys("Local Client login Suc! [{}]", _m_lClientSessionId);
    }

    @Override
    public void receiveMsg(ByteBuffer _msg)
    {
        //处理协议
        HsClientMsgDispather.getInstance().DealProtocol(this, _msg);
    }

    @Override
    public void onBuffLengthOverSize(ByteBuffer p1, ByteBuffer p2)
    {
        CommLog.error("onBuffLengthOverSize msg:[{}-{}]", p1.get(0), p1.get(1), new Exception());
    }
}
