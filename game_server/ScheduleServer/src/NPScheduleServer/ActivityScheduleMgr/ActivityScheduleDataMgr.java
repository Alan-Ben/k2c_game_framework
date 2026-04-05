package NPScheduleServer.ActivityScheduleMgr;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.ScheduleObj.*;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.SSActivityScheduleErr;
import NPCommon.ErrMain.ScheduleErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.ActivityScheduleBO;
import SSDB.Bo.ActivityScheduleUsGroupBO;
import SSDB.Bo.ActivityScheduleUsInfoBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ActivityScheduleDataMgr
{
    private static ActivityScheduleDataMgr _g_instance = new ActivityScheduleDataMgr();

    public static ActivityScheduleDataMgr getInstance()
    {
        return _g_instance;
    }

    //排期待处理数据List
    private List<ActivitySchedulePendingData> _m_pendingDataList = new ArrayList<>();

    //排期数据Map key:排期实例ID value:排期数据
    private Map<Long, ActivityScheduleData> _m_scheduleDataMap = new HashMap<>();
    //排期数据List
    private List<ActivityScheduleData> _m_scheduleDataList = new ArrayList<>();

    private MutexObject _m_mutex = new MutexObject();

    private ActivityScheduleDataMgr()
    {

    }

    public BM getBM()
    {
        return NPScheduleServer.getInstance().getBM();
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 从待处理列表中查找指定scheduleId的排期数据
     * @param _phpScheduleId 排期ID
     * @return 找到的待处理数据，如果不存在则返回null
     */
    private ActivitySchedulePendingData lookupPendingSchedule(long _phpScheduleId)
    {
        _lock();
        try{
            for (ActivitySchedulePendingData pendingData : _m_pendingDataList)
            {
                if (pendingData.getScheduleId() == _phpScheduleId)
                {
                    return pendingData;
                }
            }
            return null;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 根据数据库ID查找排期数据
     * @param _scheduleDbId 排期数据库ID
     * @return 找到的排期数据，如果不存在则返回null
     */
    public ActivityScheduleData lookupSchedule(long _scheduleDbId)
    {
        _lock();
        try{
            return _m_scheduleDataMap.get(_scheduleDbId);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 根据后台排期ID查找排期数据
     * @param _phpScheduleId 后台排期ID
     * @return 找到的排期数据，如果不存在则返回null
     */
    public ActivityScheduleData lookupScheduleByPhpScheduleId(long _phpScheduleId)
    {
        _lock();
        try{
            for (ActivityScheduleData scheduleData : _m_scheduleDataList)
            {
                if (scheduleData.getBO().getPhpScheduleId() == _phpScheduleId)
                {
                    return scheduleData;
                }
            }
            return null;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有待激活排期列表
     * @return 待激活排期列表（返回副本，线程安全）
     */
    public List<ActivitySchedulePendingData> getPendingDataList()
    {
        _lock();
        try{
            return new ArrayList<>(_m_pendingDataList);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有已激活排期列表
     * @return 已激活排期列表（返回副本，线程安全）
     */
    public List<ActivityScheduleData> getActiveDataList()
    {
        _lock();
        try{
            return new ArrayList<>(_m_scheduleDataList);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 修改排期数据 - 找不到对应的排期ID则视为新增
     * <p>
     * 执行流程：
     * 1. 解析JSON数据并验证必要字段
     * 2. 根据scheduleId从待处理列表中查找现有排期数据
     * 3. 如果不存在则创建新排期，添加到待处理列表
     * 4. 如果存在则验证版本并更新数据
     * @param _phpInfo hout排期信息
     * @return 操作结果，成功返回Result.SUCC，失败返回具体错误码
     */
    public Result chgScheduleData(Schedule_PhpInfo _phpInfo)
    {
        _lock();
        try
        {
            // 1. 从待处理列表中查找现有排期数据
            ActivitySchedulePendingData existingPendingData = lookupPendingSchedule(_phpInfo.getPhpScheduleId());

            // 2. 如果不存在则创建新排期数据，否则更新现有数据
            if (existingPendingData == null)
            {
                // 3. 检查是否是已激活的排期数据
                if (lookupScheduleByPhpScheduleId(_phpInfo.getPhpScheduleId()) != null)
                {
                    CommLog.error("ActivityScheduleDataMgr chgScheduleData fail, scheduleId:{} already active.",
                            _phpInfo.getPhpScheduleId());
                    return ScheduleErr.SCHEDULE_HAD_ACTIVE_CANT_UPDATE;
                }

                return createNewSchedule(_phpInfo);
            } else
            {
                return existingPendingData.updateData(_phpInfo);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新排期资源信息 - 同时支持待处理排期和已激活排期
     * 优先查找待处理列表，未找到则查找已激活列表
     *
     * @param _resUpdateInfo 资源更新信息
     * @return 操作结果，成功返回Result.SUCC，失败返回具体错误码
     */
    public Result updateScheduleResInfo(Schedule_ResUpdateInfo _resUpdateInfo)
    {
        _lock();
        try
        {
            // 1. 先从待处理列表中查找
            ActivitySchedulePendingData pendingData = lookupPendingSchedule(_resUpdateInfo.getPhpScheduleId());
            if (pendingData != null)
                return pendingData.updateResInfo(_resUpdateInfo);

            // 2. 从已激活列表中查找
            ActivityScheduleData scheduleData = lookupScheduleByPhpScheduleId(_resUpdateInfo.getPhpScheduleId());
            if (scheduleData == null)
            {
                CommLog.error("ActivityScheduleDataMgr.updateScheduleResInfo - " +
                    "schedule not found: phpScheduleId={}", _resUpdateInfo.getPhpScheduleId());
                return SSActivityScheduleErr.SCHEDULE_NOT_FOUND;
            }

            // 3. 执行已激活排期的资源更新
            return scheduleData.updateResInfo(_resUpdateInfo);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建新排期数据 - 新增时先放到待处理列表中
     * @param _parsedData 解析后的排期数据
     * @return 操作结果
     */
    public Result createNewSchedule(Schedule_PhpInfo _parsedData)
    {
        _lock();
        try{
            // 创建新的ActivityScheduleBO对象
            ActivityScheduleBO newScheduleBO = new ActivityScheduleBO();

            // 设置基本信息
            newScheduleBO.setPhpScheduleId(getBM(), _parsedData.getPhpScheduleId());  // 使用scheduleId作为业务主键
            newScheduleBO.setSubmitCount(getBM(), _parsedData.getSubmitCount());
            newScheduleBO.setIsActive(getBM(), false);  // 新增时设为未激活状态
            newScheduleBO.setPrePushTimeMs(getBM(), _parsedData.getPrePushTimeMs());

            // 设置活动信息
            Schedule_ActivityInfo activityInfo = _parsedData.getActivity();
            newScheduleBO.setActivityId(getBM(), activityInfo.getActivityId());
            newScheduleBO.setStartTimeMs(getBM(), activityInfo.getStartTimeMs());
            newScheduleBO.setEndTimeMs(getBM(), activityInfo.getEndTimeMs());
            newScheduleBO.setCloseTimeMs(getBM(), activityInfo.getCloseTimeMs());

            // 设置分组数据
            Schedule_GroupData groupData = new Schedule_GroupData();
            groupData.getGroupList().addAll(_parsedData.getGroupList());
            newScheduleBO.setGroupData(getBM(), groupData.makePackage().array());
            newScheduleBO.insert(getBM());

            // 包装为待处理数据并添加到待处理列表
            ActivitySchedulePendingData pendingData = new ActivitySchedulePendingData(newScheduleBO, groupData);
            _m_pendingDataList.add(pendingData);

            CommLog.info("ActivityScheduleDataMgr addPendingSchedule succ, phpScheduleId:{} submitCount:{}",
                    _parsedData.getPhpScheduleId(), _parsedData.getSubmitCount());
            return Result.SUCC;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查预下发时间并处理待处理数据的激活
     * <p>
     * 执行流程：
     * 1. 遍历待处理列表，检查预下发时间
     * 2. 对于到达预下发时间的排期，创建正式的排期数据
     * 3. 从待处理列表移动到正式列表
     * 4. 激活排期状态
     */
    public void tick()
    {
        _lock();
        try
        {
            long currentTimeMs = CommonFunc.getNowTimeMS();
            List<ActivitySchedulePendingData> toActivateList = new ArrayList<>();

            // 1. 检查哪些待处理数据需要激活
            for (int i = _m_pendingDataList.size() - 1; i >= 0; i--)
            {
                ActivitySchedulePendingData pendingData = _m_pendingDataList.get(i);
                ActivityScheduleBO bo = pendingData.getBO();
                if (bo.getPrePushTimeMs() <= currentTimeMs)
                {
                    toActivateList.add(pendingData);
                    _m_pendingDataList.remove(i);
                }
            }

            // 2. 处理需要激活的数据
            for (ActivitySchedulePendingData pendingData : toActivateList)
            {
                ActivityScheduleBO bo = pendingData.getBO();
                bo.saveIsActive(getBM(), true);

                // 3. 创建正式的排期数据对象
                ActivityScheduleData scheduleData = new ActivityScheduleData(bo, pendingData.getGroupData());
                _m_scheduleDataList.add(scheduleData);
                _m_scheduleDataMap.put(scheduleData.getScheduleId(), scheduleData);

                CommLog.info("Schedule active succ, scheduleId:{} phpScheduleId:{}", bo.getId(), pendingData.getScheduleId());

                // 4. 检查创建分组数据
                scheduleData.checkCreateGroupData();

                // 5. 开始推送流程
                ALSynTaskManager.getInstance().regTask(scheduleData::startPush);
            }

            // 5. 检查删除已完成的排期数据
            List<ActivityScheduleData> toRemoveList = new ArrayList<>();
            for (ActivityScheduleData scheduleData : _m_scheduleDataList)
            {
                if (scheduleData.isAllUsGroupDone())
                {
                    scheduleData.startProcessDiscard();
                }

                if (scheduleData.canDiscard())
                {
                    toRemoveList.add(scheduleData);
                }
            }
            for (ActivityScheduleData scheduleData : toRemoveList)
            {
                _m_scheduleDataList.remove(scheduleData);
                _m_scheduleDataMap.remove(scheduleData.getScheduleId());

                scheduleData.discard();

                CommLog.info("Schedule remove succ, scheduleId:{} phpScheduleId:{}",
                        scheduleData.getScheduleId(), scheduleData.getBO().getPhpScheduleId());
            }

            // 6. 注册下一次tick任务
            ALSynTaskManager.getInstance().regTask(() -> ActivityScheduleDataMgr.getInstance().tick(), 5000);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 从数据库初始化排期数据管理器 - 服务器启动时调用
     * <p>
     * 执行流程：
     * 1. 加载所有未完成的ActivityScheduleBO数据
     * 2. 根据激活状态分类到待处理列表和已激活列表
     * 3. 对已激活排期加载完整的分组数据结构
     * 4. 恢复推送任务状态
     * @return true=初始化成功, false=初始化失败
     */
    public boolean initFromDB()
    {
        try
        {
            CommLog.info("======= start init ActivityScheduleDataMgr... =======");

            // 1. 加载所有未完成的排期数据
            List<ActivityScheduleBO> scheduleBoList = getBM().getBM(ActivityScheduleBO.class).s_findAll();
            if (scheduleBoList == null)
            {
                CommLog.error("load ActivityScheduleBO data fail");
                return false;
            }

            for (ActivityScheduleBO scheduleBO : scheduleBoList)
            {
                if (scheduleBO.getIsActive())
                {
                    // 已激活的排期：加载到正式列表
                    loadActiveSchedule(scheduleBO);
                } else
                {
                    // 未激活的排期：加载到待处理列表
                    loadPendingSchedule(scheduleBO);
                }
            }

            // 3. 为每个已激活排期加载完整的用户分组数据
            List<ActivityScheduleUsGroupBO> usGroupBoList = getBM().getBM(ActivityScheduleUsGroupBO.class).s_findAll();
            if (usGroupBoList == null)
            {
                CommLog.error("ActivityScheduleDataMgr load ActivityScheduleUsGroupBO data fail");
                return false;
            }

            for (ActivityScheduleUsGroupBO usGroupBO : usGroupBoList)
            {
                // 根据scheduleDbId找到对应的排期数据
                ActivityScheduleData scheduleData = lookupSchedule(usGroupBO.getScheduleDbId());
                if (scheduleData == null)
                {
                    CommLog.error("ActivityScheduleDataMgr lookup scheduleDataFail, scheduleDbId={} usGroupId={}",
                            usGroupBO.getScheduleDbId(), usGroupBO.getId());
                    continue;
                }

                scheduleData._initUsGroupFromDB(usGroupBO);
            }

            // 4. 加载所有用户服务器信息数据
            List<ActivityScheduleUsInfoBO> usInfoBoList = getBM().getBM(ActivityScheduleUsInfoBO.class).s_findAll();
            if (usInfoBoList == null)
            {
                CommLog.error("ActivityScheduleDataMgr load ActivityScheduleUsInfoBO data fail");
                return false;
            }

            for (ActivityScheduleUsInfoBO usInfoBO : usInfoBoList)
            {
                // 根据usGroupDbId找到对应的用户分组
                ActivityScheduleData scheduleData = lookupSchedule(usInfoBO.getScheduleDbId());
                if (scheduleData == null)
                {
                    CommLog.error("ActivityScheduleDataMgr lookup scheduleDataFail, scheduleDbId={} usInfoId={}",
                            usInfoBO.getScheduleDbId(), usInfoBO.getId());
                    continue;
                }

                // 创建用户服务器信息对象
                scheduleData._initUsInfoFromDB(usInfoBO);
            }

            // 5. 初始化所有排期数据的完成计数器（性能优化）
            for (ActivityScheduleData scheduleData : _m_scheduleDataList)
            {
                scheduleData.initDoneCounter();
            }

            CommLog.info("ActivityScheduleDataMgr init done, pendingCount={}, activeCount={}", _m_pendingDataList.size(), _m_scheduleDataList.size());
            CommLog.info("======= end init ActivityScheduleDataMgr... =======");

            return true;
        } catch (Exception e)
        {
            CommLog.error("ActivityScheduleDataMgr init fail", e);
            return false;
        }
    }

    /**
     * 初始化完成后调用，启动排期推送任务
     */
    public void onInit()
    {
        _lock();
        try{
            for (ActivityScheduleData scheduleData : _m_scheduleDataList)
            {
                scheduleData.checkCreateGroupData();
                scheduleData.startPush();
            }
        }finally
        {
            _unlock();
        }

        ALSynTaskManager.getInstance().regTask(() -> ActivityScheduleDataMgr.getInstance().tick());
    }

    /**
     * 加载待处理排期数据
     * @param scheduleBO 排期BO对象
     * @return true=加载成功, false=加载失败
     */
    private void loadPendingSchedule(ActivityScheduleBO scheduleBO)
    {
        try
        {
            // 解析分组数据
            Schedule_GroupData groupData = new Schedule_GroupData();
            if (scheduleBO.getGroupData() != null)
            {
                groupData.readPackage(ByteBuffer.wrap(scheduleBO.getGroupData()));
            }

            // 创建待处理数据对象
            ActivitySchedulePendingData pendingData = new ActivitySchedulePendingData(scheduleBO, groupData);
            _m_pendingDataList.add(pendingData);

        } catch (Exception e)
        {
            CommLog.error("ActivityScheduleDataMgr loadPendingSchedule fail, scheduleId={}", scheduleBO.getPhpScheduleId(), e);
        }
    }

    /**
     * 加载已激活排期数据 - 需要完整重建数据结构
     * @param scheduleBO 排期BO对象
     * @return true=加载成功, false=加载失败
     */
    private void loadActiveSchedule(ActivityScheduleBO scheduleBO)
    {
        try
        {
            Schedule_GroupData info = new Schedule_GroupData();
            info.readPackage(ByteBuffer.wrap(scheduleBO.getGroupData()));

            // 1. 创建排期数据对象
            ActivityScheduleData scheduleData = new ActivityScheduleData(scheduleBO, info);

            // 2. 添加到管理容器
            _m_scheduleDataList.add(scheduleData);
            _m_scheduleDataMap.put(scheduleData.getScheduleId(), scheduleData);

        } catch (Exception e)
        {
            CommLog.error("ActivityScheduleDataMgr loadActiveSchedule fail, scheduleId={}", scheduleBO.getPhpScheduleId(), e);
        }
    }

    /**
     * 设置指定用户服务器的排期完成状态
     * @param _scheduleId
     * @param _usGroupId
     * @param _usId
     */
    public void setUsDone(long _scheduleId, long _usGroupId, int _usId)
    {
        _lock();
        try
        {
            ActivityScheduleData scheduleData = lookupSchedule(_scheduleId);
            if (scheduleData == null)
            {
                CommLog.error("ActivityScheduleDataMgr setUsDone fail, scheduleId={} not found.", _scheduleId);
                return;
            }

            scheduleData.setUsDone(_usGroupId, _usId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置指定用户服务器的排期结算状态
     * @param _scheduleId
     * @param _usGroupId
     * @param _usId
     */
    public void setUsEnterSettling(long _scheduleId, long _usGroupId, int _usId)
    {
        _lock();
        try
        {
            ActivityScheduleData scheduleData = lookupSchedule(_scheduleId);
            if (scheduleData == null)
            {
                CommLog.error("ActivityScheduleDataMgr setUsEnterSettling fail, scheduleId={} not found.", _scheduleId);
                return;
            }

            scheduleData.setUsEnterSettling(_usGroupId, _usId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置指定用户服务器的排期开启状态
     * @param _scheduleId
     * @param _usGroupId
     * @param _usId
     */
    public void setUsPlaying(long _scheduleId, long _usGroupId, int _usId)
    {
        _lock();
        try
        {
            ActivityScheduleData scheduleData = lookupSchedule(_scheduleId);
            if (scheduleData == null)
            {
                CommLog.error("ActivityScheduleDataMgr setUsDone fail, scheduleId={} not found.", _scheduleId);
                return;
            }

            scheduleData.setUsPlaying(_usGroupId, _usId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 生成指定用户服务器的排期推送数据列表
     * @param _usId 用户服务器ID
     * @return 排期推送数据列表
     */
    public List<Schedule_UsPushData> makeUsScheduleHadPushList(int _usId)
    {
        List<Schedule_UsPushData> resultList = new ArrayList<>();

        _lock();
        try
        {
            for (ActivityScheduleData scheduleData : _m_scheduleDataList)
            {
                Schedule_UsPushData pushData = scheduleData.makeUsPushDataIfPushed(_usId);
                if (pushData != null)
                {
                    resultList.add(pushData);
                }
            }
        } finally
        {
            _unlock();
        }

        return resultList;

    }
}
