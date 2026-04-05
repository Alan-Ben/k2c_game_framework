package MGClient.ClientRequestMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import MGClient.ClientPlayer.ClientPlayer;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Util.CommonFunc;

import java.nio.ByteBuffer;

public abstract class _AClientRequestHandler<TResponse extends _IALProtocolStructure> implements _IClientRequestHandler
{

    private final Class<TResponse> _m_responseClass;

    public _AClientRequestHandler(Class<TResponse> _class)
    {
        _m_responseClass = _class;
    }

    private TResponse createResponse()
    {
        try
        {
            return _m_responseClass.newInstance();
        } catch (InstantiationException e)
        {
            e.printStackTrace();
        } catch (IllegalAccessException e)
        {
            e.printStackTrace();
        }
        return null;
    }

    ;

    public abstract void handle(TResponse _response);

    private ClientPlayer _m_caller;

    public ClientPlayer getCaller()
    {
        return _m_caller;
    }

    public void handleResponse(int _errCode, ByteBuffer bytes)
    {
        if (_errCode != 0)
        {
            getCaller().logProto(ResultMgr.getInstance().lookupResult(_errCode).toString());
        } else
        {
            TResponse response = createResponse();
            bytes.get();
            bytes.get();
            response.readPackage(bytes);
            handle(response);
            String packageName = response.getClass().getSimpleName();
            StringBuilder sb = new StringBuilder();
            sb.append(packageName);
            sb.append(":\n");
            CommonFunc.GetInfoPropertys(response, sb);
            getCaller().logProto(sb.toString());
        }
    }

    public void setCaller(ClientPlayer _player)
    {
        _m_caller = _player;
    }


}
