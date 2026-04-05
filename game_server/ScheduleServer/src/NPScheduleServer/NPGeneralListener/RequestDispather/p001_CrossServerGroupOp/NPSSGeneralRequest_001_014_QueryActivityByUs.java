package NPScheduleServer.NPGeneralListener.RequestDispather.p001_CrossServerGroupOp;

import Common.Common_IntList;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_SimpleActivityInfo;
import NP2SS_R.p001_ScheduleOp.ToSS_R_001_014_QueryActivityByUs;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleData;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleDataMgr;
import NPScheduleServer.ActivityScheduleMgr.ActivitySchedulePendingData;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleUsInfo;
import NPScheduleServer.NPGeneralListener.RequestDispather.Writer.NP2SS_RB_Writer_001_CrossServerGroupOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.ArrayList;
import java.util.List;

/**
 * 查询指定UserServer的排期列表处理器
 *
 * 功能：遍历待激活和已激活排期列表，筛选出包含目标usId的排期数据
 *
 * 执行流程：
 * 1. 获取所有待激活排期数据并筛选
 * 2. 获取所有已激活排期数据并筛选
 * 3. 将符合条件的排期转换为简化格式
 * 4. 返回筛选结果
 */
public class NPSSGeneralRequest_001_014_QueryActivityByUs extends NPRequestDealer<ToSS_R_001_014_QueryActivityByUs>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter,
                               ToSS_R_001_014_QueryActivityByUs _msg)
    {
        List<Schedule_SimpleActivityInfo> result = new ArrayList<>();

        // 1. 获取所有待激活排期
        List<ActivitySchedulePendingData> pendingList =
            ActivityScheduleDataMgr.getInstance().getPendingDataList();

        // 筛选包含目标usId的待激活排期
        for (ActivitySchedulePendingData pending : pendingList)
        {
            if (containsUsIdInPending(pending, _msg.getUsId()))
            {
                Schedule_SimpleActivityInfo simpleInfo = convertPendingToSimpleInfo(pending);
                result.add(simpleInfo);
            }
        }

        // 2. 获取所有已激活排期
        List<ActivityScheduleData> activeList = ActivityScheduleDataMgr.getInstance().getActiveDataList();

        // 筛选包含目标usId且已推送的已激活排期
        for (ActivityScheduleData active : activeList)
        {
            ActivityScheduleUsInfo usInfo = active.lookupUsInfo(_msg.getUsId());
            // 只返回已推送且未完成的排期
            if (usInfo != null && usInfo.hadPush() && !usInfo.hadDone())
            {
                Schedule_SimpleActivityInfo simpleInfo = convertActiveToSimpleInfo(active, usInfo);
                result.add(simpleInfo);
            }
        }

        _commiter.commitSucRes(
            NP2SS_RB_Writer_001_CrossServerGroupOp.make_014_QueryActivityByUs(result)
        );
    }

    /**
     * 检查待激活排期是否包含指定UserServer
     *
     * @param pending 待激活排期数据
     * @param usId    目标UserServer ID
     * @return true-包含，false-不包含
     */
    private boolean containsUsIdInPending(ActivitySchedulePendingData pending, int usId)
    {
        if (pending.getGroupData() == null)
            return false;

        // 遍历所有分组
        for (Schedule_GroupInfo groupInfo : pending.getGroupData().getGroupList())
        {
            // 遍历分组中的所有UserServer列表
            for (Common_IntList usGroup : groupInfo.getUsGroupList())
            {
                if (usGroup.getValueList().contains(usId))
                {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * 转换待激活排期为简化活动信息格式
     *
     * @param pending 待激活排期数据
     * @return 简化格式的活动信息
     */
    private Schedule_SimpleActivityInfo convertPendingToSimpleInfo(ActivitySchedulePendingData pending)
    {
        Schedule_SimpleActivityInfo info = new Schedule_SimpleActivityInfo();
        info.setActivityId(pending.getBO().getActivityId());
        info.setLaunchId(pending.getBO().getActivityId());
        info.setStartTimeMs(pending.getBO().getStartTimeMs());
        info.setEndTimeMs(pending.getBO().getEndTimeMs());
        info.setCloseTimeMs(pending.getBO().getCloseTimeMs());
        info.setState(4);  // 4-排期中状态
        return info;
    }

    /**
     * 转换已激活排期为简化活动信息格式
     *
     * @param active 已激活排期数据
     * @param usInfo US信息对象
     * @return 简化格式的活动信息
     */
    private Schedule_SimpleActivityInfo convertActiveToSimpleInfo(ActivityScheduleData active, ActivityScheduleUsInfo usInfo)
    {
        Schedule_SimpleActivityInfo info = new Schedule_SimpleActivityInfo();
        info.setActivityId(active.getBO().getActivityId());
        info.setLaunchId(active.getBO().getActivityId());
        info.setStartTimeMs(active.getBO().getStartTimeMs());
        info.setEndTimeMs(active.getBO().getEndTimeMs());
        info.setCloseTimeMs(active.getBO().getCloseTimeMs());

        // 根据US状态字段判断活动状态
        // 优先级：已完成 > 已结算 > 已开启 > 预热
        int state;
        if (usInfo.hadDone())
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

        return info;
    }
}
