package NPUSServer.StageGoalFirstReachMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.StageGoalObj.StageGoal_BigStepFirstReachInfo;
import NPCommon.DB.BM.BM;
import NPGameRes.Refs.RefGeneral;
import NPGameRes.Refs.StageGoal.RefStageGoalBigStep;
import USDB.Bo.BigStageGoalFirstReachBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 大阶段目标首次达成信息管理类
 * <p>
 * 主要功能：
 * 1. 管理特定大阶段的首次达成记录
 * 2. 提供记录添加和查询功能
 * 3. 负责数据库持久化操作
 * <p>
 * 线程安全：需要外部调用方确保线程安全
 */
public class StageGoalFirstReachInfo
{
    private StageGoalFirstReachMgr _m_mgr;
    private MutexAtom _m_mutex;

    // 大阶段ID
    private long _m_bigStageId;

    // 首次达成记录列表（内存缓存）
    private List<StageGoalFirstReachRecord> _m_goalFirstReachRecordList;

    /**
     * 构造函数 - 初始化大阶段首次达成信息
     * @param _bigStageId 大阶段ID
     */
    public StageGoalFirstReachInfo(StageGoalFirstReachMgr _mgr, long _bigStageId)
    {
        _m_mgr = _mgr;
        _m_mutex = new MutexAtom();
        _m_bigStageId = _bigStageId;
        _m_goalFirstReachRecordList = new ArrayList<>();
    }

    public void _lock()
    {
        _m_mutex.lock();
    }

    public void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 是否已经达成
     * @return
     */
    public boolean isReach()
    {
        return !_m_goalFirstReachRecordList.isEmpty();
    }

    /**
     * 整理
     */
    public void sort()
    {
        _lock();
        try{
            _m_goalFirstReachRecordList.sort(Comparator.comparingLong(StageGoalFirstReachRecord::getDbdId));
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取大阶段ID
     * @return 大阶段ID
     */
    public long getBigStageId()
    {
        return _m_bigStageId;
    }

    /**
     * 初始化添加记录 - 从数据库加载时使用
     * @param _bo
     */
    public void initAddRecord(BigStageGoalFirstReachBO _bo)
    {
        if (_bo == null)
            return;

        _lock();
        try
        {
            StageGoalFirstReachRecord record = new StageGoalFirstReachRecord(this, _bo);
            _m_goalFirstReachRecordList.add(record);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 添加首次达成记录 - 记录玩家首次达成大阶段目标的时间
     * @param _cid         玩家CID
     * @param _name
     * @param _ref
     * @param _reachTimeMs 达成时间毫秒
     */
    public void addRecord(long _cid, String _name, RefStageGoalBigStep _ref, long _reachTimeMs)
    {
        boolean needSend = false;

        _lock();
        try
        {
            // 达成记录已满，忽略新记录
            if (_m_goalFirstReachRecordList.size() >= RefGeneral.Ref().stage_goal_first_reach_detail_list_show_count)
                return;

            // 检查是否已经有数据了
            for (StageGoalFirstReachRecord record : _m_goalFirstReachRecordList)
            {
                if (record == null)
                    continue;

                if (record.makeProto().getCid() == _cid)
                    return;
            }

            BM bm = _m_mgr.getServer().getBM();

            // 创建BO对象并插入数据库
            BigStageGoalFirstReachBO bo = new BigStageGoalFirstReachBO();
            bo.setBigStageId(bm, _m_bigStageId);
            bo.setCid(bm, _cid);
            bo.setReachTimeMs(bm, _reachTimeMs);
            bo.insert(bm);

            // 创建达成记录对象
            StageGoalFirstReachRecord record = new StageGoalFirstReachRecord(this, bo);

            // 添加到内存列表
            _m_goalFirstReachRecordList.add(record);

            if (_m_goalFirstReachRecordList.size() == 1)
                needSend = true;
        } finally
        {
            _unlock();
        }

        // 如果这是第一条记录，广播有首达奖励可领取
        if (needSend)
        {
            ALSynTaskManager.getInstance().regTask(new StageGoalBigStepFirstReachAddTask(_m_mgr.getServer(), _m_bigStageId));

            //跑马灯
            ArrayList<String> paramList = new ArrayList<>();
            paramList.add(_name);
            paramList.add(_ref.title);
            _m_mgr.getServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().marquee_first_reach_big_stage_marquee_id, paramList);
        }
    }

    /**
     * 生成详细信息列表
     * @return
     */
    public List<StageGoal_BigStepFirstReachInfo> makeDetailInfo()
    {
        List<StageGoal_BigStepFirstReachInfo> list = new ArrayList<>();
        _lock();
        try
        {
            for (StageGoalFirstReachRecord record : _m_goalFirstReachRecordList)
            {
                if (record == null)
                    continue;

                list.add(record.makeProto());
            }
        } finally
        {
            _unlock();
        }
        return list;
    }

    /**
     * 生成首个达成信息（如果有）
     * @return
     */
    public StageGoal_BigStepFirstReachInfo makeFirstBaseInfo()
    {
        _lock();
        try
        {
            if (!_m_goalFirstReachRecordList.isEmpty())
            {
                StageGoalFirstReachRecord record = _m_goalFirstReachRecordList.get(0);
                if (record != null)
                    return record.makeProto();
            }
            return null;
        } finally
        {
            _unlock();
        }
    }


}
