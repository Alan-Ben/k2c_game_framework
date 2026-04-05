package NPUSServer.EarningsMarqueeMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.DB.BM.BM;
import USDB.Bo.EarningsMarqueeRecordBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * 赚速跑马灯信息管理类 - 管理单个赚速目标的触发记录
 * <p>
 * 主要功能：
 * 1. 管理特定赚速目标的所有触发记录
 * 2. 检查是否还有触发配额
 * 3. 添加新的触发记录
 * <p>
 * 线程安全：使用内部锁保护
 */
public class EarningsMarqueeInfo
{
    private EarningsMarqueeMgr _m_mgr;
    private MutexAtom _m_mutex;

    // 赚速目标值
    private long _m_earningsGoal;

    // 最大触发次数
    private int _m_maxCount;

    // 已触发cid记录列表
    private List<Long> _m_recordList;

    public EarningsMarqueeInfo(EarningsMarqueeMgr _mgr, long _earningsGoal, int _maxCount)
    {
        _m_mgr = _mgr;
        _m_mutex = new MutexAtom();
        _m_earningsGoal = _earningsGoal;
        _m_maxCount = _maxCount;
        _m_recordList = new ArrayList<>();
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    public long getEarningsGoal()
    {
        return _m_earningsGoal;
    }

    public int getMaxCount()
    {
        return _m_maxCount;
    }

    public int getTriggeredCount()
    {
        _lock();
        try
        {
            return _m_recordList.size();
        } finally
        {
            _unlock();
        }
    }

    public List<Long> getTriggeredCidList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_recordList);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 从数据库加载时添加记录
     */
    public void initAddRecord(EarningsMarqueeRecordBO _bo)
    {
        _lock();
        try
        {
            _m_recordList.add(_bo.getCid());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查玩家是否已经触发过
     */
    public boolean hasTriggered(long _cid)
    {
        _lock();
        try
        {
            return _m_recordList.contains(_cid);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加触发记录
     * @return 是否成功添加
     */
    public boolean addRecord(long _cid)
    {
        _lock();
        try
        {
            // 检查是否已达上限
            if (_m_recordList.size() >= _m_maxCount)
                return false;

            // 检查玩家是否已经触发过
            if (hasTriggered(_cid))
                return false;

            BM bm = _m_mgr.getServer().getBM();

            EarningsMarqueeRecordBO bo = new EarningsMarqueeRecordBO();
            bo.setEarningsGoal(bm, _m_earningsGoal);
            bo.setCid(bm, _cid);
            bo.insert(bm);

            // 添加到内存列表
            _m_recordList.add(_cid);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清除指定玩家的触发记录
     */
    public void cleanRecord(long _cid)
    {
        _lock();
        try
        {
            _m_recordList.remove(_cid);

            HashMap<String, Object> updateValue = new HashMap<>();
            updateValue.put("earnings_goal", _m_earningsGoal);
            updateValue.put("cid", _cid);

            _m_mgr.getServer().getBM().getBM(EarningsMarqueeRecordBO.class).delAll(updateValue);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 清除所有触发记录
     */
    public void cleanRecords()
    {
        _lock();
        try
        {
            _m_recordList.clear();

            _m_mgr.getServer().getBM().getBM(EarningsMarqueeRecordBO.class).delAll("earnings_goal", _m_earningsGoal);
        } finally
        {
            _unlock();
        }
    }
}
