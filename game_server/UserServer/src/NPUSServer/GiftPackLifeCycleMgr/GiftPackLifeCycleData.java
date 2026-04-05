package NPUSServer.GiftPackLifeCycleMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import USDB.Bo.GiftPackLifeCycleDataBO;

import java.util.HashSet;
import java.util.Set;

/**
 * 礼包生命周期数据
 * 记录单个礼包在不同活动实例中的生命周期状态
 * 跟踪哪些活动实例当前正在使用此礼包
 */
public class GiftPackLifeCycleData
{
    // 礼包ID
    private long _m_giftPackId;
    // 数据库ID
    private long _m_dbId;
    // 当前已开启的活动实例ID集合，记录哪些活动实例正在使用此礼包
    private Set<Long> _m_openedActivityInstanceList;
    // 预计结束时间
    private long _m_expectedEndTimeMs;

    // 互斥锁，保护多线程访问
    private MutexAtom _m_mutex;

    /**
     * 构造函数，初始化礼包生命周期数据
     * @param _bo 数据对象
     */
    public GiftPackLifeCycleData(GiftPackLifeCycleDataBO _bo)
    {
        _m_giftPackId = _bo.getGiftPackId();
        _m_dbId = _bo.getId();
        _m_openedActivityInstanceList = new HashSet<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    public long getExpectedEndTimeMs()
    {
        return _m_expectedEndTimeMs;
    }

    /**
     * 处理活动实例开启事件
     * 将活动实例ID添加到已开启集合中，表示该活动实例开始使用此礼包
     * @param _activityInstanceId 活动实例ID
     * @param _endTimeMs
     */
    public void onActivityInstanceOpened(long _activityInstanceId, long _endTimeMs)
    {
        _lock();
        try{
            _m_openedActivityInstanceList.add(_activityInstanceId);
            if (_endTimeMs > _m_expectedEndTimeMs)
                _m_expectedEndTimeMs = _endTimeMs;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 处理活动实例关闭事件
     * 将活动实例ID从已开启集合中移除，表示该活动实例停止使用此礼包
     * @param _activityInstanceId 活动实例ID
     */
    public void onActivityInstanceClosed(long _activityInstanceId)
    {
        _lock();
        try{
            _m_openedActivityInstanceList.remove(_activityInstanceId);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 是否有活动开启
     * @return
     */
    public boolean isEmpty()
    {
        _lock();
        try{
            return _m_openedActivityInstanceList.isEmpty();
        }finally
        {
            _unlock();
        }
    }
}
