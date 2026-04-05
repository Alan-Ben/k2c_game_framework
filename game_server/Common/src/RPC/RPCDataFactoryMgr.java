package RPC;

import AllRpcData.RpcDataPlaceHolder;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

public class RPCDataFactoryMgr
{
    public static RPCDataFactoryMgr getInstance()
    {
        return _instance;
    }


    private static RPCDataFactoryMgr _instance = new RPCDataFactoryMgr();

    private Map<Integer, RpcCreator> _m_lCreatorMap = new ConcurrentHashMap<>();
    private Map<Integer, Class<? extends _ARPCData>> _m_IdClassMap = new ConcurrentHashMap<>();

    public Class<? extends _ARPCData> getRpcClass(int _classId)
    {
        return _m_IdClassMap.get(_classId);
    }

    /*********
     * 根据ClassId到创建RPCData
     * @param classId
     * @return
     */
    public _ARPCData create(int classId)
    {
        RpcCreator factory = _m_lCreatorMap.get(classId);
        if (null == factory)
        {
            CommLog.error("can not find rpcCreator for class id:{}", classId, new Exception());
            return null;
        }

        return factory.createRPCData();
    }

    @SuppressWarnings("unchecked")
    public boolean Init()
    {
        String pack = RpcDataPlaceHolder.class.getPackage().getName();

        Set<Class<?>> classSet = CommClass.getClasses(pack);

        ArrayList<Class<?>> classList = new ArrayList<>();

        for (Class<?> cs : classSet)
        {
            if (_ARPCData.class.isAssignableFrom(cs))
            {
                classList.add(cs);
            }
        }
        if (classList.isEmpty())
        {
            CommLog.error("No RpcData defined!");
            return false;

        }
        for (Class<?> clazz : classList)
        {
            if (!_ARPCData.class.isAssignableFrom(clazz))
            {
                continue;
            }
            Class<? extends _ARPCData> rpcClass = (Class<? extends _ARPCData>) clazz;
            regist(rpcClass);
        }
        return true;
    }

    public void regist(Class<? extends _ARPCData> clazz)
    {
        _ARPCData rpc;
        try
        {
            rpc = clazz.newInstance();
        } catch (Throwable e)
        {
            CommLog.error("create rpc for clazz:{} failed", clazz.getName(), e);
            return;
        }
        int classId = rpc.getClassId();
        if (_m_IdClassMap.containsKey(classId))
        {
            CommLog.error("RPCDataFactoryMgr regist class:{} id:{} duplicated ", clazz.getName(), classId, new Exception());
            return;
        }
        _m_lCreatorMap.put(classId, new RpcCreator(clazz));
        _m_IdClassMap.put(classId, clazz);

    }

    public List<Class<? extends _ARPCData>> getRpcClassList()
    {
        return new ArrayList<>(_m_IdClassMap.values());
    }

    public List<Integer> getRpcIdList()
    {
        return new ArrayList<>(_m_IdClassMap.keySet());
    }

    public int getClassId(String _className)
    {
        for (Map.Entry<Integer, Class<? extends _ARPCData>> entry : _m_IdClassMap.entrySet())
        {
            if (entry.getValue().getSimpleName().equals(_className))
            {
                return entry.getKey();
            }

        }
        return 0;
    }
}
