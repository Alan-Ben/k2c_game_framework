package RPC;


import NPCommon.Log.CommLog;

public class RpcCreator
{
    private final Class<?> _m_clazz;

    public RpcCreator(Class<?> _clazz)
    {
        _m_clazz = _clazz;
    }

    public _ARPCData createRPCData()
    {
        Object obj;
        try
        {
            obj = _m_clazz.newInstance();
        } catch (Exception e)
        {
            CommLog.error(" newInstance can not instance class for name:{}", _m_clazz.getName(), e);
            return null;
        }
        if (null == obj)
        {
            CommLog.error(" newInstance can not instance class for name:{}", _m_clazz.getName());
            return null;
        }
        if (!(obj instanceof _ARPCData))
        {
            CommLog.error(" newInstance can not instance class for name:{},real class is:{}", _m_clazz.getName(), obj.getClass().getName());
            return null;
        }
        return (_ARPCData) obj;
    }
}
