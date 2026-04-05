package NPUSServer.NPUSUserMgr.UserComp.ActivityFund;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityFundObj.ActivityFund_TaskInfo;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.ActivityFund.RefActivityFundTask;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerActivityFundBO;
import USDB.Bo.PlayerActivityFundTaskBO;
import USLOGDB.Bo.LogActivityFundTaskCountBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动基金任务信息
 * <p>
 * 主要功能：
 * 1. 管理单个任务的计数和完成状态
 * 2. 懒加载数据库插入（首次有数据变化时才插入）
 * 3. 计数达到目标时自动完成并返回分数
 * 4. 支持增量模式和覆盖模式两种计数方式
 * <p>
 * 线程安全：通过玩家锁保证线程安全
 */
public class ActivityFundTaskInfo implements _IHandlerHolder
{
    // 所属基金信息
    private ActivityFundInfo _m_fundInfo;

    // 数据库ID，0表示未插入数据库
    private long _m_dbId;

    // 任务ID
    private long _m_taskId;

    // 当前计数
    private long _m_currentCount;

    // 已完成次数
    private int _m_finishedTimes;

    // 配置缓存
    private RefActivityFundTask _m_refTask;

    // 事件监听器列表
    private List<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;

    /**
     * 构造函数 - 用于新建任务
     * @param _fundInfo 所属基金信息
     * @param _ref      任务配置
     */
    public ActivityFundTaskInfo(ActivityFundInfo _fundInfo, RefActivityFundTask _ref)
    {
        _m_fundInfo = _fundInfo;
        _m_taskId = _ref.task_id;
        _m_dbId = 0;
        _m_currentCount = 0;
        _m_finishedTimes = 0;
        _m_evtEntryList = new ArrayList<>();

        // 缓存配置对象
        _m_refTask = _ref;

        // 注册事件监听
        regEvtEntry();
    }

    /**
     * 构造函数 - 用于从数据库加载
     * @param _fundInfo 所属基金信息
     * @param _taskRef  任务配置
     * @param _bo       数据库BO对象
     */
    public ActivityFundTaskInfo(ActivityFundInfo _fundInfo, RefActivityFundTask _taskRef, PlayerActivityFundTaskBO _bo)
    {
        this(_fundInfo, _taskRef);
        _m_dbId = _bo.getId();
        _m_currentCount = _bo.getCurrentCount();
        _m_finishedTimes = _bo.getFinishedTimes();
    }

    // Getters
    public long getTaskId()
    {
        return _m_taskId;
    }

    public long getCurrentCount()
    {
        return _m_currentCount;
    }

    public int getFinishedTimes()
    {
        return _m_finishedTimes;
    }

    public RefActivityFundTask getRefTask()
    {
        return _m_refTask;
    }

    public long getDbId()
    {
        return _m_dbId;
    }

    // 便利方法
    private BM getBM()
    {
        return _m_fundInfo.getBM();
    }

    private NPUserServer getUSServer()
    {
        return _m_fundInfo.getComponent().getUSServer();
    }

    public NPUSUserData getUserData()
    {
        return _m_fundInfo.getComponent().getUserData();
    }

    /**
     * 尝试在数据库创建记录（懒加载）
     * @return true=首次创建，false=已存在
     */
    public boolean tryCreateInDB()
    {
        getUserData().lockUser();
        try
        {
            if (_m_dbId != 0)
                return false; // 已插入

            // 创建BO对象并插入数据库
            PlayerActivityFundTaskBO bo = new PlayerActivityFundTaskBO();
            bo.setCid(getBM(), _m_fundInfo.getComponent().getUserData().getCid());
            bo.setFundRecordId(getBM(), _m_fundInfo.getDbId());
            bo.setTaskId(getBM(), _m_taskId);
            bo.setCurrentCount(getBM(), _m_currentCount);
            bo.setFinishedTimes(getBM(), _m_finishedTimes);
            bo.insert(getBM());

            _m_dbId = bo.getId();
            return true; // 首次插入返回true
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 更新任务计数
     * <p>
     * 执行流程：
     * 1. 根据 is_set 判断是增量还是覆盖
     * 2. 更新计数值
     * 3. 检查是否达到完成条件
     * 4. 如果完成则增加完成次数并返回分数
     * 5. 保存到数据库
     * @param _count   计数值（增量或覆盖值）
     * @param _context 操作上下文
     * @return 本次获得的分数（如果完成任务）
     */
    public long updateCount(long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_refTask == null)
                return 0;

            // 记录变动前状态
            long countBefore = _m_currentCount;
            int finishedTimesBefore = _m_finishedTimes;

            // 根据 is_set 判断是增量还是覆盖模式
            if (_m_refTask.is_set)
            {
                // 覆盖模式
                _m_currentCount = _count;
            } else
            {
                // 增量模式
                _m_currentCount += _count;
            }

            // 检查并完成任务
            long gainScore = checkAndFinish(_context);

            // 记录任务计数变更日志
            recordTaskCountLog(countBefore, _m_currentCount, finishedTimesBefore, _m_finishedTimes, _context);

            // 保存到数据库
            if (!tryCreateInDB())
            {
                // 已存在记录，使用增量更新
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("current_count", _m_currentCount);
                updateValue.addValueObj("finished_times", _m_finishedTimes);
                getBM().getBM(PlayerActivityFundTaskBO.class).update("id", _m_dbId, updateValue);
            }

            return gainScore;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查并完成任务
     * 执行流程：
     * 1. 检查计数是否达到完成条件
     * 2. 检查是否已达到完成次数上限
     * 3. 增加完成次数并重置计数
     * 4. 返回本次获得的分数
     * @param _context 操作上下文
     * @return 本次获得的分数
     */
    private long checkAndFinish(NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            if (_m_refTask == null)
                return 0;

            // 检查计数是否达到完成条件
            if (_m_currentCount < _m_refTask.done_task_count)
                return 0;

            // 检查是否已达到完成次数上限
            if (_m_finishedTimes >= _m_refTask.task_finish_limit)
                return 0;

            // 增加完成次数
            _m_finishedTimes++;

            // 重置计数
            _m_currentCount = 0;

            // 返回获得的分数
            return _m_refTask.gain_score;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 注册事件监听
     * 监听任务触发事件
     */
    protected void regEvtEntry()
    {
        getUserData().lockUser();
        try
        {
            if (_m_refTask == null)
                return;

            if (_m_refTask.trigger_event == null || _m_refTask.trigger_event.isEmpty())
                return;

            EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(
                    _m_refTask.trigger_event.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("ActivityFundTaskInfo.regEvtEntry - event not found: taskId={}, event={}",
                        _m_taskId, _m_refTask.trigger_event, new Exception());
                return;
            }

            // 注册事件监听
            NPHandlerEntry<NPUSUserData> evtEntry = _m_fundInfo.getComponent().getUserData().getEventHandlerMgr().regHandler(
                    eventMeta.getEventId(), this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
                    {
                        @Override
                        public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                        {
                            NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                            onLogicEvt(_evt, context);
                        }
                    });

            _m_evtEntryList.add(evtEntry);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 取消事件监听
     */
    public void unregEvtEntry()
    {
        getUserData().lockUser();
        try
        {
            if (_m_evtEntryList.isEmpty()) return;

            for (NPHandlerEntry<NPUSUserData> entry : _m_evtEntryList)
            {
                _m_fundInfo.getComponent().getUserData().getEventHandlerMgr().unregHandler(entry);
            }
            _m_evtEntryList.clear();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 处理逻辑事件
     * @param _evt     事件对象
     * @param _context 操作上下文
     */
    protected void onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 先检查并重置过期的任务数据
            _m_fundInfo.checkAndResetExpiredTasks();

            NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);

            //检查触发条件
            if (!NPPlayerConditionDealerMgr.IsEnable(
                    _m_refTask.trigger_condition, _m_fundInfo.getComponent().getUserData(), varInfo))
                return;

            //增加任务计数
            long gainScore = updateCount(_evt.getValue(_m_refTask.trigger_count_rate), _context);

            // 如果任务完成则通知父对象增加分数
            if (gainScore > 0)
            {
                _m_fundInfo.addTaskScore(gainScore, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 销毁任务信息
     */
    public void discard()
    {
        getUserData().lockUser();
        try
        {
            if (_m_dbId != 0)
                getBM().getBM(PlayerActivityFundBO.class).delAll("id", _m_dbId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 转换为协议对象
     * @return 协议对象
     */
    public ActivityFund_TaskInfo toProto()
    {
        getUserData().lockUser();
        try
        {
            ActivityFund_TaskInfo obj = new ActivityFund_TaskInfo();
            obj.setTaskId(_m_taskId);
            obj.setCurrentCount(_m_currentCount);
            obj.setFinishedTimes(_m_finishedTimes);
            return obj;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    // ========== GM命令相关方法 ==========

    /**
     * 设置任务计数（GM命令使用）
     * <p>
     * 执行流程：
     * 1. 直接设置计数值（覆盖模式）
     * 2. 检查并完成任务
     * 3. 保存到数据库
     * @param _count   计数值
     * @param _context 操作上下文
     */
    public void setCount(long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 记录变动前状态
            long countBefore = _m_currentCount;
            int finishedTimesBefore = _m_finishedTimes;

            // 设置计数
            _m_currentCount = _count;

            // 检查并完成任务
            long gainScore = checkAndFinish(_context);

            // 记录任务计数变更日志
            recordTaskCountLog(countBefore, _m_currentCount, finishedTimesBefore, _m_finishedTimes, _context);

            // 保存到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("current_count", _m_currentCount);
                updateValue.addValueObj("finished_times", _m_finishedTimes);
                getBM().getBM(PlayerActivityFundTaskBO.class).update("id", _m_dbId, updateValue);
            }

            // 如果任务完成则通知父对象增加分数
            if (gainScore > 0)
            {
                _m_fundInfo.addTaskScore(gainScore, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加任务计数（GM命令使用）
     * <p>
     * 执行流程：
     * 1. 增加计数值
     * 2. 检查并完成任务
     * 3. 保存到数据库
     * @param _count   增加的计数
     * @param _context 操作上下文
     */
    public void addCount(long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            // 记录变动前状态
            long countBefore = _m_currentCount;
            int finishedTimesBefore = _m_finishedTimes;

            // 增加计数
            _m_currentCount += _count;

            // 检查并完成任务
            long gainScore = checkAndFinish(_context);

            // 记录任务计数变更日志
            recordTaskCountLog(countBefore, _m_currentCount, finishedTimesBefore, _m_finishedTimes, _context);

            // 保存到数据库
            if (!tryCreateInDB())
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("current_count", _m_currentCount);
                updateValue.addValueObj("finished_times", _m_finishedTimes);
                getBM().getBM(PlayerActivityFundTaskBO.class).update("id", _m_dbId, updateValue);
            }

            // 如果任务完成则通知父对象增加分数
            if (gainScore > 0)
            {
                _m_fundInfo.addTaskScore(gainScore, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 记录任务计数变更日志
     *
     * @param _countBefore 变动前计数
     * @param _countAfter 变动后计数
     * @param _finishedTimesBefore 变动前完成次数
     * @param _finishedTimesAfter 变动后完成次数
     * @param _context 操作上下文
     */
    private void recordTaskCountLog(long _countBefore, long _countAfter,
                                     int _finishedTimesBefore, int _finishedTimesAfter,
                                     NPPlayerContext _context)
    {
        try
        {
            // 创建日志BO对象
            LogActivityFundTaskCountBO logBO = new LogActivityFundTaskCountBO();

            // 设置玩家CID
            logBO.setCid(getBM(), _m_fundInfo.getComponent().getUserData().getCid());

            // 设置基金ID
            logBO.setFundId(getBM(), _m_fundInfo.getFundId());

            // 设置活动实例ID
            logBO.setActivityInstanceId(getBM(), _m_fundInfo.getActivityInstanceId());

            // 设置任务ID
            logBO.setTaskId(getBM(), _m_taskId);

            // 设置计数变动
            logBO.setCountBefore(getBM(), _countBefore);
            logBO.setCountAfter(getBM(), _countAfter);
            logBO.setChangeValue(getBM(), _countAfter - _countBefore);

            // 设置完成次数变动
            logBO.setFinishedTimesBefore(getBM(), _finishedTimesBefore);
            logBO.setFinishedTimesAfter(getBM(), _finishedTimesAfter);

            // 使用CommLogDB记录日志
            CommLogDB.log(getBM(), logBO, _context);
        }
        catch (Exception e)
        {
            CommLog.error("ActivityFundTaskInfo.recordTaskCountLog - failed to record task count log: cid={}, fundId={}, taskId={}, error={}",
                    _m_fundInfo.getComponent().getUserData().getCid(), _m_fundInfo.getFundId(), _m_taskId, e.getMessage(), e);
        }
    }

    /**
     * 重置任务到初始状态
     * <p>
     * 执行流程：
     * 1. 重置当前计数为0
     * 2. 重置完成次数为0
     * 3. 重置数据库ID为0（标记为未插入状态）
     * <p>
     * 注意：此方法只重置内存对象，不操作数据库
     * 数据库记录由 ActivityFundInfo.checkAndResetExpiredTasks() 批量删除
     */
    public void resetToInitState()
    {
        getUserData().lockUser();
        try
        {
            _m_currentCount = 0;
            _m_finishedTimes = 0;
            _m_dbId = 0; // 重置为未插入状态，下次有数据变化时会重新插入
        } finally
        {
            getUserData().unlockUser();
        }
    }
}