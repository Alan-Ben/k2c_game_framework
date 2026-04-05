package RPC;

import CommonProto.Common_RB_255_001_RetRpc;
import NPCommon.Log.CommLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;

public abstract class _ARPCData
{

    private _IWCGBasicRequestCommiter _m_committer = null;

    public abstract ByteBuffer getResponseBytes();

    public abstract ByteBuffer getRequestBytes();

    public abstract void readRequest(ByteBuffer _buf);

    public abstract void readResponse(ByteBuffer _buf);

    public abstract String getRequestString();

    public abstract String getResponseString();

    public abstract int getClassId();

    public void setCommitter(_IWCGBasicRequestCommiter _committer)
    {
        _m_committer = _committer;
    }

    public _ARPCData()
    {
    }

    public _IWCGBasicRequestCommiter getCommiter()
    {
        return _m_committer;
    }

    public void commitFail(int _errCode)
    {
        if (null == _m_committer)
        {
            CommLog.error("none commiter set while commit()", new Exception(""));
            return;
        }
        _m_committer.commitFailRes(_errCode);
    }

    public void commit()
    {
        if (null == _m_committer)
        {
            CommLog.error("none commiter set while commit()", new Exception(""));
            return;
        }
        Common_RB_255_001_RetRpc proto = new Common_RB_255_001_RetRpc();
        proto.setRetBytes(getResponseBytes());
        _m_committer.commitSucRes(proto);
    }

    @Override
    public String toString()
    {
        return String.format("name:[%s]\nreq:%s\nret:%s", getClass().getSimpleName(), getRequestString(), getResponseString());
    }
}
