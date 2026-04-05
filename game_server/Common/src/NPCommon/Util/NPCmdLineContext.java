package NPCommon.Util;

import NPCommon.Context._IContext;
import NPCommon.Enum.NPCommonEnum;
import NPEnum.ENPGameEvent;

public class NPCmdLineContext implements _IContext
{
    private int _m_iDeep = 0; //调研深度
    private int _m_iContextId; //上下文id
    private long _m_guid; //上下文唯一id

    /**************
     * 构造一个对象
     * @param _event
     * @return
     */
    public static NPCmdLineContext createNew(ENPGameEvent _event)
    {
        NPCmdLineContext newContext = new NPCmdLineContext();
        newContext._m_iContextId = _event.ordinal();
        newContext._m_guid = WCGGuid.newGuid();
        return newContext;
    }

    @Override
    public long getGuid()
    {
        return _m_guid;
    }

    @Override
    public int getContextId()
    {
        return _m_iContextId;
    }

    @Override
    public int getDeep()
    {
        return _m_iDeep;
    }

    @Override
    public _IContext duplicate()
    {
        NPCmdLineContext context = new NPCmdLineContext();
        context._m_guid = _m_guid;
        context._m_iContextId = _m_iContextId;
        context._m_iDeep = _m_iDeep + 1;
        return context;
    }

    @Override
    public NPCommonEnum.ENPContextType getType()
    {
        return NPCommonEnum.ENPContextType.COMMON_CONTEXT;
    }
}
