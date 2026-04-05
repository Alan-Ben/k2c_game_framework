package NPScheduleServer.ActivityScheduleMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_IntList;
import Common.ScheduleObj.Schedule_GroupData;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_ResUpdateInfo;
import Common.ScheduleObj.Schedule_UsPushData;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.ErrMain.SSActivityScheduleErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPScheduleServer.ActivityScheduleMgr.Task.ActivityScheduleResUpdatePushTask;
import NPScheduleServer.ActivityScheduleMgr.Task.NoticeCanSendRewardTask;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.ActivityScheduleBO;
import SSDB.Bo.ActivityScheduleUsGroupBO;
import SSDB.Bo.ActivityScheduleUsInfoBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动排期数据容器 - 管理单个排期的完整信息
 * <p>
 * 主要功能：
 * 1. 封装ActivityScheduleBO数据库对象
 * 2. 管理配置分组列表
 * 3. 管理用户分组列表
 * 4. 提供数据访问接口
 */
public class ActivityScheduleData
{
    // 数据库实体对象
    private ActivityScheduleBO _m_bo;
    // 分组数据
    private Schedule_GroupData _m_groupData;

    // 用户分组列表
    private List<ActivityScheduleUsGroup> _m_userGroupList;

    // 已完成的UsGroup数量计数器（性能优化：避免每次遍历）
    private int _m_doneUsGroupCount;

    // 已废弃的UsGroup数量计数器（性能优化：避免每次遍历）
    private int _m_discardedUsGroupCount;

    private MutexAtom _m_mutex;

    /**
     * 构造函数
     * @param _bo        数据库实体对象
     * @param _groupData
     */
    public ActivityScheduleData(ActivityScheduleBO _bo, Schedule_GroupData _groupData)
    {
        _m_bo = _bo;
        _m_userGroupList = new ArrayList<>();

        _m_groupData = _groupData;

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

    /**
     * 添加US分组到排期数据
     * @param _usGroupBO 用户分组BO对象
     */
    public void _initUsGroupFromDB(ActivityScheduleUsGroupBO _usGroupBO)
    {
        // 排期未初始化完成，不能加载用户分组数据
        if (!_m_bo.getIsInited())
        {
            NPScheduleServer.getInstance().getDDAlert().err("ActivityScheduleData.initUsGroup",
                    "ActivityScheduleData initUsGroup Schedule not inited, scheduleId:{} usGroupDbId:{}",getScheduleId(), _usGroupBO.getId());
            return;
        }

        ActivityScheduleUsGroup usGroup = new ActivityScheduleUsGroup(this, _usGroupBO);
        _m_userGroupList.add(usGroup);
    }

    /**
     * 初始化US信息
     * @param _usInfoBO
     */
    public void _initUsInfoFromDB(ActivityScheduleUsInfoBO _usInfoBO)
    {
        // 排期未初始化完成，不能加载用户分组数据
        if (!_m_bo.getIsInited())
        {
            NPScheduleServer.getInstance().getDDAlert().err("ActivityScheduleData.initUsInfo",
                    "ActivityScheduleData initUsInfo Schedule not inited, scheduleId:{} usInfoDbId:{}",getScheduleId(), _usInfoBO.getId());
            return;
        }

        for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
        {
            if (usGroup.getGroupDbId() == _usInfoBO.getUsGroupDbId())
            {
                usGroup.initUsInfo(_usInfoBO);
                return;
            }
        }
    }

    /**
     * 初始化完成计数器
     *
     * 应在从数据库加载所有UsGroup和UsInfo后调用此方法
     * 用于计算已完成的UsGroup数量和已废弃的UsGroup数量，避免后续频繁遍历
     *
     * 调用时机：数据加载完成后，在开始处理业务逻辑之前
     */
    public void initDoneCounter()
    {
        _lock();
        try
        {
            _m_doneUsGroupCount = 0;
            _m_discardedUsGroupCount = 0;
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                if (usGroup.isAllDone())
                {
                    _m_doneUsGroupCount++;
                }
                if (usGroup.canDiscard())
                {
                    _m_discardedUsGroupCount++;
                }
            }
            CommLog.info("ActivityScheduleData.initDoneCounter - initialized: scheduleId={}, doneCount={}/{}, discardedCount={}/{}",
                    getScheduleId(), _m_doneUsGroupCount, _m_userGroupList.size(), _m_discardedUsGroupCount, _m_userGroupList.size());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取数据库实体对象
     * @return ActivityScheduleBO对象
     */
    public ActivityScheduleBO getBO()
    {
        return _m_bo;
    }

    public BM getBM()
    {
        return NPScheduleServer.getInstance().getBM();
    }

    /**
     * 获取排期ID（业务主键）
     * @return 排期ID
     */
    public long getScheduleId()
    {
        return _m_bo.getId();
    }

    /**
     * 获取提交次数
     * @return 提交次数
     */
    public int getSubmitCount()
    {
        return _m_bo.getSubmitCount();
    }

    /**
     * 检查是否所有UsGroup都已完成
     *
     * 性能优化：
     * - 使用计数器代替遍历，时间复杂度 O(n) → O(1)
     * - 计数器在 setUsDone() 中实时更新
     *
     * @return true-所有分组都已完成，false-还有未完成的分组
     */
    public boolean isAllUsGroupDone()
    {
        _lock();
        try
        {
            return _m_doneUsGroupCount == _m_userGroupList.size();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否包含指定UserServer
     * @param usId 目标UserServer ID
     * @return true-包含，false-不包含
     */
    public boolean containsUsId(int usId)
    {
        _lock();
        try
        {
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                if (usGroup.containsUsId(usId))
                {
                    return true;
                }
            }
            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找指定UserServer的信息对象
     * @param usId 目标UserServer ID
     * @return UserServer信息对象，如果不存在则返回null
     */
    public ActivityScheduleUsInfo lookupUsInfo(int usId)
    {
        _lock();
        try
        {
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                ActivityScheduleUsInfo usInfo = usGroup.lookupUsInfo(usId);
                if (usInfo != null)
                {
                    return usInfo;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有未完成的US信息列表
     * @return US信息列表
     */
    public List<ActivityScheduleUsInfo> getAllUsInfoList()
    {
        List<ActivityScheduleUsInfo> resultList = new ArrayList<>();
        _lock();
        try
        {
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                List<ActivityScheduleUsInfo> usInfoList = usGroup.getAllUsInfoList();
                resultList.addAll(usInfoList);
            }
            return resultList;
        } finally
        {
            _unlock();
        }
    }


    /**
     * 根据分组数据库ID查找用户分组对象
     * @param _groupDbId
     * @return
     */
    public ActivityScheduleUsGroup lookupUsGroup(long _groupDbId)
    {
        _lock();
        try
        {
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                if (usGroup.getGroupDbId() == _groupDbId)
                {
                    return usGroup;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查创建分组数据
     */
    public void checkCreateGroupData()
    {
        _lock();
        try
        {
            // 已经初始化过了
            if (_m_bo.getIsInited())
                return;

            if (_m_groupData == null)
            {
                NPScheduleServer.getInstance().getDDAlert().err("ActivityScheduleData.checkCreateGroupData",
                        "ActivityScheduleData checkCreateGroupData ScheduleId=" + getScheduleId() + " groupData is null");
                return;
            }

            for (Schedule_GroupInfo groupInfo : _m_groupData.getGroupList())
            {
                for (Common_IntList userGroup : groupInfo.getUsGroupList())
                {
                    // 1. 根据解析结果创建用户分组对象
                    ActivityScheduleUsGroupBO bo = new ActivityScheduleUsGroupBO();
                    bo.setScheduleDbId(getBM(), _m_bo.getId());
                    bo.setResFileName(getBM(), groupInfo.getResFile());
                    bo.setResFileMd5(getBM(), groupInfo.getResFileMd5());
                    bo.setResFileDir(getBM(), groupInfo.getResFileDir());
                    bo.setHadDiscarded(getBM(), false);  // 新建分组，未废弃
                    bo.insert(getBM());

                    ActivityScheduleUsGroup usGroup = new ActivityScheduleUsGroup(this, bo);
                    _m_userGroupList.add(usGroup);

                    // 2. 根据us列表 创建每个us的数据对象
                    for (Integer usId : userGroup.getValueList())
                    {
                        ActivityScheduleUsInfoBO usBo = new ActivityScheduleUsInfoBO();
                        usBo.setScheduleDbId(getBM(), _m_bo.getId());
                        usBo.setUsId(getBM(), usId);
                        usBo.setUsGroupDbId(getBM(), bo.getId());
                        usBo.insert(getBM());

                        ActivityScheduleUsInfo usInfo = new ActivityScheduleUsInfo(usGroup, usBo);
                        usGroup.addUsInfo(usInfo);
                    }
                }
            }

            _m_bo.saveIsInited(getBM(), true);


        } finally
        {
            _unlock();
        }
    }

    /**
     * 开始推送流程
     * @return
     */
    public void startPush()
    {
        _lock();
        try
        {
            CommLog.info("ActivityScheduleData.startPush scheduleId={} start push to US", getScheduleId());

            // 遍历用户分组，开始推送
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                usGroup.startPush();
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 根据USID查找对应的推送数据，如果未推送则创建推送数据
     * @param _usId
     * @return
     */
    public Schedule_UsPushData makeUsPushDataIfPushed(int _usId)
    {
        _lock();
        try
        {
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                ActivityScheduleUsInfo usInfo = usGroup.lookupUsInfo(_usId);
                if (usInfo != null && usInfo.hadPush() && !usInfo.hadDone())
                {
                    return usGroup.toUsSchedulePushProto();
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置指定US分组的指定US为已完成状态
     *
     * 执行流程：
     * 1. 查找对应的UsGroup和UsInfo
     * 2. 标记该UsInfo为完成状态
     * 3. 检查该UsGroup是否全部完成，如果是则增加完成计数器
     *
     * @param _usGroupId UsGroup数据库ID
     * @param _usId      UserServer ID
     *
     * 性能优化：实时维护 _m_doneUsGroupCount 计数器
     */
    public void setUsDone(long _usGroupId, int _usId)
    {
        _lock();
        try
        {
            ActivityScheduleUsGroup usGroup = lookupUsGroup(_usGroupId);
            if (usGroup != null)
            {
                ActivityScheduleUsInfo usInfo = usGroup.lookupUsInfo(_usId);
                if (usInfo != null)
                {
                    // 检查该UsGroup在标记前是否已完成
                    boolean wasGroupDone = usGroup.isAllDone();

                    // 标记该UsInfo为完成
                    usInfo.markHadDone();

                    // 如果该UsGroup从未完成变为完成，增加计数器
                    if (!wasGroupDone && usGroup.isAllDone())
                    {
                        _m_doneUsGroupCount++;
                        CommLog.info("ActivityScheduleData.setUsDone - UsGroup completed: scheduleId={}, usGroupId={}, doneCount={}/{}",
                                getScheduleId(), _usGroupId, _m_doneUsGroupCount, _m_userGroupList.size());
                    }
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置指定US分组的指定US为已进入结算状态
     * @param _usGroupId
     * @param _usId
     */
    public void setUsEnterSettling(long _usGroupId, int _usId)
    {
        _lock();
        try
        {
            ActivityScheduleUsGroup usGroup = lookupUsGroup(_usGroupId);
            if (usGroup != null)
            {
                ActivityScheduleUsInfo usInfo = usGroup.lookupUsInfo(_usId);
                if (usInfo != null)
                {
                    //判断是否已经上报过了
                    if (usGroup.isAllEnterSettling() && usInfo.hadEnterSettle())
                    {
                        //检查是否全部US都已进入冻结状态，如果是则考虑该us之前没收到，单独推送一次
                        ALSynTaskManager.getInstance().regTask(
                                new NoticeCanSendRewardTask(getScheduleId(), _usId));

                        return;
                    }

                    usInfo.markHadEnterSettle();
                }

                //检查是否全部US都已进入冻结状态
                if (usGroup.isAllEnterSettling())
                {
                    usGroup.noticeCanSendReward();

                    CommLog.info("ActivityScheduleData allEnterSettling start noticeCanSendReward, scheduleId={} usGroupId={}",
                            getScheduleId(), _usGroupId);
                }
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 设置指定US分组的指定US为已完成状态
     * @param _usGroupId
     * @param _usId
     */
    public void setUsPlaying(long _usGroupId, int _usId)
    {
        _lock();
        try
        {
            ActivityScheduleUsGroup usGroup = lookupUsGroup(_usGroupId);
            if (usGroup != null)
            {
                ActivityScheduleUsInfo usInfo = usGroup.lookupUsInfo(_usId);
                if (usInfo != null)
                {
                    usInfo.markPlaying();
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁数据
     */
    public void discard()
    {
        _lock();
        try
        {
            getBM().getBM(ActivityScheduleBO.class).delAll("id", getScheduleId());
            getBM().getBM(ActivityScheduleUsGroupBO.class).delAll("schedule_db_id", getScheduleId());
            getBM().getBM(ActivityScheduleUsInfoBO.class).delAll("schedule_db_id", getScheduleId());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 输出排期状态信息 - 用于调试和日志输出
     * <p>
     * 输出内容：
     * 1. 排期基本信息（排期ID、提交次数、活动ID、时间信息）
     * 2. 所有US分组的详细状态（包含每个分组下所有US的状态）
     * @return 包含排期完整状态的字符串
     */
    @Override
    public String toString()
    {
        _lock();
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.append("ActivityScheduleData{");
            sb.append("scheduleId=").append(getScheduleId());
            sb.append(", submitCount=").append(getSubmitCount());
            sb.append(", activityId=").append(_m_bo.getActivityId());
            sb.append(", startTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getStartTimeMs()));
            sb.append(", endTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getEndTimeMs()));
            sb.append(", closeTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getCloseTimeMs()));
            sb.append(", isInited=").append(_m_bo.getIsInited());
            sb.append(", groupCount=").append(_m_userGroupList.size());
            sb.append(", groups=[\n");

            for (int i = 0; i < _m_userGroupList.size(); i++)
            {
                sb.append("    ").append(_m_userGroupList.get(i).toString());
                if (i < _m_userGroupList.size() - 1)
                {
                    sb.append(",");
                }
                sb.append("\n");
            }

            sb.append("]}");
            return sb.toString();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新资源信息（热更新）
     * <p>
     * 执行流程：
     * 1. 版本控制检查：submitCount必须严格大于当前值
     * 2. 激活状态检查：只能更新已激活的排期
     * 3. 分组列表匹配检查：usIdList必须完全匹配现有分组
     * 4. 更新submitCount到数据库
     * 5. 遍历更新每个分组的资源信息
     * @param resUpdateInfo 资源更新信息（包含phpScheduleId、submitCount、groupList）
     * @return Result.SUCC 成功，或错误码
     * <p>
     * 线程安全：使用_lock()/_unlock()保护并发访问
     */
    public Result updateResInfo(Schedule_ResUpdateInfo resUpdateInfo)
    {
        _lock();
        try
        {
            // 1. 版本控制检查
            if (resUpdateInfo.getSubmitCount() <= getSubmitCount())
            {
                CommLog.error("ActivityScheduleData.updateResInfo - version check failed: " +
                                "submitCount too low, phpScheduleId={}, currentSubmitCount={}, newSubmitCount={}",
                        _m_bo.getPhpScheduleId(), getSubmitCount(), resUpdateInfo.getSubmitCount());
                return SSActivityScheduleErr.SUBMIT_COUNT_TOO_LOW;
            }

            // 2. 激活状态检查
            if (!_m_bo.getIsActive())
            {
                CommLog.error("ActivityScheduleData.updateResInfo - active check failed: " +
                        "schedule not active, phpScheduleId={}", _m_bo.getPhpScheduleId());
                return SSActivityScheduleErr.SCHEDULE_NOT_ACTIVE;
            }

            // 3. 验证并更新分组（合并操作，消除重复查找）
            ResultOne<List<ActivityScheduleUsGroup>> result = validateAndUpdateGroups(resUpdateInfo.getGroupList());
            if (!result.isSucc())
                return result.getResult();

            // 4. 更新submitCount到数据库
            _m_bo.saveSubmitCount(getBM(), resUpdateInfo.getSubmitCount());


            // 5. 遍历推送更新
            for (ActivityScheduleUsGroup usGroup : result.getData())
            {
                Schedule_UsPushData usSchedulePushProto = usGroup.toUsSchedulePushProto();
                for (ActivityScheduleUsInfo usInfo : usGroup.getAllUsInfoList())
                {
                    new ActivityScheduleResUpdatePushTask(usSchedulePushProto, usInfo).run();
                }
            }

            CommLog.info("ActivityScheduleData.updateResInfo - update success: phpScheduleId={}, submitCount={}, groupCount={}",
                    _m_bo.getPhpScheduleId(), resUpdateInfo.getSubmitCount(), resUpdateInfo.getGroupList().size());

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 验证分组匹配并直接更新资源信息
     * <p>
     * 优化要点：
     * 1. 验证时即完成分组查找，避免后续重复查找
     * 2. 匹配成功后立即更新资源信息
     * 3. 单次遍历完成验证+更新两个操作
     * <p>
     * 验证规则：
     * 1. Schedule_GroupInfo.usGroupList 是二维结构，一个GroupInfo包含多个分组
     * 2. 允许部分推送分组，但推送的分组总数不能超过现有分组数
     * 3. 每个推送分组必须与某个现有分组完全匹配（usId集合相同）
     * 4. 同一个现有分组不能被重复匹配
     * <p>
     * 示例：
     * - 现有分组：{1,2} 和 {3,4}
     * - ✅ 允许：{1,2} 或 {3,4} 或 {1,2}{3,4}
     * - ❌ 拒绝：{1}{2} 或 {1,3} 或 {1,2,3}
     * @param newGroupList 新的分组列表（每个GroupInfo可能包含多个分组）
     * @return Result.SUCC 成功，否则返回GROUP_LIST_MISMATCH错误
     */
    private ResultOne<List<ActivityScheduleUsGroup>> validateAndUpdateGroups(List<Schedule_GroupInfo> newGroupList)
    {
        // 1. 计算推送分组总数
        int pushGroupCount = 0;
        for (Schedule_GroupInfo groupInfo : newGroupList)
        {
            if (groupInfo.getUsGroupList() != null)
            {
                pushGroupCount += groupInfo.getUsGroupList().size();
            }
        }

        if (pushGroupCount > _m_userGroupList.size())
        {
            CommLog.error("ActivityScheduleData.validateAndUpdateGroups - group count exceeds: phpScheduleId={}, currentCount={}, pushCount={}",
                    _m_bo.getPhpScheduleId(), _m_userGroupList.size(), pushGroupCount);
            return ResultOne.failed(SSActivityScheduleErr.GROUP_LIST_MISMATCH);
        }

        // 2. 创建匹配标记数组（每个现有分组最多被匹配一次）
        Schedule_GroupInfo[] matched = new Schedule_GroupInfo[_m_userGroupList.size()];

        // 3. 遍历每个推送分组，进行匹配验证并立即更新资源信息
        for (Schedule_GroupInfo groupInfo : newGroupList)
        {
            for (Common_IntList singleGroup : groupInfo.getUsGroupList())
            {
                boolean isMatched = false;

                List<Integer> usIdList = singleGroup.getValueList();
                // 3.1 UsId排序
                usIdList.sort(Integer::compare);

                // 3.2 线性扫描查找匹配的现有分组
                for (int i = 0; i < _m_userGroupList.size(); i++)
                {
                    // 比较usId列表
                    List<Integer> oriSortedUsIdList = _m_userGroupList.get(i).getSortedUsIdList();
                    // 完全匹配
                    if (oriSortedUsIdList.equals(usIdList))
                    {
                        // 检查是否重复匹配同一个现有分组
                        if (matched[i] != null)
                        {
                            CommLog.error("ActivityScheduleData.validateAndUpdateGroups - duplicate group match: " +
                                    "phpScheduleId={}, usIdList={}", _m_bo.getPhpScheduleId(), usIdList);
                            return ResultOne.failed(SSActivityScheduleErr.GROUP_LIST_REPEAT);
                        }

                        matched[i] = groupInfo;
                        isMatched = true;
                        break;
                    }
                }

                if (!isMatched)
                {
                    // 未找到匹配分组，返回错误
                    CommLog.error("ActivityScheduleData.validateAndUpdateGroups - group not matched: phpScheduleId={}, usIdList={}",
                            _m_bo.getPhpScheduleId(), usIdList);
                    return ResultOne.failed(SSActivityScheduleErr.GROUP_LIST_MISMATCH);
                }
            }
        }

        List<ActivityScheduleUsGroup> updatedGroups = new ArrayList<>();

        // 4. 验证通过，更新资源信息
        for (int i = 0; i < matched.length; i++)
        {
            // 跳过未匹配的分组
            if (matched[i] == null)
                continue;

            Schedule_GroupInfo groupInfo = matched[i];
            ActivityScheduleUsGroup usGroup = _m_userGroupList.get(i);
            usGroup.updateResInfo(groupInfo.getResFile(), groupInfo.getResFileMd5(), groupInfo.getResFileDir());
            updatedGroups.add(usGroup);
        }

        return ResultOne.succ(updatedGroups);
    }

    /**
     * 增加已废弃UsGroup计数器
     *
     * 调用时机：ActivityScheduleUsGroup废弃处理成功后调用
     *
     * 线程安全：使用_lock()/_unlock()保护并发访问
     */
    public void incrementDiscardedCount()
    {
        _lock();
        try
        {
            _m_discardedUsGroupCount++;

            CommLog.info("ActivityScheduleData.incrementDiscardedCount - UsGroup discarded: scheduleId={}, discardedCount={}/{}",
                    getScheduleId(), _m_discardedUsGroupCount, _m_userGroupList.size());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查排期数据是否可以被销毁
     *
     * 性能优化：
     * - 使用计数器代替遍历，时间复杂度 O(n) → O(1)
     * - 计数器在 ActivityScheduleUsGroup.startProcessDiscard() 成功后更新
     *
     * 判断标准：
     * - 所有UsGroup都已完成废弃处理（crossInstanceId == 0）
     *
     * 参照方法：isAllUsGroupDone()
     *
     * @return true-可以销毁，false-还有UsGroup未完成废弃处理
     */
    public boolean canDiscard()
    {
        _lock();
        try
        {
            return _m_discardedUsGroupCount == _m_userGroupList.size();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 标记该排期数据可以被销毁
     */
    public void startProcessDiscard()
    {
        _lock();
        try
        {
            for (ActivityScheduleUsGroup usGroup : _m_userGroupList)
            {
                usGroup.startProcessDiscard();
            }
        } finally
        {
            _unlock();
        }
    }
}
