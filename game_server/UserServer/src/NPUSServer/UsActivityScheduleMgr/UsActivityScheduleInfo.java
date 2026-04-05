package NPUSServer.UsActivityScheduleMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityObj.Activity_HotRefInfo;
import Common.Common_IntList;
import Common.ScheduleObj.Schedule_UsPushData;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsActivityScheduleBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/**
 * 排期下的活动数据管理
 * 1. 活动注册失败重试机制
 * 2. 活动结束时上报SS
 */
public class UsActivityScheduleInfo implements _IHandlerHolder
{
    private NPUserServer _m_usUSServer;

    //数据实例ID
    private long _m_dbId;

    //排期ID，同时也是对应活动的实例ID
    private long _m_scheduleId;
    //跨服分组ID
    private long _m_usGroupId;
    //跨服实例ID
    private long _m_crossInstanceId;
    //活动主体实例ID
    private long _m_gameLogicInstanceId;
    //活动数据
    private long _m_activityId;
    private long _m_startTimeMs;
    private long _m_endTimeMs;
    private long _m_closeTimeMs;
    private int _m_submitCount;

    //当前活动排期下的us列表
    private List<Integer> _m_usIdList;
    //资源文件信息
    private String _m_resFileName;
    private String _m_resFileMd5;
    private String _m_resFileDir;

    //是否开启注册
    private boolean _m_startReg;
    //是否注册
    private boolean _m_bRegged;

    //活动是否完成
    private boolean _m_bDone;

    //活动实例ID
    private long _m_activityInstanceId;

    private MutexAtom _m_mutex;

    public UsActivityScheduleInfo(NPUserServer _usServer, UsActivityScheduleBO _bo)
    {
        _m_usUSServer = _usServer;

        _m_dbId = _bo.getId();

        _m_scheduleId = _bo.getScheduleId();
        _m_usGroupId = _bo.getUsGroupId();
        _m_crossInstanceId = _bo.getCrossInstanceId();
        _m_gameLogicInstanceId = _bo.getGameLogicInstanceId();
        _m_bRegged = _bo.getIsRegistered();
        _m_bDone = _bo.getIsDone();
        _m_activityId = _bo.getActivityId();
        _m_startTimeMs = _bo.getStartTimeMs();
        _m_endTimeMs = _bo.getEndTimeMs();
        _m_closeTimeMs = _bo.getCloseTimeMs();
        _m_submitCount = _bo.getSubmitCount();

        _m_usIdList = new ArrayList<>();
        if (_bo.getUsIdList() != null)
        {
            Common_IntList usList = new Common_IntList();
            ByteBuffer usListBuff = ByteBuffer.wrap(_bo.getUsIdList());
            usList.readPackage(usListBuff);
            _m_usIdList = usList.getValueList();
        }

        _m_resFileName = _bo.getResFileName();
        _m_resFileMd5 = _bo.getResFileMd5();
        _m_resFileDir = _bo.getResFileDir();
        _m_activityInstanceId = _bo.getActivityInstanceId();

        _m_mutex = new MutexAtom();
    }

    public NPUserServer getUSServer()
    {
        return _m_usUSServer;
    }

    public long getId()
    {
        return _m_dbId;
    }

    public long getScheduleId()
    {
        return _m_scheduleId;
    }

    /**
     * 来自SS服务器排期的自增ID，可以用于上传跨服组队服务器，用于表示组队的分组ID
     * @return
     */
    public long getUsGroupId()
    {
        return _m_usGroupId;
    }

    public long getActivityId()
    {
        return _m_activityId;
    }

    public long getStartTimeMs()
    {
        return _m_startTimeMs;
    }

    public long getEndTimeMs()
    {
        return _m_endTimeMs;
    }

    public long getCloseTimeMs()
    {
        return _m_closeTimeMs;
    }

    public long getCrossInstanceId()
    {
        return _m_crossInstanceId;
    }

    public long getGameLogicInstanceId()
    {
        return _m_gameLogicInstanceId;
    }

    public String getResFileName()
    {
        return _m_resFileName;
    }

    public String getResFileMd5()
    {
        return _m_resFileMd5;
    }

    public String getResFileDir()
    {
        return _m_resFileDir;
    }

    public int getSubmitCount()
    {
        return _m_submitCount;
    }

    /**
     * 获取参与该排期的服务器列表
     * @return
     */
    public List<Integer> getUsIdList()
    {
        return _m_usIdList;
    }

    public boolean isRegged()
    {
        return _m_bRegged;
    }

    public boolean isDone()
    {
        return _m_bDone;
    }

    public long getActivityInstanceId()
    {
        return _m_activityInstanceId;
    }

    /**
     * 开启活动注册
     * @param _context
     */
    public void startToRegActivity(NPPlayerContext _context)
    {
        _m_mutex.lock();
        try
        {
            //已注册成功
            if (_m_bRegged)
                return;

            //已开启注册
            if (_m_startReg)
                return;

            _m_startReg = true;
        } finally
        {
            _m_mutex.unlock();
        }

        //注册活动
        _AActivityBase activity = getUSServer().getCommActivityMgr().register(this, _m_activityId, _m_crossInstanceId, _m_gameLogicInstanceId
                , _m_startTimeMs, _m_endTimeMs, _m_closeTimeMs, _context);

        _m_mutex.lock();
        try
        {
            //注册失败，需要重复注册，超过一段时间进行报错，通过GM命令手动介入处理
            if (null == activity)
            {
                _m_startReg = false;

                USLog.error(_m_usUSServer, "schedule:{} register activity:{} fail.", _m_scheduleId, _m_activityId);
                return;
            }

            //更新数据
            _m_bRegged = true;
            _m_activityInstanceId = activity.getInstanceId();
        } finally
        {
            _m_mutex.unlock();
        }

        ALMySqlUpdateValue update = new ALMySqlUpdateValue();
        update.addValueObj("is_registered", 1);
        update.addValueObj("activity_instance_id", _m_activityInstanceId);
        getUSServer().getBM().getBM(UsActivityScheduleBO.class).update("id", _m_dbId, update);

        USLog.info(_m_usUSServer, "schedule:{} register activity:{} suc.", _m_scheduleId, _m_activityId);
    }

    public void onScheduleInfoPush(Schedule_UsPushData _usSchedule)
    {

    }

    /**
     * 活动完成时的处理
     */
    public void onActivityDiscard()
    {
        _m_bDone = true;

        ALMySqlUpdateValue update = new ALMySqlUpdateValue();
        update.addValueObj("is_done", 1);
        getUSServer().getBM().getBM(UsActivityScheduleBO.class).update("id", _m_dbId, update);

        ALSynTaskManager.getInstance().regTask(new UsActivityReportDoneToSsTask(getUSServer(), this));
    }

    /**
     * 设置活动可以发送奖励
     */
    public void setCanSendReward()
    {
        //设置活动可以发送奖励
        getUSServer().getCommActivityMgr().setCanSendReward(_m_activityInstanceId);
    }

    /**
     * 更新排期资源信息
     * @param _usSchedule
     */
    public void updateScheduleRes(Schedule_UsPushData _usSchedule)
    {
        _m_mutex.lock();
        try
        {
            String origResFile = _m_resFileName;
            int oriSubmitCount = _m_submitCount;

            _m_resFileName = _usSchedule.getResFile();
            _m_resFileMd5 = _usSchedule.getResFileMd5();
            _m_resFileDir = _usSchedule.getResFileDir();
            _m_submitCount = _usSchedule.getSubmitCount();

            ALMySqlUpdateValue update = new ALMySqlUpdateValue();
            update.addValueObj("res_file_name", _m_resFileName);
            update.addValueObj("res_file_md5", _m_resFileMd5);
            update.addValueObj("res_file_dir", _m_resFileDir);
            update.addValueObj("submit_count", _m_submitCount);
            getUSServer().getBM().getBM(UsActivityScheduleBO.class).update("id", _m_dbId, update);

            USLog.info(getUSServer(), "UsActivityScheduleInfo updateScheduleRes scheduleId:{}, origResFile:{} oriSubmitCount:{}, newResFile:{} newSubmitCount:{}",
                    _m_scheduleId, origResFile, oriSubmitCount,
                    _usSchedule.getResFile(), _usSchedule.getSubmitCount());
        } finally
        {
            _m_mutex.unlock();
        }
    }

    /**
     * 销毁数据
     */
    public void discard()
    {
        getUSServer().getBM().getBM(UsActivityScheduleBO.class).delAll("id", _m_dbId);
    }

    /**
     * 构造活动热更信息
     * @return
     */
    public Activity_HotRefInfo makeHotRefInfo()
    {
        return new Activity_HotRefInfo(
                getActivityInstanceId(),
                getActivityId(),
                getResFileName(),
                getResFileMd5(),
                getResFileDir());
    }
}
