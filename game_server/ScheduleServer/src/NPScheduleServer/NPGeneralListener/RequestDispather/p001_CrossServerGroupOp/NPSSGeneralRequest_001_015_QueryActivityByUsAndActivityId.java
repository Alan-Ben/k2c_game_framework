package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import Common.Common_IntList;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_SimpleActivityInfo;
import Common.ScheduleObj.Schedule_SimpleActivityInfoByUs;
import NP2SS_R.p001_ScheduleOp.ToSS_R_001_015_QueryActivityByUsAndActivityId;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleData;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.ActivityScheduleMgr.ActivitySchedulePendingData;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleUsInfo;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

/**
 * 查询指定US列表中指定活动的排期信息请求处理器
 *
 * 功能：根据UsId列表和ActivityId，返回匹配的排期信息（包含待激活和已激活）
 *
 * 执行流程：
 * 1. 获取所有待激活排期数据并筛选
 * 2. 获取所有已激活排期数据并筛选
 * 3. 条件：activityId匹配 且 包含请求的任意usId
 * 4. 将符合条件的排期转换为简化格式返回
 */
public class NPSSGeneralRequest_001_015_QueryActivityByUsAndActivityId
    extends NPRequestDealer<ToSS_R_001_015_QueryActivityByUsAndActivityId>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer,
                               ToSS_R_001_015_QueryActivityByUsAndActivityId _msg)
    {
        // 将usIdList转换为Set便于快速查找
        Set<Integer> usIdSet = new HashSet<>(_msg.getUsIdList());
        List<Schedule_SimpleActivityInfoByUs> resultList = new ArrayList<>();

        // 1. 获取所有待激活排期
        List<ActivitySchedulePendingData> allPendingList =
            ActivityScheduleDataMgr.getInstance().getPendingDataList();

        // 筛选待激活排期
        for (ActivitySchedulePendingData pending : allPendingList)
        {
            // 检查activityId是否匹配
            if (pending.getBO().getActivityId() != _msg.getActivityId())
            {
                continue;
            }

            // 检查groupList中是否包含请求的usId
            List<Integer> usIdList = containsAnyUsIdInPending(pending, usIdSet);
            if (usIdList == null || usIdList.isEmpty())
                continue;

            for (Integer usId : usIdList)
            {
                resultList.add(convertPendingToSimpleInfo(pending, usId));
            }
        }

        // 2. 获取所有已激活排期
        List<ActivityScheduleData> activeList =
            ActivityScheduleDataMgr.getInstance().getActiveDataList();

        // 筛选已激活排期
        for (ActivityScheduleData active : activeList)
        {
            // 检查activityId是否匹配
            if (active.getBO().getActivityId() != _msg.getActivityId())
            {
                continue;
            }

            // 检查是否包含请求的usId
            List<Schedule_SimpleActivityInfoByUs> activeInfoList =
                convertActiveToSimpleInfoList(active, usIdSet);
            resultList.addAll(activeInfoList);
        }

        // 返回响应
        _committer.commitSucRes(
            NP2SS_RB_Writer_001_CrossServerGroupOp.make_015_QueryPendingActivityByUsAndActivityId(resultList)
        );
    }

    /**
     * 检查待激活排期的groupList中是否包含指定usId集合中的任意一个
     *
     * @param pending 待激活排期数据
     * @param usIdSet UsId集合
     * @return 包含的usId列表
     */
    private List<Integer> containsAnyUsIdInPending(ActivitySchedulePendingData pending, Set<Integer> usIdSet)
    {
        if (pending.getGroupData() == null)
        {
            return null;
        }

        List<Integer> usIdList = new ArrayList<>();
        for (Schedule_GroupInfo groupInfo : pending.getGroupData().getGroupList())
        {
            for (Common_IntList usGroup : groupInfo.getUsGroupList())
            {
                for (int usId : usGroup.getValueList())
                {
                    if (usIdSet.contains(usId))
                    {
                        usIdList.add(usId);
                    }
                }
            }
        }

        return usIdList;
    }

    /**
     * 将待激活排期数据转换为简化信息格式
     * @param pending 待激活排期数据
     * @param usId US ID
     * @return 简化格式的活动信息
     */
    private Schedule_SimpleActivityInfoByUs convertPendingToSimpleInfo(ActivitySchedulePendingData pending, Integer usId)
    {
        Schedule_SimpleActivityInfoByUs result = new Schedule_SimpleActivityInfoByUs();
        result.setUsId(usId);
        Schedule_SimpleActivityInfo info = new Schedule_SimpleActivityInfo();
        info.setActivityId(pending.getBO().getActivityId());
        info.setLaunchId(pending.getBO().getPhpScheduleId());
        info.setStartTimeMs(pending.getBO().getStartTimeMs());
        info.setEndTimeMs(pending.getBO().getEndTimeMs());
        info.setCloseTimeMs(pending.getBO().getCloseTimeMs());
        info.setState(4);  // 4-排期中状态
        result.setActivityInfo(info);
        return result;
    }

    /**
     * 将已激活排期数据转换为简化信息格式列表
     *
     * @param active 已激活排期数据
     * @param usIdSet 要查询的UsId集合
     * @return 简化格式的活动信息列表
     */
    private List<Schedule_SimpleActivityInfoByUs> convertActiveToSimpleInfoList(
        ActivityScheduleData active, Set<Integer> usIdSet)
    {
        List<Schedule_SimpleActivityInfoByUs> resultList = new ArrayList<>();

        // 获取所有未完成的US信息列表（一次性获取，避免多次循环）
        List<ActivityScheduleUsInfo> allUsInfoList = active.getAllUsInfoList();

        // 筛选出在usIdSet中的US
        for (ActivityScheduleUsInfo usInfo : allUsInfoList)
        {
            if (!usIdSet.contains(usInfo.getUsId()))
            {
                continue;
            }

            Schedule_SimpleActivityInfoByUs result = new Schedule_SimpleActivityInfoByUs();
            result.setUsId(usInfo.getUsId());

            Schedule_SimpleActivityInfo info = new Schedule_SimpleActivityInfo();
            info.setActivityId(active.getBO().getActivityId());
            info.setLaunchId(active.getBO().getActivityId());
            info.setStartTimeMs(active.getBO().getStartTimeMs());
            info.setEndTimeMs(active.getBO().getEndTimeMs());
            info.setCloseTimeMs(active.getBO().getCloseTimeMs());

            // 根据US状态字段判断活动状态
            int state;
            if (!usInfo.hadPush())
            {
                state = 4;  // 4-排期中（已激活但未推送）
            }
            else if (usInfo.hadDone())
            {
                state = 3;  // 3-已关闭
            }
            else if (usInfo.hadEnterSettle())
            {
                state = 2;  // 2-展示中（结算中）
            }
            else if (usInfo.hadEnterPlaying())
            {
                state = 1;  // 1-进行中
            }
            else
            {
                state = 0;  // 0-预热（已推送但未开启）
            }
            info.setState(state);

            result.setActivityInfo(info);
            resultList.add(result);
        }

        return resultList;
    }
}
