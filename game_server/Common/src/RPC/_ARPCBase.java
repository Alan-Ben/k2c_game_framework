package RPC;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Util.StringFunc;

import java.nio.ByteBuffer;

public abstract class _ARPCBase<TRequest extends _IALProtocolStructure, TResponse extends _IALProtocolStructure> extends _ARPCData
{
    public _ARPCBase()
    {
        _request = createRequest();
        _response = createResponse();
    }

    private TRequest _request;
    private TResponse _response;

    public TRequest req()
    {
        return _request;
    }

    public TResponse retObj()
    {
        return _response;
    }

    public void readRequest(ByteBuffer _buf)
    {
        _request.readPackage(_buf);
    }

    public ByteBuffer getRequestBytes()
    {
        return req().makePackage();
    }

    public void readResponse(ByteBuffer _buf)
    {
        _response.readPackage(_buf);
    }

    public ByteBuffer getResponseBytes()
    {
        return retObj().makePackage();
    }

    @Override
    public String getRequestString()
    {
        return StringFunc.getString(req());
    }

    @Override
    public String getResponseString()
    {
        return StringFunc.getString(retObj());
    }

    protected abstract TRequest createRequest();

    protected abstract TResponse createResponse();

}
