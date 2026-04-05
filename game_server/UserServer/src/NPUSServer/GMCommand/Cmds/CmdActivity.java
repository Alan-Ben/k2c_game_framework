package NPUSServer.GMCommand.Cmds;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.Common_IntList;
import Common.ScheduleObj.Schedule_ActivityInfo;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_PhpInfo;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_013_CmdAddActivitySchedule;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_013_CmdAddActivitySchedule;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.GMCommand.UsCmdBase;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;

/**
 * @description: 活动作弊命令
 */
@ACommander(comment = "活动系统作弊命令", name = "activity")
public class CmdActivity extends UsCmdBase
{
    @ACommand(comment = "获取全部活动列表")
    public String lookupAll()
    {
        StringBuilder sb = new StringBuilder();

        ArrayList<_AActivityBase> list = getUserServer().getCommActivityMgr().lookupAllActivity();
        sb.append("activity size:").append(list.size()).append("\n");

        for (_AActivityBase activity : list)
        {
            if (null == activity)
                continue;

            sb.append("=====================================\n");
            sb.append(activity.toString());
        }

        return sb.toString();
    }

    @ACommand(comment = "开启活动[活动ID,延迟开启时间,活动时长,领奖时间]")
    public void start(long _activityId, int _delayOpenSec, int _playingSec, int _frozenSec)
    {
        Common_IntList usIdList = new Common_IntList();
        usIdList.addValueList(getUserServer().getServerTypeId());
        sendActivitySchedule(_activityId, _delayOpenSec, _playingSec, _frozenSec, usIdList);
    }

    @ACommand(comment = "开启多服活动[活动ID,延迟开启时间,活动时长,领奖时间,US列表(逗号分隔如1,2,3)]")
    public void startMultiUs(long _activityId, int _delayOpenSec, int _playingSec, int _frozenSec, String _usIds)
    {
        Common_IntList usIdList = new Common_IntList();
        for (String part : _usIds.split(","))
        {
            usIdList.addValueList(Integer.parseInt(part.trim()));
        }
        sendActivitySchedule(_activityId, _delayOpenSec, _playingSec, _frozenSec, usIdList);
    }

    /** 构造活动调度信息并发送到ScheduleServer */
    private void sendActivitySchedule(long _activityId, int _delayOpenSec, int _playingSec, int _frozenSec, Common_IntList _usIdList)
    {
        Schedule_PhpInfo scheduleInfo = new Schedule_PhpInfo();
        scheduleInfo.setPhpScheduleId(-1);
        scheduleInfo.setPrePushTimeMs(0);

        Schedule_ActivityInfo scheduleActivity = new Schedule_ActivityInfo();
        scheduleActivity.setActivityId(_activityId);
        long startTimeMs = (CommonFunc.getNowTimeSec() + _delayOpenSec) * 1000L;
        scheduleActivity.setStartTimeMs(startTimeMs);
        long endTimeMs = startTimeMs + _playingSec * 1000L;
        scheduleActivity.setEndTimeMs(endTimeMs);
        long closeTimeMs = endTimeMs + _frozenSec * 1000L;
        scheduleActivity.setCloseTimeMs(closeTimeMs);
        scheduleInfo.setActivity(scheduleActivity);

        Schedule_GroupInfo scheduleGroupInfo = new Schedule_GroupInfo();
        scheduleGroupInfo.addUsGroupList(_usIdList);
        scheduleInfo.addGroupList(scheduleGroupInfo);

        //发送到ScheduleServer
        getUserServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SCHEDULE.ordinal(),
                new NP2SS_R_001_013_CmdAddActivitySchedule(scheduleInfo, true), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2SS_RB_001_013_CmdAddActivitySchedule();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {
                        takeCallBack().onRunOver(true, "insert activity schedule success");
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        takeCallBack().onRunOver(false, "insert activity schedule fail, errCode:" + _errCode);
                    }
                });
    }

    @ACommand(comment = "设置活动结算[活动实例ID]")
    public String setSettling(long _instanceId)
    {
        boolean res = getUserServer().getCommActivityMgr().cmdSettle(_instanceId, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "设置活动领奖[活动实例ID]")
    public String setRewarding(long _instanceId)
    {
        boolean res = getUserServer().getCommActivityMgr().cmdReward(_instanceId, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "设置活动关闭[活动实例ID]")
    public String setClose(long _instanceId)
    {
        boolean res = getUserServer().getCommActivityMgr().cmdClose(_instanceId, getContext());

        return res ? "ok" : "fail";
    }

    @ACommand(comment = "移除未开启活动[活动实例ID]")
    public String removeNotStartActivity(long _instanceId)
    {
        boolean res = getUserServer().getCommActivityMgr().cmdRemoveNotStartActivity(_instanceId, getContext());

        return res ? "ok" : "fail";
    }
    
    @ACommand(comment = "获取活动结算公会数据[活动实例ID，公会ID]")
    public String getSettleGuild(long _instanceId, long _guildId)
    {
    	_AActivityBase activity = getUserServer().getCommActivityMgr().lookupActivity(_instanceId);
    	if(null == activity)
    		return "fail";
    	
    	return "" + activity.getSettleGuildMgr().getLeaderCid(_guildId);
    }
}
