package NPGameRes.LogicEvent.MetaData;

import NPGameRes.LogicEvent._ALogicEventBase;

import java.util.ArrayList;
import java.util.List;

/*********
 * 事件系统元数据，描述事件名称，id，参数列表
 */
public class EventMeta
{
    private final String _m_sEventName; //事件名称
    private final int _m_iEventId; //事件id
    private final List<EventParam> _m_alParams = new ArrayList<>(); //事件参数列表
    private final Class<? extends _ALogicEventBase> _m_eventClass; //事件对应的Class;

    public String getName()
    {
        return _m_sEventName;
    }

    public int getEventId()
    {
        return _m_iEventId;
    }

    public Class<? extends _ALogicEventBase> getEventClass()
    {
        return _m_eventClass;
    }
    
    public List<EventParam> getParamList()
    {
    	return _m_alParams;
    }

    /**
     * 构造函数
     * @param _name
     * @param _id
     */
    public EventMeta(String _name, int _id, Class<? extends _ALogicEventBase> _eventClass)
    {
        _m_sEventName = _name;
        _m_iEventId = _id;
        _m_eventClass = _eventClass;
    }

    /****
     * 注册一个人事件参数
     * @param _paramName
     */
    public void registParam(String _paramName)
    {
        EventParam param = new EventParam();

        param.index = _m_alParams.size();
        param.name = _paramName;

        _m_alParams.add(param);
    }

    /*******
     * 根据参数名称，查找参数
     * @param _paramName
     * @return
     */
    public EventParam lookupParam(String _paramName)
    {
        for (EventParam param : _m_alParams)
        {
            if (param.name.toUpperCase().compareTo(_paramName.toUpperCase()) == 0)
            {
                return param;
            }
        }
        return null;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append(String.format("[%d][%s] class:[]Params:[", getEventId(), getName(), getEventClass().getSimpleName()));
        for (EventParam param : _m_alParams)
        {
            sb.append(String.format("[%d]=%s;", param.index, param.name));
        }
        sb.append("]");
        return sb.toString();
    }

    public int getParamCount()
    {
        return _m_alParams.size();
    }

    public EventParam lookupParamByIndex(int _index)
    {
        if (_index < 0 || _index >= _m_alParams.size())
        {
            return null;
        }
        return _m_alParams.get(_index);
    }
}
