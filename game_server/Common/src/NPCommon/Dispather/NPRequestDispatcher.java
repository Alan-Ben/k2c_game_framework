package NPCommon.Dispather;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.WCGBasicRequestDispather;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys.WCGBasicRequestMainOrderDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._AWCGBasicRequestSubOrderDealer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.nio.ByteBuffer;

/*
regHandler(new WCGRequestDealer<XXXXXXX>() {

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, XXXXXXX _msg)
    {
            
    }
});
*/
public class NPRequestDispatcher
{
    // 主消息分发器对象
    private WCGBasicRequestDispather _m_mainDispatcher = new WCGBasicRequestDispather()
    {
    };

    // 注册接口
    public <T extends _IALProtocolStructure> void regHandler(NPRequestDealer<T> handler)
    {
        CommonMainOrderDelaer commMainOrderDealer = _getMainOrderDealer(handler.getMainOrder());
        commMainOrderDealer.RegProtocol(handler);
    }

    protected void logMsg(_IALProtocolStructure _msg)
    {
    }

    // 消息处理接口
    public boolean DealProtocol(_IWCGBasicRequestCommiter _receiver, ByteBuffer _msg)
    {
        return this._m_mainDispatcher.DealProtocol(_receiver, _msg);
    }
    public boolean DealProtocol(_IWCGBasicRequestCommiter _receiver, _IALProtocolStructure _protocol)
    {
        return this._m_mainDispatcher.DealProtocol(_receiver, _protocol);
    }

    // 通用 的Main Order dealer
    private static class CommonMainOrderDelaer extends WCGBasicRequestMainOrderDealer
    {
        public CommonMainOrderDelaer(byte _mainOrder, int _protocolMaxTypeNum)
        {
            super(_mainOrder, _protocolMaxTypeNum);
        }

        public void RegProtocol(@SuppressWarnings("rawtypes") _AWCGBasicRequestSubOrderDealer handler)
        {
            this.regDealer(handler);
        }
    }

    // 获取MainOrderDealer，没有的话，注册一个
    private CommonMainOrderDelaer _getMainOrderDealer(byte _maindId)
    {
        WCGBasicRequestMainOrderDealer dealer = _m_mainDispatcher.getDealer(_maindId);
        if (dealer == null)
        {
            dealer = new CommonMainOrderDelaer(_maindId, 255);
            _m_mainDispatcher.RegProtocol(dealer);
        }
        return (CommonMainOrderDelaer) dealer;
    }


    // 通用的Sub Order dealer
    public static abstract class NPRequestDealer<T extends ALBasicProtocolPack._IALProtocolStructure> extends
            _AWCGBasicRequestSubOrderDealer<T>
    {
        private Class<?> _m_cClazz = null;

        protected abstract void _dealMessage(_IWCGBasicRequestCommiter _receiver, T _msg);

        @Override
        protected void _dealProtocol(_IWCGBasicRequestCommiter _receiver, T _msg)
        {
            try
            {
                _dealMessage(_receiver, _msg);
            } catch (Exception e)
            {
                CommLog.error("deal request proto {} caught execption:\n", _msg.getClass().toString(), e);
            }

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
                throw new RuntimeException("Index outof bounds");
            }

            if (!(params[index] instanceof Class))
            {
                return Object.class;
            }
            return (Class) params[index];
        }

        @SuppressWarnings("unchecked")
        @Override
        protected T _createProtocolObj()
        {
            try
            {
                if (null == _m_cClazz)
                {
                    _m_cClazz = getTClass(0);
                }
                return (T) _m_cClazz.newInstance();
            } catch (InstantiationException e)
            {
                e.printStackTrace();
            } catch (IllegalAccessException e)
            {
                e.printStackTrace();
            }
            return null;
        }
    }
}
