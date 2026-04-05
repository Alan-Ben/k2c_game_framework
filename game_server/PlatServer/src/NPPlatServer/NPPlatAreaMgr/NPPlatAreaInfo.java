package NPPlatServer.NPPlatAreaMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;
import NPPlatServer.NPPS_Listener.PS_GSListener;
import NPPlatServer.NPPS_Listener.PS_LSListener;
import NPPlatServer.PlatServerConf;

import java.util.ArrayList;
import java.util.concurrent.atomic.AtomicLong;

/****************
 * 平台中管理的对应区域信息对象
 *
 * @author Administrator
 *
 */
public class NPPlatAreaInfo
{
    /**
     * 通用的序列号累加对象
     */
    private static int _g_iAreaTagSerialize = 1;

    /**
     * 区域相关标记
     */
    private int _m_iAreaSerialize;
    private String _m_sAreaTag;
    /**
     * 是否公共区域
     */
    private boolean _m_bIsCommonArea;

    /**
     * 可处理的所有用户数量
     */
    private int _m_iTotalHandleUserCount;
    /**
     * 已处理的用户数量
     */
    private int _m_iHandledUserCount;

    /**
     * 本区域的登录服务器列表
     */
    private ArrayList<PS_LSListener> _m_lAreaLSList;
    /**
     * 本区域的连接服务器列表
     */
    private ArrayList<PS_GSListener> _m_lAreaGSList;

    /**
     * 管理类操作锁，由于管理类中的列表操作不频繁，因此与统计共用一个锁
     */
    private MutexAtom _m_mutex;

    private AtomicLong _m_iHandleGateCounter = new AtomicLong(0);

    public NPPlatAreaInfo(String _areaTag)
    {
        _m_iAreaSerialize = _g_iAreaTagSerialize++;
        _m_sAreaTag = _areaTag;
        _m_bIsCommonArea = _areaTag.equalsIgnoreCase(PlatServerConf.getInstance().getCommonAreaTag());

        _m_iTotalHandleUserCount = 0;
        _m_iHandledUserCount = 0;

        _m_lAreaLSList = new ArrayList<PS_LSListener>();
        _m_lAreaGSList = new ArrayList<PS_GSListener>();

        _m_mutex = new MutexAtom();
    }

    public int getAreaSerialize()
    {
        return _m_iAreaSerialize;
    }

    public String getAreaTag()
    {
        return _m_sAreaTag;
    }

    public boolean isCommonArea()
    {
        return _m_bIsCommonArea;
    }

    /*****************
     * 注册登录服务器对象
     *
     * @param _gsListener
     */
    public void regLS(PS_LSListener _lsListener)
    {
        if (null == _lsListener)
            return;

        _lock();

        try
        {
            // 将对象加入队列
            _m_lAreaLSList.add(_lsListener);
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 注册连接服务器对象
     *
     * @param _gsListener
     */
    public void regGS(PS_GSListener _gsListener)
    {
        if (null == _gsListener)
            return;

        _lock();

        try
        {
            // 增加总可加入用户统计
            _m_iTotalHandleUserCount += _gsListener.getTotalHandleUserCount();
            _m_iHandledUserCount += _gsListener.getHandleUserCount();
            // 将对象加入队列
            _m_lAreaGSList.add(_gsListener);
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 注销登录服务器对象
     *
     * @param _gsListener
     */
    public void unregLS(PS_LSListener _lsListener)
    {
        if (null == _lsListener)
            return;

        _lock();

        try
        {
            // 将对象从队列中删除
            for (int i = 0; i < _m_lAreaLSList.size(); i++)
            {
                if (_m_lAreaLSList.get(i) == _lsListener)
                {
                    // 从队列移除
                    _m_lAreaLSList.remove(i);
                    return;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /*****************
     * 注销连接服务器对象
     *
     * @param _gsListener
     */
    public void unregGS(PS_GSListener _gsListener)
    {
        if (null == _gsListener)
            return;

        // 先释放所有连接服务器用户
        int reduceAllUserCount = _gsListener.clearAllUser();

        _lock();

        try
        {
            // 将对象从队列中删除
            for (int i = 0; i < _m_lAreaGSList.size(); i++)
            {
                if (_m_lAreaGSList.get(i) == _gsListener)
                {
                    // 释放对应连接服务器信息
                    _m_iTotalHandleUserCount -= _gsListener.getTotalHandleUserCount();
                    _m_iHandledUserCount -= reduceAllUserCount;
                    // 从队列移除
                    _m_lAreaGSList.remove(i);
                    return;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /***************
     * 尝试增加处理的用户对象，返回处理的服务器对象
     *
     * @return
     */
    public PS_GSListener tryHandleUser()
    {
        long count = _m_iHandleGateCounter.incrementAndGet();
        if (count % 100 == 0)
        {
            dumpGateList();
        }

        PS_GSListener selectListener = null;
        _lock();

        try
        {
            // 先判断是否可处理用户
            if (_m_iHandledUserCount >= _m_iTotalHandleUserCount)
                return null;

            PS_GSListener tmpListener = null;
            long minWeight = Long.MAX_VALUE;
            // 从队列中寻找一个处理权重最低且还可处理用户的服务器
            for (int i = 0; i < _m_lAreaGSList.size(); i++)
            {
                tmpListener = _m_lAreaGSList.get(i);
                if (null == tmpListener || !tmpListener.canHandle())
                    continue;

                // 判断是否最小权重
                if (null == selectListener || minWeight > tmpListener.getHandleUserWeight())
                {
                    // 设置选中本服务器
                    selectListener = tmpListener;
                    minWeight = tmpListener.getHandleUserWeight();
                }
            }
        } finally
        {
            _unlock();
        }

        // 尝试由选中服务器处理用户
        if (null == selectListener || !selectListener.addHandleUser())
            return null;


        // 返回对应服务器
        return selectListener;
    }

    /*****************
     * 打印gate服务器信息
     */
    private void dumpGateList()
    {
        _lock();

        try
        {

            for (int i = 0; i < _m_lAreaGSList.size(); i++)
            {
                PS_GSListener tmpListener = _m_lAreaGSList.get(i);
                if (null == tmpListener) continue;
                CommLog.info("gateserver[{}] ip:{},port:{},getHandleUserWeight:{},getHandleUserCount:{}"
                        , i
                        , tmpListener.getConnectIp()
                        , tmpListener.getConnectPort()
                        , tmpListener.getHandleUserWeight()
                        , tmpListener.getHandleUserCount()
                );
            }
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

    public void enumLsListeners(ArrayList<PS_LSListener> _list)
    {
        if (null == _list)
            return;
        _lock();
        try
        {
            _list.addAll(_m_lAreaLSList);
        } finally
        {
            _unlock();
        }

    }
}
