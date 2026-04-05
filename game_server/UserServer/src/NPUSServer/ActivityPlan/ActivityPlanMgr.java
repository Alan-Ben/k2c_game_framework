package NPUSServer.ActivityPlan;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Enum.EUsParam;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Activity.RefActivityPlan;
import NPUSServer.NPUserServer;
import USDB.Bo.ActivityPlanBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class ActivityPlanMgr implements _IALSynTask
{
    private NPUserServer _m_server;
    private List<ActivityPlanInfo> _m_planList;
    private MutexAtom _m_mutex;

    public ActivityPlanMgr(NPUserServer _server)
    {
        _m_server = _server;
        _m_planList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    /**
     * 初始化
     * @return
     */
    public boolean s_init()
    {
        List<ActivityPlanBO> boList = _m_server.getBM().getBM(ActivityPlanBO.class).s_findAll();
        if (boList == null)
            return false;

        for (ActivityPlanBO bo : boList)
        {
            ActivityPlanInfo info = new ActivityPlanInfo(this, bo);
            _m_planList.add(info);
        }

        //按照开始时间排序
        _m_planList.sort(Comparator.comparingLong(ActivityPlanInfo::getStartTimeMs));
        return true;
    }

    /**
     * 查询计划
     */
    public ActivityPlanInfo lookPlanInfo(long _planId)
    {
        _lock();
        try
        {
            for (ActivityPlanInfo info : _m_planList)
            {
                if (info.getPlanId() == _planId)
                    return info;
            }
        } finally
        {
            _unlock();
        }
        return null;
    }

    /**
     * tick
     */
    @Override
    public void run()
    {
        long nowTimeMs = CommonFunc.getNowTimeMS();

        //检查是否需要初始化
        if (_m_server.getUSParams().getParam(EUsParam.IS_ACTIVITY_SCHEDULE_INIT) == 0)
        {
            long serverStartDateMs;
            //获取开服时间
            if (!_m_server.getStartDate().isEmpty())
            {
                //查询平台配置
                serverStartDateMs = CommonFunc.simpleDateFormatTimeMs(_m_server.getStartDate(), "yyyy-MM-dd");
            } else if (_m_server.getUSParams().getParam(EUsParam.GM_SERVER_START_DATE) != 0)
            {
                //查询GM命令配置的服务器参数
                serverStartDateMs = CommonFunc.getZeroFromTimeTag((int) _m_server.getUSParams().getParam(EUsParam.GM_SERVER_START_DATE));
            } else
            {
                //没有配置则不处理
                ALSynTaskManager.getInstance().regTask(this, 5000);
                return;
            }

            //如果开服时间大于当前时间, 则不处理
            if (serverStartDateMs > nowTimeMs)
            {
                ALSynTaskManager.getInstance().regTask(this, 5000);
                return;
            }

            List<RefActivityPlan> refList = RefActivityPlan.getMgr().getList();
            for (RefActivityPlan ref : refList)
            {
                for (Long activityId : ref.activity_list)
                {
                    ActivityPlanBO bo = new ActivityPlanBO();
                    bo.setPlanRefId(_m_server.getBM(), ref.Id());
                    bo.setActivityId(_m_server.getBM(), activityId);
                    long startTimeMs = serverStartDateMs + (ref.start_day - 1) * CommonFunc.DAY_SEC * 1000L;
                    bo.setStartMs(_m_server.getBM(), startTimeMs);
                    long endTimeMs = startTimeMs + ref.duration_hour * CommonFunc.HOUR_SEC * 1000L;
                    bo.setEndMs(_m_server.getBM(), endTimeMs);
                    long closeTimeMs = endTimeMs + ref.rewarding_duration_sec * 1000L;
                    bo.setCloseMs(_m_server.getBM(), closeTimeMs);
                    bo.insert(_m_server.getBM());

                    _m_planList.add(new ActivityPlanInfo(this, bo));
                }
            }

            _m_server.getUSParams().setParam(EUsParam.IS_ACTIVITY_SCHEDULE_INIT, 1);
        }

        //检查是否需要执行
        List<ActivityPlanInfo> needDealList = new ArrayList<>();
        _lock();
        try
        {
            for (ActivityPlanInfo info : _m_planList)
            {
                if (!info.needDeal(nowTimeMs))
                    continue;

                needDealList.add(info);
            }
        } finally
        {
            _unlock();
        }

        //执行需要处理的活动
        for (ActivityPlanInfo scheduleInfo : needDealList)
        {
            scheduleInfo.deal();
        }

        //注册下次执行
        ALSynTaskManager.getInstance().regTask(this, 5000);
    }

    /**
     * 清除活动计划
     */
    public void cmdClearAllPlan()
    {
        _lock();
        try
        {
            _m_planList.clear();
            _m_server.getBM().getBM(ActivityPlanBO.class).delAll();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置活动计划禁用状态
     * @return
     */
    public Result cmdSetPlanDisableState(long _planId, boolean _state)
    {
        ActivityPlanInfo planInfo = lookPlanInfo(_planId);
        if (planInfo == null)
            return CommErr.PARAM_ERROR;

        planInfo.setDisableState(_state);

        return Result.SUCC;
    }

    /**
     * 修改活动计划
     * @param _planId
     * @param _startTime
     * @param _endTime
     * @param _settleTime
     * @param _closeTime
     * @return
     */
    public Result cmdModifyPlan(long _planId, long _startTime, long _endTime, long _settleTime, long _closeTime)
    {
        ActivityPlanInfo planInfo = lookPlanInfo(_planId);
        if (planInfo == null)
            return CommErr.PARAM_ERROR;

        return planInfo.modify(_startTime, _endTime, _closeTime);
    }

    /**
     * 删除计划
     * @param _planId
     * @return
     */
    public Result cmdDelPlan(long _planId)
    {
        ActivityPlanInfo planInfo = lookPlanInfo(_planId);
        if (planInfo == null)
            return CommErr.PARAM_ERROR;

        _lock();
        try
        {
            _m_planList.remove(planInfo);
            planInfo.discard();
        } finally
        {
            _unlock();
        }

        return Result.SUCC;
    }

    /**
     * 增加活动计划
     * @param _activityId
     * @param _startTime
     * @param _endTime
     * @param _closeTime
     * @return
     */
    public Result cmdAddPlan(long _activityId, long _startTime, long _endTime, long _closeTime)
    {
        //增加活动计划
        ActivityPlanBO bo = new ActivityPlanBO();
        bo.setActivityId(_m_server.getBM(), _activityId);
        bo.setStartMs(_m_server.getBM(), _startTime);
        bo.setEndMs(_m_server.getBM(), _endTime);
        bo.setCloseMs(_m_server.getBM(), _closeTime);
        bo.insert(_m_server.getBM());

        ActivityPlanInfo planInfo = new ActivityPlanInfo(this, bo);
        _lock();
        try
        {
            _m_planList.add(planInfo);
            _m_planList.sort(Comparator.comparingLong(ActivityPlanInfo::getStartTimeMs));
        } finally
        {
            _unlock();
        }

        return Result.SUCC;

    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (ActivityPlanInfo planInfo : _m_planList)
        {
            sb.append(planInfo.toString()).append("\n");
        }
        return sb.toString();
    }
}
