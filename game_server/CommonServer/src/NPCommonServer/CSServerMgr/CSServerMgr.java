package NPCommonServer.CSServerMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;

import java.util.ArrayList;
import java.util.List;

/****************
 * 平台中管理的对应区域信息对象
 * @author Administrator
 *
 */
public class CSServerMgr
{
    private static CSServerMgr _g_instance = new CSServerMgr();

    public static CSServerMgr getInstance()
    {
        if (null == _g_instance)
            _g_instance = new CSServerMgr();

        return _g_instance;
    }

    /**
     * 本区域的房间服务器信息列表
     */
    private ArrayList<CSAreaRoomServerInfo> _m_lAreaRSList;

    /**
     * 管理类操作锁，由于管理类中的列表操作不频繁，因此与统计共用一个锁
     */
    private MutexAtom _m_mutex;

    protected CSServerMgr()
    {
        _m_lAreaRSList = new ArrayList<CSAreaRoomServerInfo>();

        _m_mutex = new MutexAtom();
    }

    public List<CSAreaRoomServerInfo> getRoomServers()
    {
        return this._m_lAreaRSList;
    }

    public int roomServerCount()
    {
        return _m_lAreaRSList.size();
    }

    //根据Id查找 RoomServerInfo
    public CSAreaRoomServerInfo lookupRoomById(int _serverTypeId)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_lAreaRSList.size(); i++)
            {
                if (_m_lAreaRSList.get(i).getRoomServerTypeId() == _serverTypeId)
                {
                    return _m_lAreaRSList.get(i);
                }
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 注册房间服务器对象
     * @param _gsListener
     */
    public void regRS(int _roomServerTypeId)
    {
        _lock();

        try
        {
            CSAreaRoomServerInfo room = this.lookupRoomById(_roomServerTypeId);
            if (room != null)
            {
                ALServerLog.Error("WCGCSAreaInfo.regRS duplicate room server registration roomid:" + _roomServerTypeId + " !");
                return;
            }
            _m_lAreaRSList.add(new CSAreaRoomServerInfo(_roomServerTypeId));
        } finally
        {
            _unlock();
        }
        ALServerLog.Error("WCGCSAreaInfo.regRS success regRoomServer by ServetId::" + _roomServerTypeId + " !");
    }

    /*****************
     * 注销登录服务器对象
     * @param _gsListener
     */
    public void unregRS(int _roomServerTypeId)
    {
        _lock();

        try
        {
            //将对象从队列中删除
            for (int i = 0; i < _m_lAreaRSList.size(); i++)
            {
                if (_m_lAreaRSList.get(i).getRoomServerTypeId() == _roomServerTypeId)
                {
                    _m_lAreaRSList.remove(i);
                    return;
                }
            }
        } finally
        {
            _unlock();
        }

        ALServerLog.Error("WCGCSAreaInfo.unregRS can not find room for id:" + _roomServerTypeId + " !");
    }

    /***************
     * 清除所有服务器的信息
     */
    public void clear()
    {
        _lock();

        try
        {
            _m_lAreaRSList.clear();
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 获取最低承载的房间对象
     * @param _gsListener
     */
    public CSAreaRoomServerInfo handleUser(int _roomUserCount)
    {
        _lock();

        try
        {
            CSAreaRoomServerInfo tmpInfo = null;
            //将对象从队列中删除
            for (int i = 0; i < _m_lAreaRSList.size(); i++)
            {
                if (null == tmpInfo || _m_lAreaRSList.get(i).getRoomTotalHandleCount() < tmpInfo.getRoomTotalHandleCount())
                {
                    tmpInfo = _m_lAreaRSList.get(i);
                }
            }

            //累加用户数量，先累加，避免数据未同步时出现可能的负载均衡错误
            if (null != tmpInfo)
                tmpInfo.chgTmpHandleCount(_roomUserCount);

            return tmpInfo;
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
}
