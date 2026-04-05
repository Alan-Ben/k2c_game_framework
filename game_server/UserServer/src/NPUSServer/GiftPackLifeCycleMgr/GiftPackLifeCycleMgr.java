package NPUSServer.GiftPackLifeCycleMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import NPCommon.Util.Delegate.ADelegateOne;
import NPUSServer.NPUserServer;
import USDB.Bo.GiftPackLifeCycleDataBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 礼包生命周期管理器
 * 负责管理礼包在活动实例生命周期中的状态变化
 * 跟踪礼包的开启和关闭状态，与活动实例绑定
 */
public class GiftPackLifeCycleMgr
{
    // 服务器引用
    private NPUserServer _m_server;

    // 礼包生命周期数据映射表，key为礼包ID，value为生命周期数据
    private Map<Long, GiftPackLifeCycleData> _m_lifeCycleDataMap;

    // 礼包生命周期结束触发器 参数是关闭的生命周期实例id列表
    private ADelegateOne<List<Long>> _m_giftPackClosedTrigger;

    // 锁对象
    private MutexObject _m_mutex;

    /**
     * 私有构造函数，初始化管理器
     * @param _server 服务器实例
     */
    public GiftPackLifeCycleMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_lifeCycleDataMap = new HashMap<>();
        _m_giftPackClosedTrigger = new ADelegateOne<>(this);
        _m_mutex = new MutexObject();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 获取服务器引用
     * @return 服务器实例
     */
    public NPUserServer getServer()
    {
        return _m_server;
    }

    /**
     * 初始化礼包生命周期管理器
     * @return
     */
    public boolean initFromDB()
    {
        List<GiftPackLifeCycleDataBO> giftPackLifeCycleDataBOList = _m_server.getBM().getBM(GiftPackLifeCycleDataBO.class).s_findAll();
        if (giftPackLifeCycleDataBOList == null)
            return false;

        for (GiftPackLifeCycleDataBO bo : giftPackLifeCycleDataBOList)
        {
            GiftPackLifeCycleData data = new GiftPackLifeCycleData(bo);
            _m_lifeCycleDataMap.put(bo.getGiftPackId(), data);
        }

        return true;
    }

    /**
     * 获取礼包生命周期管理器实例
     * @return
     */
    public ADelegateOne<List<Long>> getGiftPackClosedTrigger()
    {
        return _m_giftPackClosedTrigger;
    }

    /**
     * 查找指定礼包的生命周期数据
     * @param _giftPackId 礼包ID
     * @return 生命周期数据，不存在时返回null
     */
    public GiftPackLifeCycleData lookup(long _giftPackId)
    {
        _lock();
        try
        {
            return _m_lifeCycleDataMap.get(_giftPackId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保获取指定礼包的生命周期数据，不存在时自动创建
     * @param _giftPackId 礼包ID
     * @return 生命周期数据，保证非null
     */
    public GiftPackLifeCycleData ensure(long _giftPackId)
    {
        _lock();
        try
        {
            GiftPackLifeCycleData data = _m_lifeCycleDataMap.get(_giftPackId);
            if (data == null)
            {
                GiftPackLifeCycleDataBO bo = new GiftPackLifeCycleDataBO();
                bo.setGiftPackId(_m_server.getBM(), _giftPackId);
                bo.insert(_m_server.getBM());

                data = new GiftPackLifeCycleData(bo);
                _m_lifeCycleDataMap.put(_giftPackId, data);
            }
            return data;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否可以购买
     * @param _giftPackId 礼包ID
     * @return 如果礼包存在且活动开启，则返回true，表示可以购买
     */
    public long canPurchase(long _giftPackId)
    {
        _lock();
        try
        {
            GiftPackLifeCycleData data = lookup(_giftPackId);
            // 如果数据不存在或没有开启的活动实例，则不能购买
            if (data == null || data.isEmpty())
                return -1;

            return data.getDbId();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 处理活动实例开启事件
     * 当活动实例开启时，通知所有相关礼包进入开启状态
     * @param _activityInstanceId 活动实例ID
     * @param _giftPackIdList     相关的礼包ID列表
     * @param _endTimeMs
     */
    public void onActivityInstanceOpened(long _activityInstanceId, List<Long> _giftPackIdList, long _endTimeMs)
    {
        _lock();
        try
        {
            for (long giftPackId : _giftPackIdList)
            {
                GiftPackLifeCycleData data = ensure(giftPackId);
                data.onActivityInstanceOpened(_activityInstanceId, _endTimeMs);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 处理活动实例结算事件
     * 当活动实例结算时，通知所有相关礼包进入关闭状态
     * @param _activityInstanceId 活动实例ID
     * @param _giftPackIdList     相关的礼包ID列表
     */
    public void onActivityInstanceSetting(long _activityInstanceId, List<Long> _giftPackIdList)
    {
        _lock();
        try
        {
            // 需要移除的礼包实例ID列表
            List<Long> needRemoveDbIdList = null;

            // 遍历所有礼包ID，处理生命周期数据
            for (long giftPackId : _giftPackIdList)
            {
                GiftPackLifeCycleData data = lookup(giftPackId);
                if (data == null)
                    continue;

                data.onActivityInstanceClosed(_activityInstanceId);

                // 如果生命周期数据为空，则从映射表中移除
                if (data.isEmpty())
                {
                    _m_lifeCycleDataMap.remove(giftPackId);

                    if (needRemoveDbIdList == null)
                        needRemoveDbIdList = new ArrayList<>();

                    needRemoveDbIdList.add(data.getDbId());
                }
            }

            // 如果有需要移除的数据库ID列表，则从数据库中删除
            if (needRemoveDbIdList != null && !needRemoveDbIdList.isEmpty())
            {
                _m_giftPackClosedTrigger.onAsyncEvent(needRemoveDbIdList);
                _m_server.getBM().getBM(GiftPackLifeCycleDataBO.class).delAllInList("id", needRemoveDbIdList);
            }
        } finally
        {
            _unlock();
        }

    }
}
