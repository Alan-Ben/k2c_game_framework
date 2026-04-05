package NPUSServer.UsActivityScheduleMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ActivityObj.Activity_HotRefInfo;
import Common.Common_IntList;
import Common.ScheduleObj.Schedule_UsPushData;
import GS2GC.p017_ActivityOp.GS2GC_017_062_OnActivityHotRefInfoChg;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.UsHotRefDataMgr.ActivityHotRefDataMgr;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsActivityScheduleBO;

import java.util.ArrayList;
import java.util.List;

/**
 * US的排期数据管理
 * <p>
 * 数据来自SS服务器下发的排期数据
 * 排期数据主键 = zoneId + scheduleId
 * 每个排期数据只包含一个活动，收到排期立刻进行活动注册
 * 活动结束时，同时上报SS服务器，收到回包后再进行排期数据移除
 * @author mj
 */
public class UsActivityScheduleMgr
{
    private NPUserServer _m_usServer;
    //活动排期数据列表
    private ArrayList<UsActivityScheduleInfo> _m_usScheduleList;
    //锁对象
    private MutexObject _m_mutex;

    public UsActivityScheduleMgr(NPUserServer _usServer)
    {
        _m_usServer = _usServer;

        _m_usScheduleList = new ArrayList<>();

        _m_mutex = new MutexObject();
    }

    public NPUserServer getUSServer()
    {
        return _m_usServer;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public boolean initFromDB()
    {
        List<UsActivityScheduleBO> boList = getUSServer().getBM().getBM(UsActivityScheduleBO.class).s_findAll();
        if (null == boList)
        {
            USLog.error(_m_usServer, "UsZoneActivityScheduleMgr init bo fail.");
            return false;
        }

        for (UsActivityScheduleBO bo : boList)
        {
            UsActivityScheduleInfo info = new UsActivityScheduleInfo(getUSServer(), bo);
            _m_usScheduleList.add(info);

            //注册活动所需的资源
            if (info.getResFileName() != null && !info.getResFileName().isEmpty())
                ActivityHotRefDataMgr.getInstance().regHotRefGroup(info.getUsGroupId(),
                        info.getResFileName(), info.getResFileMd5(), info.getResFileDir(), info.getSubmitCount(), true);
        }

        return true;
    }

    /**
     * 初始化完成调用
     */
    public void onInited()
    {
        //进行一次排期数据检查
        checkAllSchedule();

        //获取当前SS上已下发的排期数据
        ALSynTaskManager.getInstance().regTask(new UsActivityGetScheduleListTask(_m_usServer));
    }

    /**
     * 对所有排期数据进行检查
     * 1. 需要上报已经完成的排期数据
     */
    public void checkAllSchedule()
    {
        List<UsActivityScheduleInfo> needDealList = new ArrayList<>();

        _lock();
        try
        {
            //检查所有排期数据
            for (int i = 0; i < _m_usScheduleList.size(); i++)
            {
                UsActivityScheduleInfo info = _m_usScheduleList.get(i);
                if (null == info)
                    continue;

                if (!info.isRegged()) //尚未注册的活动，需要开启注册
                {
                    needDealList.add(info);
                }

                //判断排期状态，尝试恢复未完成的任务
                if (info.isDone())
                {
                    //活动完成，需要向SS上报数据
                    ALSynTaskManager.getInstance().regTask(new UsActivityReportDoneToSsTask(getUSServer(), info));
                }
            }
        } finally
        {
            _unlock();
        }

        for (UsActivityScheduleInfo scheduleInfo : needDealList)
        {
            try
            {
                scheduleInfo.startToRegActivity(NPPlayerContext.createNew(ENPGameEvent.ZONE_ACTIVITY_SCHEDULE_REG));
            } catch (Exception e)
            {
                USLog.error(_m_usServer, "", e);
            }
        }
    }

    /**
     * 查找指定的排期对象
     * @param _scheduleId
     * @return
     */
    public UsActivityScheduleInfo lookupSchedule(long _scheduleId)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_usScheduleList.size(); i++)
            {
                UsActivityScheduleInfo info = _m_usScheduleList.get(i);
                if (null == info)
                    continue;

                if (info.getScheduleId() == _scheduleId)
                    return info;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找指定的排期对象 通过活动实例id
     * @param _activityInstanceId
     * @return
     */
    public UsActivityScheduleInfo lookupScheduleByActivityInstanceId(long _activityInstanceId)
    {
        _lock();

        try
        {
            for (int i = 0; i < _m_usScheduleList.size(); i++)
            {
                UsActivityScheduleInfo info = _m_usScheduleList.get(i);
                if (null == info)
                    continue;

                if (info.getActivityInstanceId() == _activityInstanceId)
                    return info;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找指定的排期对象
     * @param _scheduleId
     * @return
     */
    public void setScheduleCanSendReward(long _scheduleId)
    {
        UsActivityScheduleInfo scheduleInfo = lookupSchedule(_scheduleId);
        if (null == scheduleInfo)
        {
            USLog.error(_m_usServer, "UsZoneActivityScheduleMgr noticeScheduleCanSendReward fail, schedule:{} not found.", _scheduleId);
            return;
        }

        scheduleInfo.setCanSendReward();

        USLog.info(getUSServer(), "UsZoneActivityScheduleMgr noticeScheduleCanSendReward succ, schedule:{}.", _scheduleId);
    }

    /**
     * 收到新的排期信息推送
     * @param _usSchedule
     */
    public void onScheduleInfoPush(Schedule_UsPushData _usSchedule)
    {
        UsActivityScheduleInfo usScheduleInfo = lookupSchedule(_usSchedule.getScheduleId());
        if (null == usScheduleInfo)
        {
            //注册新的排期
            regActivitySchedule(_usSchedule);
        }else
        {
            _lock();
            try{
                if (usScheduleInfo.getSubmitCount() >= _usSchedule.getSubmitCount())
                {
                    //提交次数变更异常，忽略本次更新
                    USLog.warn(_m_usServer, "UsZoneActivityScheduleMgr onScheduleInfoPush fail," +
                                    " scheduleId:{} submitCount invalid, curSubmitCount:{} newSubmitCount:{}",
                            _usSchedule.getScheduleId(), usScheduleInfo.getSubmitCount(), _usSchedule.getSubmitCount());
                    return;
                }

                //更新排期信息
                usScheduleInfo.updateScheduleRes(_usSchedule);
            }finally
            {
                _unlock();
            }

            ActivityHotRefDataMgr.getInstance().regHotRefGroup(_usSchedule.getUsGroupId(),
                    _usSchedule.getResFile(), _usSchedule.getResFileMd5(), _usSchedule.getResFileDir(),
                    _usSchedule.getSubmitCount(), false);

            //广播热更信息变更
            getUSServer().getUsUserMgr().broadCastMessage(new GS2GC_017_062_OnActivityHotRefInfoChg(usScheduleInfo.makeHotRefInfo()));
        }
    }

    /**
     * 注册新的活动排期，需要检查是否已经存在
     * @param _usSchedule
     */
    public void regActivitySchedule(Schedule_UsPushData _usSchedule)
    {
        //已经存在，无需重复注册
        UsActivityScheduleInfo info = lookupSchedule(_usSchedule.getScheduleId());
        if (null != info)
            return;

        _lock();
        try
        {
            BM bmObj = getUSServer().getBM();

            UsActivityScheduleBO bo = new UsActivityScheduleBO();
            bo.setScheduleId(bmObj, _usSchedule.getScheduleId());
            bo.setUsGroupId(bmObj, _usSchedule.getUsGroupId());
            bo.setCrossInstanceId(bmObj, _usSchedule.getCrossInstanceId());
            bo.setGameLogicInstanceId(bmObj, _usSchedule.getGameLogicInstanceId());
            bo.setActivityId(bmObj, _usSchedule.getActivity().getActivityId());
            bo.setStartTimeMs(bmObj, _usSchedule.getActivity().getStartTimeMs());
            bo.setEndTimeMs(bmObj, _usSchedule.getActivity().getEndTimeMs());
            bo.setCloseTimeMs(bmObj, _usSchedule.getActivity().getCloseTimeMs());
            bo.setUsIdList(bmObj, CommonFunc.ByteBfferToBytes(new Common_IntList(_usSchedule.getUsIdList()).makePackage()));
            bo.setResFileName(bmObj, _usSchedule.getResFile());
            bo.setResFileMd5(bmObj, _usSchedule.getResFileMd5());
            bo.setResFileDir(bmObj, _usSchedule.getResFileDir());
            bo.setSubmitCount(bmObj, _usSchedule.getSubmitCount());
            bo.insert(bmObj);

            info = new UsActivityScheduleInfo(getUSServer(), bo);
            _m_usScheduleList.add(info);

            USLog.info(getUSServer(), "US RegActivitySchedule GroupId:{} ScheduleId:{} CrsInstanceId:{} ActivityId:{} ActivityTime:{}-{}-{}"
                    , bo.getUsGroupId(), bo.getScheduleId(), bo.getCrossInstanceId()
                    , _usSchedule.getActivity().getActivityId(), _usSchedule.getActivity().getStartTimeMs(),
                    _usSchedule.getActivity().getEndTimeMs(), _usSchedule.getActivity().getCloseTimeMs());
        } finally
        {
            _unlock();
        }

        try
        {
            info.startToRegActivity(NPPlayerContext.createNew(ENPGameEvent.ZONE_ACTIVITY_SCHEDULE_REG));
        } catch (Exception e)
        {
            USLog.error(_m_usServer, "", e);
        }

        //注册活动所需的资源
        if (!_usSchedule.getResFile().isEmpty())
        {
            ActivityHotRefDataMgr.getInstance().regHotRefGroup(_usSchedule.getUsGroupId(),
                    _usSchedule.getResFile(), _usSchedule.getResFileMd5(),
                    _usSchedule.getResFileDir(), _usSchedule.getSubmitCount(), false);

            //广播热更信息变更
            getUSServer().getUsUserMgr().broadCastMessage(new GS2GC_017_062_OnActivityHotRefInfoChg(info.makeHotRefInfo()));
        }
    }

    /**
     * 移除排期数据
     * @param _scheduleId
     */
    public void removeActivitySchedule(long _scheduleId)
    {
        List<Long> needDelRefGroupList = new ArrayList<>();

        _lock();
        try
        {
            for (int i = _m_usScheduleList.size() - 1; i >= 0; i--)
            {
                UsActivityScheduleInfo info = _m_usScheduleList.get(i);
                if (null == info)
                    continue;

                if (info.getScheduleId() == _scheduleId)
                {
                    _m_usScheduleList.remove(i);
                    info.discard();

                    if (info.getResFileName() != null && !info.getResFileName().isEmpty())
                        needDelRefGroupList.add(info.getUsGroupId());
                }
            }
        } finally
        {
            _unlock();
        }

        //反注册活动所需的资源
        for (Long usGroupId : needDelRefGroupList)
        {
            ActivityHotRefDataMgr.getInstance().unregHotRefGroup(usGroupId);
        }
    }

    /**
     * 激活指定排期的热更配表
     * <p>
     * 执行流程：
     * 1. 根据活动实例id查找排期信息
     * 2. 获取排期对应的usGroupId
     * 3. 调用 ActivityHotRefDataMgr 激活对应分组的热更配表
     * @param _activityInstanceId 活动实例id
     * @return true表示激活成功，false表示激活失败（排期不存在或激活失败）
     */
    public boolean activateScheduleHotRef(long _activityInstanceId)
    {
        UsActivityScheduleInfo scheduleInfo = lookupScheduleByActivityInstanceId(_activityInstanceId);
        if (scheduleInfo == null)
        {
            USLog.error(_m_usServer, "activate schedule hot ref failed, schedule not found, activityInstanceId={}", _activityInstanceId);
            return false;
        }

        // 检查排期是否有热更配表资源
        if (scheduleInfo.getResFileName().isEmpty())
            return true;

        // 激活热更配表
        boolean result = ActivityHotRefDataMgr.getInstance().activateHotRefGroup(scheduleInfo.getUsGroupId());
        if (result)
        {
            USLog.info(_m_usServer, "activate schedule hot ref success, scheduleId={}, groupId={}",
                    scheduleInfo.getScheduleId(), scheduleInfo.getUsGroupId());
        } else
        {
            USLog.error(_m_usServer, "activate schedule hot ref failed, scheduleId={}, groupId={}",
                    scheduleInfo.getScheduleId(), scheduleInfo.getUsGroupId());
        }

        return result;
    }

    @Override
    public String toString()
    {
        _lock();

        try
        {
            StringBuilder sb = new StringBuilder();
            sb.append("schedule size:").append(_m_usScheduleList.size());

            for (UsActivityScheduleInfo schedule : _m_usScheduleList)
            {
                if (null == schedule)
                    continue;

                sb.append("\n").append("scheduleId:").append(schedule.getScheduleId())
                        .append(", activityId:").append(schedule.getActivityId())
                        .append(", startTimeMs:").append(schedule.getStartTimeMs())
                        .append(", endTimeMs:").append(schedule.getEndTimeMs())
                        .append(", closeTimeMs:").append(schedule.getCloseTimeMs());
            }

            return sb.toString();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造热更信息列表
     * @return
     */
    public List<Activity_HotRefInfo> makeHotRefList()
    {
        List<Activity_HotRefInfo> hotRefList = new ArrayList<>();
        _lock();
        try
        {
            for (UsActivityScheduleInfo scheduleInfo : _m_usScheduleList)
            {
                if (scheduleInfo.getResFileName().isEmpty())
                    continue;

                hotRefList.add(scheduleInfo.makeHotRefInfo());
            }
        } finally
        {
            _unlock();
        }
        return hotRefList;
    }
}
