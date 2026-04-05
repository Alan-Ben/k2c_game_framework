package NPGateServer.USRefVersionMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;

import java.util.Hashtable;

/******************
 * 不同服务器版本管理对象
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2020年12月9日 下午10:48:18
 */
public class USVersionMgr
{
    private static USVersionMgr _g_instance = new USVersionMgr();

    public static USVersionMgr getInstance()
    {
        return _g_instance;
    }

    private Hashtable<Integer, USVersionInfo> _m_htUSVersionInfoTable;
    private MutexAtom _m_mutex;

    protected USVersionMgr()
    {
        _m_htUSVersionInfoTable = new Hashtable<Integer, USVersionInfo>();
        _m_mutex = new MutexAtom();
    }

    /*************
     * 查询数据对象
     * @param _usTypeId
     * @return
     */
    public USVersionInfo getUSRefVersionInfo(int _usTypeId)
    {
        _m_mutex.lock();

        try
        {
            return _m_htUSVersionInfoTable.get(_usTypeId);
        } finally
        {
            _m_mutex.unlock();
        }
    }

    public USVersionInfo getOrCreateUSRefVersionInfo(int _usTypeId)
    {
        _m_mutex.lock();

        try
        {
            USVersionInfo preInfo = _m_htUSVersionInfoTable.get(_usTypeId);
            if (null == preInfo)
            {
                preInfo = new USVersionInfo(_usTypeId);
                _m_htUSVersionInfoTable.put(_usTypeId, preInfo);
            }

            return preInfo;
        } finally
        {
            _m_mutex.unlock();
        }
    }
}
