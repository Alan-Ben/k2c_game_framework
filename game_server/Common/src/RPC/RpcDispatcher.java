package RPC;


import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import RPC.DefaultHandler.DefaultRpcHandlerHolder;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class RpcDispatcher
{
    private static RpcDispatcher _g_rpcDispatcher;

    public static RpcDispatcher getGlobalRpcDispatcher()
    {
        return _g_rpcDispatcher;
    }

    public RpcDispatcher()
    {
        _g_rpcDispatcher = this;
        autoRegistHandler(DefaultRpcHandlerHolder.class.getPackage().getName());
        autoRegistHandler(this.getClass().getPackage().getName());
    }

    private final Map<Class<?>, RpcHandlerBase> _m_handlers = new ConcurrentHashMap<>();
    private _ARpcLogger _m_logger;

    public void dispatchRpc(final _IWCGBasicRequestCommiter _committer, final _ARPCData _rpcData)
    {
        final RpcHandlerBase handler = _m_handlers.get(_rpcData.getClass());
        if (null == handler)
        {
            CommLog.error("can not find rpc handler for class {}", _rpcData.getClass().getName(), new Exception("Error"));
            return;
        }
        try
        {
            handler.handleRpc(_committer, _rpcData);
            if (null != getLogger())
            {
                getLogger().logSucc(_rpcData);
            }
        } catch (Throwable e)
        {
            if (null != getLogger())
            {
                getLogger().logFail(_rpcData, e.getMessage());
            }
            CommLog.error("handle rpc {} caught exception:\n", _rpcData.getClass().toString(), e);
            _committer.commitFailRes(CommErr.SYS_ERR.getCode());
        }

    }

    public void regHandler(RpcHandlerBase _handler)
    {
        if (null != _m_handlers.get(_handler.getDataClass()))
        {
            CommLog.error("Duplicate regist rpc handler for class " + _handler.getDataClass().getName());
        }
        _m_handlers.put(_handler.getDataClass(), _handler);
    }

    public _ARpcLogger getLogger()
    {
        return _m_logger;
    }

    public void setLogger(_ARpcLogger _logger)
    {
        _m_logger = _logger;
    }

    /*********
     * 自动注册RPC处理类
     */
    public void autoRegistHandler(String _packagePath)
    {
        List<Class<?>> clazzs = CommClass.getAllClassByInterface(_IAutoRegistHandler.class, _packagePath);
        for (Class<?> clazz : clazzs)
        {
            try
            {
                RpcHandlerBase handler = (RpcHandlerBase) clazz.newInstance();
                regHandler(handler);
            } catch (Throwable e)
            {
                CommLog.error("auto regist rpc handler:{} err", clazz.getSimpleName(), e);
            }
        }
    }
}
