package NPScheduleServer.NPGeneralListener.RequestDispather.Writer;

import Common.NpServerObj.NPServerObj_CrossServerGroupInfo;
import Common.ScheduleObj.Schedule_SimpleActivityInfo;
import Common.ScheduleObj.Schedule_SimpleActivityInfoByUs;
import Common.ScheduleObj.Schedule_UsPushData;
import NP2SS_RB.p001_ScheduleOp.*;

import java.util.List;

public class NP2SS_RB_Writer_001_CrossServerGroupOp
{
    ///////////////////////////////////////////////// 分组系统相关协议 /////////////////////////////////////////////////
	
    public static NP2SS_RB_001_001_GetServerBelongingCrossGroup make_001_GetServerBelongingCrossGroup(boolean _inGroup, NPServerObj_CrossServerGroupInfo _groupInfo)
    {
        NP2SS_RB_001_001_GetServerBelongingCrossGroup protocol = new NP2SS_RB_001_001_GetServerBelongingCrossGroup();
        protocol.setInGroup(_inGroup);
        if (_groupInfo != null)
            protocol.setGroupInfo(_groupInfo);
        return protocol;
    }
    
    ///////////////////////////////////////////////// 排期系统相关协议 /////////////////////////////////////////////////
    
    public static NP2SS_RB_001_010_ReportUsActivityScheduleDone make_010_ReportUsActivityScheduleDone()
    {
        return new NP2SS_RB_001_010_ReportUsActivityScheduleDone();
    }
    
    public static NP2SS_RB_001_011_GetUsActivityScheduleList make_011_GetUsActivityScheduleList(List<Schedule_UsPushData> _scheduleList)
    {
    	NP2SS_RB_001_011_GetUsActivityScheduleList proto = new NP2SS_RB_001_011_GetUsActivityScheduleList();
        proto.getDataList().addAll(_scheduleList);
    	return proto;
    }

    public static NP2SS_RB_001_012_ReportUsActivityScheduleSettling make_012_ReportUsActivityScheduleSettling()
    {
        return new NP2SS_RB_001_012_ReportUsActivityScheduleSettling();
    }

    public static NP2SS_RB_001_013_CmdAddActivitySchedule make_013_CmdAddActivitySchedule()
    {
        return new NP2SS_RB_001_013_CmdAddActivitySchedule();
    }

    public static NP2SS_RB_001_016_ReportUsActivitySchedulePlaying make_016_ReportUsActivitySchedulePlaying()
    {
        return new NP2SS_RB_001_016_ReportUsActivitySchedulePlaying();
    }

    /**
     * 构造查询排期响应协议
     * @param activityList 排期列表（包含待激活和已激活）
     * @return 响应协议对象
     */
    public static ToSS_RB_001_014_QueryActivityByUs make_014_QueryActivityByUs(
        List<Schedule_SimpleActivityInfo> activityList)
    {
        ToSS_RB_001_014_QueryActivityByUs ret = new ToSS_RB_001_014_QueryActivityByUs();
        ret.getActivityList().addAll(activityList);
        return ret;
    }

    /**
     * 构造查询指定活动在US列表中的排期响应协议
     * @param activityList 活动列表
     * @return 响应协议对象
     */
    public static ToSS_RB_001_015_QueryActivityByUsAndActivityId make_015_QueryPendingActivityByUsAndActivityId(
        List<Schedule_SimpleActivityInfoByUs> activityList)
    {
        ToSS_RB_001_015_QueryActivityByUsAndActivityId ret = new ToSS_RB_001_015_QueryActivityByUsAndActivityId();
        ret.getActivityList().addAll(activityList);
        return ret;
    }

    /**
     * 构造推送资源更新信息响应协议
     * @return 响应协议对象
     */
    public static ToSS_RB_001_017_CmdPushResUpdateInfo make_017_CmdPushResUpdateInfo()
    {
        return new ToSS_RB_001_017_CmdPushResUpdateInfo();
    }
}
