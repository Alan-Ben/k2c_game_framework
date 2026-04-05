package NPUSServer.Common.Context;

import Common.Common_Context;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.Context._IContext;
import NPCommon.Enum.NPCommonEnum.ENPContextType;
import NPCommon.Util.WCGGuid;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;

//*************
//*效果触发上下文
// * 
////////////// */
public class NPPlayerContext implements _IContext
{
    private int _m_iDeep = 0; //调研深度
    private int _m_iContextId; //上下文id
    private long _m_guid; //上下文唯一id
    private NPItemCollector _m_collector;//物品收集器

    /**************
     * 构造一个对象
     * @param _event
     * @return
     */
    public static NPPlayerContext createNew(ENPGameEvent _event)
    {
        NPPlayerContext newContext = new NPPlayerContext();
        //初始化上下文
        newContext.initContext(_event);

        return newContext;
    }

    /**************
     * 构造一个对象
     * @param _event
     * @return
     */
    public static NPPlayerContext createNew(int _event)
    {
        NPPlayerContext newContext = new NPPlayerContext();
        //初始化上下文
        newContext.initContext(_event);

        return newContext;
    }

    //通过另外一个事件对象进行构造
    public static NPPlayerContext createNew(_IContext _context)
    {
        NPPlayerContext newContext = new NPPlayerContext();
        //初始化上下文
        newContext.initContext(_context);

        return newContext;
    }
    //通过协议包构造
    public static NPPlayerContext createNew(Common_Context _context)
    {
        NPPlayerContext newContext = new NPPlayerContext();
        //初始化上下文
        ENPGameEvent enpGameEvent = ENPGameEvent.ENPGameEvent_FromInt(_context.getEventId());
        if (enpGameEvent != null)
        {
            newContext.initContext(enpGameEvent);
        }
        newContext.setGuid(_context.getGuid());
        return newContext;
    }

    /****
     * 屏蔽默认构造函数，避免直接创建上下文
     */
    private NPPlayerContext()
    {

    }

    /****
     * 返回事件调用深度
     * @return
     */
    public int getDeep()
    {
        return _m_iDeep;
    }

    /*****
     * 复制一个上下文，同时调用深度+1
     * @return
     */
    public NPPlayerContext duplicate()
    {
        NPPlayerContext context = new NPPlayerContext();
        context._m_guid = _m_guid;
        context._m_iContextId = _m_iContextId;
        context._m_iDeep = _m_iDeep + 1;
        context._m_collector = new NPItemCollector(_m_iContextId);
        return context;
    }

    @Override
    public ENPContextType getType()
    {
        return ENPContextType.COMMON_CONTEXT;
    }

    /**************
     * 初始化上下文
     * @param _event
     */
    public void initContext(ENPGameEvent _event)
    {
        _m_iContextId = _event.ordinal();
        _m_guid = WCGGuid.newGuid();
        _m_collector = new NPItemCollector(_m_iContextId);
    }

    /**************
     * 初始化上下文
     * @param _event
     */
    public void initContext(int _event)
    {
        _m_iContextId = _event;
        _m_guid = WCGGuid.newGuid();
        _m_collector = new NPItemCollector(_m_iContextId);
    }

    public void initContext(_IContext _context)
    {
        _m_iContextId = _context.getContextId();
        _m_guid = _context.getGuid();
        _m_collector = new NPItemCollector(_m_iContextId);
    }

    public int getContextId()
    {
        return _m_iContextId;
    }

    public long getGuid()
    {
        return _m_guid;
    }

    public void setGuid(long _guid)
    {
        _m_guid = _guid;
    }

    public NPItemCollector getCollector()
    {
        return _m_collector;
    }


    /**************
     * 收集获得的物品
     * @param _eItemType
     * @param _subId
     * @param _num
     */
    public void collectItem(ENPItemType _eItemType, long _subId, long _num, boolean _isNotMerge)
    {
        //根据是否合并进行处理
        if (_isNotMerge)
            getCollector().addNoMergeItem(_eItemType, _subId, _num);
        else
            getCollector().addItem(_eItemType, _subId, _num);

    }

    /**
     * 重置物品收集器
     */
    public void resetCollector()
    {
        _m_collector = new NPItemCollector(this._m_iContextId);
    }

    public Common_Context toProto()
    {
        Common_Context context = new Common_Context();
        context.setEventId(this._m_iContextId);
        context.setGuid(this._m_guid);
        return  context;
    }
}
