package NPGameRes.LogicEvent.MetaData;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;
import NPGameRes.LogicEvent.MetaData.Annotation.EventDesc;
import NPGameRes.LogicEvent._ALogicEventBase;

import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

/******
 * 时间类型的记录和统一管理
 * 只用于动态分析类型，对事件进行分类和类型数据管理。
 * 触发器和相关逻辑处理不在本类型中，相当于一种动态类分析的refdata数据管理器
 */
public class EventMetaMgr_Refdata
{
    private static EventMetaMgr_Refdata _g_instance = new EventMetaMgr_Refdata();
    public static EventMetaMgr_Refdata getInstance()
    {
        if(null == _g_instance)
            _g_instance = new EventMetaMgr_Refdata();

        return _g_instance;
    }

    private final Map<String, EventMeta> _m_metaNameMap = new ConcurrentHashMap<>(); //用名字检索事件元数据
    private final Map<Integer, EventMeta> _m_metaIDMap = new ConcurrentHashMap<>(); //用ID检索事件元数据
    private final Map<Class<? extends _ALogicEventBase>, EventMeta> _m_metaClassMap = new ConcurrentHashMap<>(); //用class检索事件元数据

    protected EventMetaMgr_Refdata()
    {
    }

    /****
     * 根据名称查找元数据
     * @param _eventName
     * @return
     */
    public EventMeta lookupMetaByEventName(String _eventName)
    {
        return _m_metaNameMap.get(_eventName);
    }

    /******
     * 根据事件id，查找事件元数据
     * @param _eventId
     * @return
     */
    public EventMeta lookupMetaByEventId(int _eventId)
    {
        return _m_metaIDMap.get(_eventId);
    }

    /******
     * 根据事件Class，查找事件元数据
     * @param _class
     * @return
     */
    public EventMeta lookupMetaByClass(Class<? extends _ALogicEventBase> _class)
    {
        return _m_metaClassMap.get(_class);
    }

    /**
     * 注册事件元数据
     * @param _eventMeta
     * @return
     */
    public boolean registEventMeta(EventMeta _eventMeta)
    {
        if (null != _m_metaNameMap.get(_eventMeta.getName()))
        {
            CommLog.error("duplicate regist event meta name:{}", _eventMeta.getName(), new Exception());
            return false;
        }
        if (_m_metaIDMap.containsKey(_eventMeta.getEventId()))
        {
            CommLog.error("duplicate regist event meta name:{} id:{}", _eventMeta.getName(), _eventMeta.getEventId(), new Exception());
            return false;
        }

        _m_metaNameMap.put(_eventMeta.getName(), _eventMeta);
        _m_metaIDMap.put(_eventMeta.getEventId(), _eventMeta);
        _m_metaClassMap.put(_eventMeta.getEventClass(), _eventMeta);
        return true;
    }

    /*****
     * 使用注解注册元数据
     * @param _metaDesc
     */
    public boolean registMetaByDesc(EventDesc _metaDesc, Class<? extends _ALogicEventBase> _eventClass)
    {
        EventMeta metaData = new EventMeta(_metaDesc.name(), _metaDesc.id(), _eventClass);

        for (int i = 0; i < _metaDesc.params().length; i++)
        {
            metaData.registParam(_metaDesc.params()[i]);
        }
        return registEventMeta(metaData);
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("Total " + _m_metaNameMap.size() + " events metas:\n");
        List<EventMeta> list = new ArrayList<EventMeta>(_m_metaNameMap.values());
        Collections.sort(list, new Comparator<EventMeta>()
        {
            @Override
            public int compare(EventMeta o1, EventMeta o2)
            {
                return o1.getEventId() - o2.getEventId();
            }
        });
        for (EventMeta meta : list)
        {
            sb.append(meta.toString());
            sb.append("\n");
        }
        return sb.toString();
    }

    ///是否做了初始化
    private boolean _m_bIsInited = false;
    private boolean _m_bInitRes = false;

    /*****
     * 初始化注册事件元数据
     * @param _packagePath
     * @return
     */
    @SuppressWarnings("unchecked")
    public synchronized boolean s_init(String _packagePath)
    {
        if (_packagePath == null || _packagePath.isEmpty())
        {
            return false;
        }

        //检索对应包名下的所有类
        Set<Class<?>> clazzs = CommClass.getClasses(_packagePath);
        if (clazzs.isEmpty())
        {
            return false;
        }

        if(_m_bIsInited)
            return _m_bInitRes;
        _m_bIsInited = true;

        //逐个类进行说明文件判断，并比对父子类
        //注册所有event的子类对象
        for (Class<?> clazz : clazzs)
        {
            EventDesc desc = clazz.getAnnotation(EventDesc.class);
            if (desc == null)
            {
                continue;
            }
            if (!_ALogicEventBase.class.isAssignableFrom(clazz))
            {
                continue;
            }

            //注册子类对象
            if (!registMetaByDesc(desc, (Class<? extends _ALogicEventBase>) clazz))
            {
                _m_bInitRes = false;
                return false;
            }
        }

        _m_bInitRes = true;
        return true;
    }
}
