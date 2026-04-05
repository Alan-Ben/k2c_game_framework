package NPPlatServer.NPPlatAreaMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPPlatServer.NPPS_Listener.PS_LSListener;

import java.util.ArrayList;

/****************
 * 平台不同地域的数据管理对象
 * @author Administrator
 *
 */
public class NPPlatAreaMgr
{
    private static NPPlatAreaMgr _g_instance = new NPPlatAreaMgr();

    public static NPPlatAreaMgr getInstance()
    {
        return _g_instance;
    }

    /**
     * 不同区域对象的映射列表
     */
    private ArrayList<NPPlatAreaInfo> _m_lAreaInfoList;
    private MutexObject _m_mutex;

    protected NPPlatAreaMgr()
    {
        _m_lAreaInfoList = new ArrayList<NPPlatAreaInfo>();
        _m_mutex = new MutexObject();
    }

    /***************
     * 获取共用的区域信息
     * @param _areaTag
     * @return
     */
    public NPPlatAreaInfo getCommonAreaInfo()
    {
        _lock();

        try
        {
            NPPlatAreaInfo tmpInfo = null;
            for (int i = 0; i < _m_lAreaInfoList.size(); i++)
            {
                tmpInfo = _m_lAreaInfoList.get(i);
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.isCommonArea())
                    return tmpInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 根据区域标记获取区域信息对象
     * @param _areaTag
     * @return
     */
    public NPPlatAreaInfo getAreaInfo(String _areaTag)
    {
        _lock();

        try
        {
            NPPlatAreaInfo tmpInfo = null;
            for (int i = 0; i < _m_lAreaInfoList.size(); i++)
            {
                tmpInfo = _m_lAreaInfoList.get(i);
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.getAreaTag().equalsIgnoreCase(_areaTag))
                    return tmpInfo;
            }

            //无数据则创建新数据并加入队列
            tmpInfo = new NPPlatAreaInfo(_areaTag);
            _m_lAreaInfoList.add(tmpInfo);

            return tmpInfo;
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 根据区域标记获取区域信息对象
     * @param _areaTag
     * @return
     */
    public NPPlatAreaInfo lookupAreaInfoByName(String _areaTag)
    {
        _lock();

        try
        {
            NPPlatAreaInfo tmpInfo = null;
            for (int i = 0; i < _m_lAreaInfoList.size(); i++)
            {
                tmpInfo = _m_lAreaInfoList.get(i);
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.getAreaTag().equalsIgnoreCase(_areaTag))
                    return tmpInfo;
            }
            return null;

        } finally
        {
            _unlock();
        }
    }

    /****************
     * 根据区域标记序列号获取区域信息对象
     * @param _areaTag
     * @return
     */
    public NPPlatAreaInfo getAreaInfo(int _areaTagIdx)
    {
        _lock();

        try
        {
            NPPlatAreaInfo tmpInfo = null;
            for (int i = 0; i < _m_lAreaInfoList.size(); i++)
            {
                tmpInfo = _m_lAreaInfoList.get(i);
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.getAreaSerialize() == _areaTagIdx)
                    return tmpInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }


    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public void enumLoginServers(ArrayList<PS_LSListener> _list)
    {
        _lock();
        try
        {
            NPPlatAreaInfo tmpInfo = null;
            for (int i = 0; i < _m_lAreaInfoList.size(); i++)
            {
                tmpInfo = _m_lAreaInfoList.get(i);
                if (null == tmpInfo)
                    continue;
                tmpInfo.enumLsListeners(_list);
            }
        } finally
        {
            _unlock();
        }
    }
}
