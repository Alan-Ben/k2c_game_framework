package MGClient.ClientRequestMgr;

import ALBasicProtocolPack._IALProtocolStructure;

import java.nio.ByteBuffer;

public abstract class _AClientRequest<TRequest extends _IALProtocolStructure, TResponse extends _IALProtocolStructure> extends ClientRequestBase
{
    public _AClientRequest()
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
        return req().makeFullPackage();
    }

    public void readResponse(ByteBuffer _buf)
    {
        _response.readPackage(_buf);
    }

    public ByteBuffer getResponseBytes()
    {
        return retObj().makeFullPackage();
    }

    protected abstract TRequest createRequest();

    protected abstract TResponse createResponse();
}
