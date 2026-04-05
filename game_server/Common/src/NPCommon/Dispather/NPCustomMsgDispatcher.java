package NPCommon.Dispather;

import ALBasicProtocolPack.BasicObj.ALBasicProtocolDispather;
import ALBasicProtocolPack.BasicObj._AALBasicProtocolMainOrderDealer;
import ALBasicProtocolPack.BasicObj._AALBasicProtocolSubOrderDealer;
import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;

import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.nio.ByteBuffer;

/* 
this.regHandler(new WCGCustomMsgDealer<XXXXXXXX>() {
    @Override
    protected void _dealMessage(_IALProtocolReceiver _receiver, XXXXXXXX _msg)
    {

    }
});
 */
public class NPCustomMsgDispatcher
{
    public static interface _IWCGMsgLocker
    {
        void lockMsg();

        void unlockMsg();
    }

    // 主消息分发器对象
    private ALBasicProtocolDispather _m_mainDispatcher = new ALBasicProtocolDispather()
    {
    };

    // 注册接口
    public <T extends _IALProtocolStructure> void regHandler(NPCustomMsgDealer<T> handler)
    {
        CommonMainOrderDelaer commMainOrderDealer = _getMainOrderDealer(handler.getMainOrder());
        commMainOrderDealer.RegProtocol(handler);
    }

    // 消息处理接口
    public boolean DealProtocol(_IALProtocolReceiver _receiver, ByteBuffer _msg)
    {
        return this._m_mainDispatcher.DealProtocol(_receiver, _msg);
    }

    // 通用 的Main Order dealer
    private static class CommonMainOrderDelaer extends _AALBasicProtocolMainOrderDealer
    {
        public CommonMainOrderDelaer(byte _mainOrder, int _protocolMaxTypeNum)
        {
            super(_mainOrder, _protocolMaxTypeNum);
        }

        public void RegProtocol(@SuppressWarnings("rawtypes") _AALBasicProtocolSubOrderDealer _subOrderDealer)
        {
            this.regDealer(_subOrderDealer);
        }
    }

    // 通用的Sub Order dealer
    public abstract static class NPCustomMsgDealer<T extends ALBasicProtocolPack._IALProtocolStructure> extends
            _AALBasicProtocolSubOrderDealer<T>
    {
        private Class<?> _m_cClazz = null;

        public NPCustomMsgDealer()
        {
        }

        protected abstract void _dealMessage(_IALProtocolReceiver _receiver, T _msg);

        @Override
        protected void _dealProtocol(_IALProtocolReceiver _receiver, T _msg)
        {
            _AWCGProtoLogger logger = _AWCGProtoLogger.getLogger();
            if (null != logger)
            {
                logger.logProto(_receiver, _msg);
            }
            _IWCGMsgLocker locker = null;
            if (_receiver instanceof _IWCGMsgLocker)
            {
                locker = (_IWCGMsgLocker) _receiver;
            }

            if (locker != null) locker.lockMsg();
            try
            {
                _dealMessage(_receiver, _msg);
            } catch (Exception e)
            {
                CommLog.error("deal custom proto {} caught execption:\n", _msg.getClass().toString(), e);
            } finally
            {
                if (locker != null) locker.unlockMsg();
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

    // 获取MainOrderDealer，没有的话，注册一个
    private CommonMainOrderDelaer _getMainOrderDealer(byte _maindId)
    {
        _AALBasicProtocolMainOrderDealer dealer = _m_mainDispatcher.getDealer(_maindId);
        if (dealer == null)
        {
            dealer = new CommonMainOrderDelaer(_maindId, 255);
            _m_mainDispatcher.RegProtocol(dealer);
        }
        return (CommonMainOrderDelaer) dealer;
    }
}
