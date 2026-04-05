package RPC;

import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;

public abstract class RpcRequestHandler<T extends _ARPCData> extends RpcHandlerBase
{
    private Class<?> _m_DataClazz;

    @Override
    public void handleRpc(_IWCGBasicRequestCommiter _committer, _ARPCData _rpcData)
    {
        @SuppressWarnings("unchecked")
        T _realRpcData = (T) _rpcData;
        _rpcData.setCommitter(_committer);
        deal(_committer, _realRpcData);
    }

    public abstract void deal(_IWCGBasicRequestCommiter _committer, T _rpc);

    @Override
    public Class<?> getDataClass()
    {
        if (null == _m_DataClazz)
        {
            _m_DataClazz = getTClass(0);
        }
        return _m_DataClazz;
    }

    @SuppressWarnings("rawtypes")
    private Class getTClass(int index)
    {
        Type genType = getClass().getGenericSuperclass();

        if (!(genType instanceof ParameterizedType))
        {
            return Object.class;
        }

        Type[] params = ((ParameterizedType) genType).getActualTypeArguments();

        if (index >= params.length || index < 0)
        {
            throw new RuntimeException("Index out of bounds");
        }

        if (!(params[index] instanceof Class))
        {
            return Object.class;
        }
        return (Class) params[index];
    }
}
