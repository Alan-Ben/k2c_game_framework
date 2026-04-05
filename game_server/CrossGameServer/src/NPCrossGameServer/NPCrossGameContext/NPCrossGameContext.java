package NPCrossGameServer.NPCrossGameContext;

import NPCommon.Context._IContext;
import NPCommon.Enum.NPCommonEnum.ENPContextType;
import NPCommon.Util.WCGGuid;
import NPEnum.ENPGameEvent;

//*************
//*效果触发上下文
// * 
////////////// */
public class NPCrossGameContext implements _IContext
{
    private int _m_iDeep = 0; //调研深度
    private int _m_iContextId; //上下文id
    private long _m_guid; //上下文唯一id

    /**************
     * 构造一个对象
     * @param _event
     * @return
     */
    public static NPCrossGameContext createNew(ENPGameEvent _event)
    {
        NPCrossGameContext newContext = new NPCrossGameContext();
        //初始化上下文
        newContext.initContext(_event);

        return newContext;
    }

    /****
     * 屏蔽默认构造函数，避免直接创建上下文
     */
    private NPCrossGameContext()
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
    public NPCrossGameContext duplicate()
    {
        NPCrossGameContext context = new NPCrossGameContext();
        context._m_guid = _m_guid;
        context._m_iContextId = _m_iContextId;
        context._m_iDeep = _m_iDeep + 1;
        return context;
    }

    @Override
    public ENPContextType getType()
    {
        return ENPContextType.COMMON_CONTEXT;
    }

    public int getContextId()
    {
        return _m_iContextId;
    }

    public long getGuid()
    {
        return _m_guid;
    }

    public void set_m_guid(long _guid)
    {
        _m_guid = _guid;
    }

    /**************
     * 初始化上下文
     * @param _event
     */
    public void initContext(ENPGameEvent _event)
    {
        _m_iContextId = _event.ordinal();
        _m_guid = WCGGuid.newGuid();
    }
}
