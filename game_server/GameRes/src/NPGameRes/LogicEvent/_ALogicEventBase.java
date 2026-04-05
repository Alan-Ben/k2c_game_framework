package NPGameRes.LogicEvent;

import NPCommon.CommonObj.NPCountRate;
import NPCommon.Context._IContext;
import NPCommon.Enum.NPCommonEnum.ENPVariableCalType;
import NPCommon.Log.CommLog;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent.MetaData.EventParam;

import java.lang.reflect.Constructor;
import java.nio.ByteBuffer;
import java.util.ArrayList;

/*****
 * 逻辑事件基类
 */
public abstract class _ALogicEventBase
{
    private EventMeta _m_meta; //事件元数据
    private long[] _m_arrParams; //参数列表

    private _IContext _m_context;//事件上下文


    /****
     * 构造函数
     * @param _context
     */
    public _ALogicEventBase(_IContext _context)
    {
        //拷贝一份上下文，每个event的上下文都是独立存在的
        _m_context = _context.duplicate();
        //检索本对象的数据
        _m_meta = EventMetaMgr_Refdata.getInstance().lookupMetaByClass(this.getClass());
        if (null == _m_meta)
        {
            CommLog.error("Can not find Meta data for Event class:{} ", this.getClass().getName());
        }

        //构造参数数组
        _m_arrParams = new long[_m_meta.getParamCount()];
    }

    protected _ALogicEventBase(long[] _params, _IContext _context)
    {
        _m_context = _context.duplicate();
        _m_meta = EventMetaMgr_Refdata.getInstance().lookupMetaByClass(this.getClass());
        if (null == _m_meta)
        {
            CommLog.error("Can not find Meta data for Event class:{} ", this.getClass().getName());
        }

        //构造参数数组
        _m_arrParams = new long[_m_meta.getParamCount()];
        for (int i = 0; i < _params.length; i++)
        {
            if (i < _m_arrParams.length)
            {
                _m_arrParams[i] = _params[i];
            }
        }
    }
    
    public _IContext getContext()
    {
        return _m_context;
    }

    /*****
     * 返回事件元数据
     * @return
     */
    public EventMeta getMeta()
    {
        return _m_meta;
    }
    
    public void setMeta(EventMeta _meta)
    {
    	_m_meta = _meta;
    }

    /****
     * 返回事件id
     * @return
     */
    public int getEventId()
    {
        return getMeta().getEventId();
    }

    /*
     * 返回事件名称
     */
    public String getEventName()
    {
        return getMeta().getName();
    }

    /*****
     * 根据参数名，设置参数
     * @param paramName
     * @param _value
     */
    public void setParamValue(String paramName, long _value)
    {
        EventParam eventParam = getMeta().lookupParam(paramName);
        if (eventParam == null)
        {
            CommLog.error("{} setParamValue invalid param:{}", getClass().getName(), paramName, new Exception());
            return;
        }
        _m_arrParams[eventParam.index] = _value;
    }

    /****
     * 根据参数索引设置参数
     * @param _paramIndex
     * @param _value
     */
    public void setParamValue(int _paramIndex, long _value)
    {
        EventParam eventParam = getMeta().lookupParamByIndex(_paramIndex);
        if (eventParam == null)
        {
            CommLog.error("{} setParamValue invalid param:{}", getClass().getName(), _paramIndex, new Exception());
            return;
        }
        _m_arrParams[eventParam.index] = _value;
    }
    
    public void setParamValue(ArrayList<Long> _params)
    {
    	if(null == _params)
    		return;
    	
    	_m_arrParams = new long[_params.size()];
        for(int i = 0; i < _params.size(); i++)
        {
        	_m_arrParams[i] = _params.get(i);
        }
    }

    /*****
     * 根据参数索引，返回参数值
     * @param _index
     * @return
     */
    public long getParamValue(int _index)
    {
        if (_index < 0 || _index >= _m_arrParams.length)
        {
            return 0;
        }
        return _m_arrParams[_index];
    }

    /*****
     * 根据参数名称，返回参数值
     * @param _paramName
     * @return
     */
    public long getParamValue(String _paramName)
    {
        if (null == _paramName)
            return 0;

        EventParam eventParam = getMeta().lookupParam(_paramName);
        if (null == eventParam)
        {
            return 0L;
        }
        return getParamValue(eventParam.index);
    }

    /**
     * 根据数值配置的数据进行解析
     * <p>
     * 规则1：如果未设置，返回0
     * 规则2：如果param是null/空字符串，表示数值只设置了rate，直接返回rate
     * 规则3：其他情况 rate*对应的参数所带的数值
     * @param _obj
     * @return
     */
    public long getValue(NPCountRate _obj)
    {
        if (null == _obj)
            return 0;

        //纯数值模式
        if (null == _obj.param() || _obj.param().isEmpty())
            return _obj.rate();

        //需要引入事件参数
        if(ENPVariableCalType.NONE == _obj.type())
        {
        	return getParamValue(_obj.param());
        }
        else if(ENPVariableCalType.ADD == _obj.type())
        {
        	return getParamValue(_obj.param()) + _obj.rate();
        }
        else if(ENPVariableCalType.SUB == _obj.type())
        {
        	return getParamValue(_obj.param()) - _obj.rate();
        }
        else if(ENPVariableCalType.MUL == _obj.type())
        {
        	return getParamValue(_obj.param()) * _obj.rate();
        }
        else if(ENPVariableCalType.DIV == _obj.type())
        {
        	return getParamValue(_obj.param()) / _obj.rate();
        }
        else
        {
            CommLog.error("_ALogicEventBase.getValue not find value type:{}.", _obj.type());
            return 0;
        }
    }

    public ByteBuffer toByteBuffer()
    {
        int size = 4 + 4 + _m_arrParams.length * 8;//EventId+参数数据长度
        ByteBuffer buff = ByteBuffer.allocate(size);
        buff.putInt(getEventId());
        buff.putInt(_m_arrParams.length);
        for (long params : _m_arrParams)
        {
            buff.putLong(params);
        }
        return buff;
    }

    @SuppressWarnings("rawtypes")
    public static _ALogicEventBase createFromByteBuffer(ByteBuffer buffer, _IContext _context)
    {
        if (null == buffer)
        {
            CommLog.error("LogicEvent createFromByteBuffer failed,buffer==null");
            return null;
        }
        int eventId = buffer.getInt();
        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventId(eventId);
        if (null == eventMeta)
        {
            CommLog.error("LogicEvent createFromByteBuffer failed,can not find EventMeta for id:{}", eventId);
            return null;
        }

        int len = buffer.getInt();
        long[] longParams = new long[len];
        for (int i = 0; i < len; i++)
        {
            longParams[i] = buffer.getLong();
        }

        _ALogicEventBase eventBase = null;
        try
        {
            Class[] params = new Class[2];
            params[0] = long[].class;
            params[1] = _IContext.class;
            Constructor cr = eventMeta.getEventClass().getConstructor(params);
            Object[] args = new Object[2];
            args[0] = longParams;
            args[1] = _context;
            eventBase = (_ALogicEventBase) cr.newInstance(args);

        } catch (Exception e)
        {
            CommLog.error("LogicEvent  createFromByteBuffer can not instance class for name:{}", eventMeta.getEventClass().getName(), e);
            return null;
        }
        return eventBase;
    }

    /**
     * 通过事件名称和参数列表创建事件实例（用于远程触发）
     * @param _eventName 事件名称，与 @EventDesc.name 一致
     * @param _params    参数列表
     * @param _context   事件上下文
     * @return 事件实例，找不到元数据或反射失败时返回 null
     */
    @SuppressWarnings("rawtypes")
    public static _ALogicEventBase createFromNameAndParams(String _eventName, java.util.ArrayList<Long> _params, _IContext _context)
    {
        if (null == _eventName || _eventName.isEmpty())
        {
            CommLog.error("_ALogicEventBase.createFromNameAndParams failed: eventName is empty");
            return null;
        }

        EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(_eventName);
        if (null == eventMeta)
        {
            CommLog.error("_ALogicEventBase.createFromNameAndParams failed, can not find EventMeta for name:{}", _eventName);
            return null;
        }

        int paramCount = (_params != null) ? _params.size() : 0;
        long[] longParams = new long[paramCount];
        for (int i = 0; i < paramCount; i++)
        {
            longParams[i] = _params.get(i);
        }

        try
        {
            Class[] paramTypes = new Class[2];
            paramTypes[0] = long[].class;
            paramTypes[1] = _IContext.class;
            Constructor cr = eventMeta.getEventClass().getConstructor(paramTypes);
            Object[] args = new Object[2];
            args[0] = longParams;
            args[1] = _context;
            return (_ALogicEventBase) cr.newInstance(args);
        }
        catch (Exception e)
        {
            CommLog.error("_ALogicEventBase.createFromNameAndParams can not instance class for name:{}", _eventName, e);
            return null;
        }
    }
}
