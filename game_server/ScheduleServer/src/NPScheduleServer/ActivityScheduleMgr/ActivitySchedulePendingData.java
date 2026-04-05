package NPScheduleServer.ActivityScheduleMgr;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.Common_IntList;
import Common.ScheduleObj.*;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.SSActivityScheduleErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPScheduleServer.NPScheduleServer;
import SSDB.Bo.ActivityScheduleBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动排期待处理数据包装器
 * <p>
 * 主要功能：
 * 1. 包装ActivityScheduleBO对象
 * 2. 提供待处理数据的更新逻辑
 * 3. 管理排期数据的版本控制
 */
public class ActivitySchedulePendingData
{
    // BO对象引用
    private ActivityScheduleBO _m_bo;
    private Schedule_GroupData _m_groupData;

    private MutexAtom _m_mutex;

    public ActivitySchedulePendingData(ActivityScheduleBO _bo, Schedule_GroupData _groupData)
    {
        _m_bo = _bo;
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
     * 获取BO对象
     */
    public ActivityScheduleBO getBO()
    {
        return _m_bo;
    }

    public Schedule_GroupData getGroupData()
    {
        return _m_groupData;
    }

    public BM getBM()
    {
        return NPScheduleServer.getInstance().getBM();
    }

    /**
     * 获取排期ID
     */
    public long getScheduleId()
    {
        return _m_bo.getPhpScheduleId();
    }

    /**
     * 更新待处理数据
     * <p>
     * 执行流程：
     * 1. 验证提交次数确保是最新数据
     * 2. 更新BO对象的所有字段
     * @param _parsedData 解析后的新排期数据
     * @return 操作结果
     */
    public Result updateData(Schedule_PhpInfo _parsedData)
    {
        _lock();
        try{
            // 1. 验证提交次数，确保是最新数据
            if (_m_bo.getSubmitCount() >= _parsedData.getSubmitCount())
            {
                CommLog.error("ActivitySchedulePendingData updateData fail version is smaller, curSubmitCount:{} newSubmitCount:{}",
                        _m_bo.getSubmitCount(), _parsedData.getSubmitCount());
                return SSActivityScheduleErr.NOT_LATEST_ERR;
            }

            BM bm = getBM();

            _m_groupData = new Schedule_GroupData();
            _m_groupData.getGroupList().addAll(_parsedData.getGroupList());

            // 2. 更新BO对象的字段
            _m_bo.setSubmitCount(bm, _parsedData.getSubmitCount());
            _m_bo.setPrePushTimeMs(bm, _parsedData.getPrePushTimeMs());

            Schedule_ActivityInfo activityInfo = _parsedData.getActivity();
            _m_bo.setActivityId(bm, activityInfo.getActivityId());
            _m_bo.setStartTimeMs(bm, activityInfo.getStartTimeMs());
            _m_bo.setEndTimeMs(bm, activityInfo.getEndTimeMs());
            _m_bo.setCloseTimeMs(bm, activityInfo.getCloseTimeMs());
            _m_bo.setGroupData(bm, _m_groupData.makePackage().array());
            _m_bo.saveAllMarked(bm);

            CommLog.info("ActivitySchedulePendingData updateData succ, phpScheduleId:{} newSubmitCount:{}",
                    getScheduleId(), _parsedData.getSubmitCount());
            return Result.SUCC;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 更新待处理排期的资源信息
     * 根据usGroupList匹配分组，更新对应分组的资源文件信息并保存到数据库
     *
     * @param _resUpdateInfo 资源更新信息
     * @return 操作结果
     */
    public Result updateResInfo(Schedule_ResUpdateInfo _resUpdateInfo)
    {
        _lock();
        try{
            // 1. 版本控制检查
            if (_resUpdateInfo.getSubmitCount() <= _m_bo.getSubmitCount())
            {
                CommLog.error("ActivitySchedulePendingData.updateResInfo - version check failed: " +
                    "phpScheduleId={}, currentSubmitCount={}, newSubmitCount={}",
                    _m_bo.getPhpScheduleId(), _m_bo.getSubmitCount(), _resUpdateInfo.getSubmitCount());
                return SSActivityScheduleErr.SUBMIT_COUNT_TOO_LOW;
            }

            List<Schedule_GroupInfo> updateGroupList = _resUpdateInfo.getGroupList();
            List<Schedule_GroupInfo> existGroupList = _m_groupData.getGroupList();

            // 2. 验证分组数量不超过现有分组
            if (updateGroupList.size() > existGroupList.size())
            {
                CommLog.error("ActivitySchedulePendingData.updateResInfo - group count exceeds: " +
                    "phpScheduleId={}, existCount={}, updateCount={}",
                    _m_bo.getPhpScheduleId(), existGroupList.size(), updateGroupList.size());
                return SSActivityScheduleErr.GROUP_LIST_MISMATCH;
            }

            // 3. 先验证所有分组匹配关系
            Schedule_GroupInfo[] matched = new Schedule_GroupInfo[existGroupList.size()];

            for (Schedule_GroupInfo updateGroup : updateGroupList)
            {
                boolean found = false;
                for (int i = 0; i < existGroupList.size(); i++)
                {
                    if (matched[i] != null) continue;

                    if (isUsGroupListMatch(updateGroup.getUsGroupList(), existGroupList.get(i).getUsGroupList()))
                    {
                        // 检查是否重复匹配同一个现有分组
                        matched[i] = updateGroup;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    CommLog.error("ActivitySchedulePendingData.updateResInfo - group not matched: " +
                        "phpScheduleId={}", _m_bo.getPhpScheduleId());
                    return SSActivityScheduleErr.GROUP_LIST_MISMATCH;
                }
            }

            // 4. 验证通过，统一更新资源信息
            for (int i = 0; i < matched.length; i++)
            {
                if (matched[i] == null)
                    continue;

                existGroupList.get(i).setResFile(matched[i].getResFile());
                existGroupList.get(i).setResFileMd5(matched[i].getResFileMd5());
                existGroupList.get(i).setResFileDir(matched[i].getResFileDir());
            }

            // 5. 保存更新后的数据到数据库
            BM bm = getBM();
            _m_bo.setSubmitCount(bm, _resUpdateInfo.getSubmitCount());
            _m_bo.setGroupData(bm, _m_groupData.makePackage().array());
            _m_bo.saveAllMarked(bm);

            CommLog.info("ActivitySchedulePendingData.updateResInfo - update success: " +
                "phpScheduleId={}, submitCount={}, groupCount={}",
                _m_bo.getPhpScheduleId(), _resUpdateInfo.getSubmitCount(), updateGroupList.size());

            return Result.SUCC;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 比较两个usGroupList是否匹配
     * 逐个比较Common_IntList中的排序后的usId列表
     *
     * @param _listA usGroupList A
     * @param _listB usGroupList B
     * @return 匹配返回true
     */
    private boolean isUsGroupListMatch(List<Common_IntList> _listA, List<Common_IntList> _listB)
    {
        _lock();
        try{
            if (_listA.size() != _listB.size())
                return false;

            // 对每个Common_IntList，比较排序后的usId列表
            boolean[] matchedB = new boolean[_listB.size()];
            for (Common_IntList intListA : _listA)
            {
                List<Integer> sortedA = new ArrayList<>(intListA.getValueList());
                sortedA.sort(Integer::compare);

                boolean found = false;
                for (int j = 0; j < _listB.size(); j++)
                {
                    if (matchedB[j])
                        continue;

                    List<Integer> sortedB = new ArrayList<>(_listB.get(j).getValueList());
                    sortedB.sort(Integer::compare);

                    if (sortedA.equals(sortedB))
                    {
                        matchedB[j] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 输出待激活排期信息 - 用于调试和日志输出
     *
     * 输出内容：
     * 1. 排期基本信息（后台排期ID、数据库ID、提交次数）
     * 2. 预推送时间和激活状态
     * 3. 活动信息（活动ID、开始/结束/关闭时间）
     * 4. 分组配置信息（分组数量、资源文件信息）
     *
     * @return 包含待激活排期完整信息的字符串
     */
    @Override
    public String toString()
    {
        _lock();
        try{
            StringBuilder sb = new StringBuilder();
            sb.append("ActivitySchedulePendingData{");
            sb.append("phpScheduleId=").append(_m_bo.getPhpScheduleId());
            sb.append(", dbId=").append(_m_bo.getId());
            sb.append(", submitCount=").append(_m_bo.getSubmitCount());
            sb.append(", prePushTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getPrePushTimeMs()));
            sb.append(", isActive=").append(_m_bo.getIsActive());
            sb.append(", activityId=").append(_m_bo.getActivityId());
            sb.append(", startTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getStartTimeMs()));
            sb.append(", endTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getEndTimeMs()));
            sb.append(", closeTimeMs=").append(CommonFunc.getTimeStringMs(_m_bo.getCloseTimeMs()));

            if (_m_groupData != null)
            {
                sb.append(", groupCount=").append(_m_groupData.getGroupList().size());
                sb.append(", groupInfo=[\n");
                for (int i = 0; i < _m_groupData.getGroupList().size(); i++)
                {
                    Schedule_GroupInfo groupInfo = _m_groupData.getGroupList().get(i);
                    sb.append("    {resFile='").append(groupInfo.getResFile()).append("'");
                    sb.append(", usGroupCount=").append(groupInfo.getUsGroupList().size());
                    sb.append(", usGroups=[");

                    // 输出每个 US 分组的 US 列表
                    for (int j = 0; j < groupInfo.getUsGroupList().size(); j++)
                    {
                        if (j > 0)
                        {
                            sb.append(", ");
                        }
                        sb.append("[");
                        List<Integer> usList = groupInfo.getUsGroupList().get(j).getValueList();
                        for (int k = 0; k < usList.size(); k++)
                        {
                            if (k > 0)
                            {
                                sb.append(",");
                            }
                            sb.append(usList.get(k));
                        }
                        sb.append("]");
                    }
                    sb.append("]}");

                    if (i < _m_groupData.getGroupList().size() - 1)
                    {
                        sb.append(",");
                    }
                    sb.append("\n");
                }
                sb.append("]");
            }
            else
            {
                sb.append(", groupData=null");
            }

            sb.append("}");
            return sb.toString();
        }finally
        {
            _unlock();
        }
    }
}
