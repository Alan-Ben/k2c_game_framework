package NPUSServer.ActivityPlan;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.Common_IntList;
import Common.ScheduleObj.Schedule_ActivityInfo;
import Common.ScheduleObj.Schedule_GroupInfo;
import Common.ScheduleObj.Schedule_PhpInfo;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_013_CmdAddActivitySchedule;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_013_CmdAddActivitySchedule;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.ActivityPlanBO;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class ActivityPlanInfo
{
    private ActivityPlanMgr _m_mgr;
    private ActivityPlanBO _m_bo;
    private boolean _m_isProcessing;

    public ActivityPlanInfo(ActivityPlanMgr _mgr, ActivityPlanBO _bo)
    {
        _m_mgr = _mgr;
        _m_bo = _bo;
    }

    public long getPlanId()
    {
        return _m_bo.getId();
    }

    /**
     * 获取活动开启时间
     * @return
     */
    public long getStartTimeMs()
    {
        return _m_bo.getStartMs();
    }

    /**
     * 判断是否需要处理
     * @param _nowTimeMs
     * @return
     */
    public boolean needDeal(long _nowTimeMs)
    {
        //判断是否已经处理
        if (_m_bo.getTriggerMs() > 0)
            return false;

        //判断是否禁用
        if (_m_bo.getDisable())
            return false;

        //判断是否在处理
        if (_m_isProcessing)
            return false;

        //判断是否到了开始时间（提前30秒）
        return _m_bo.getStartMs() < _nowTimeMs + 30 * 1000;
    }

    /**
     * 处理活动计划
     */
    public void deal()
    {
        //判断是否正在处理
        if (_m_isProcessing)
            return;

        _m_isProcessing = true;

        NPUserServer server = _m_mgr.getServer();

        Schedule_PhpInfo scheduleInfo = new Schedule_PhpInfo();
        scheduleInfo.setPhpScheduleId(-1);
        scheduleInfo.setPrePushTimeMs(0);

        Schedule_ActivityInfo scheduleActivity = new Schedule_ActivityInfo();
        scheduleActivity.setActivityId(_m_bo.getActivityId());
        scheduleActivity.setStartTimeMs(_m_bo.getStartMs());
        scheduleActivity.setEndTimeMs(_m_bo.getEndMs());
        scheduleActivity.setCloseTimeMs(_m_bo.getCloseMs());
        scheduleInfo.setActivity(scheduleActivity);

        Schedule_GroupInfo scheduleGroupInfo = new Schedule_GroupInfo();
        Common_IntList usIdList = new Common_IntList();
        usIdList.addValueList(server.getServerTypeId());
        scheduleGroupInfo.addUsGroupList(usIdList);

        scheduleInfo.addGroupList(scheduleGroupInfo);

        //发送到ScheduleServer
        server.sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SCHEDULE.ordinal(),
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
                        USLog.info("ActivityScheduleInfo dealSuc, activityId:{} startMs:{} endMs:{} closeMs:{}",
                                _m_bo.getActivityId(), _m_bo.getStartMs(), _m_bo.getEndMs(), _m_bo.getCloseMs());

                        //更新计划状态
                        _m_bo.saveTriggerMs(_m_mgr.getServer().getBM(), CommonFunc.getNowTimeMS());
                        _m_isProcessing = false;
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error("ActivityScheduleInfo dealFail, activityScheduleId:{} errCode:{}", _m_bo.getId(), _errCode);
                        _m_isProcessing = false;
                    }
                });
    }

    /**
     * 设置禁用状态
     * @param _state
     */
    public void setDisableState(boolean _state)
    {
        _m_bo.saveDisable(_m_mgr.getServer().getBM(), _state);
        USLog.info(_m_mgr.getServer(), "ActivityScheduleInfo setDisableState {}", toString());
    }

    /**
     * 修改活动计划
     * @param _startTime
     * @param _endTime
     * @param _closeTime
     * @return
     */
    public Result modify(long _startTime, long _endTime, long _closeTime)
    {
        //判断是否在处理
        if (_m_isProcessing)
        {
            USLog.error(_m_mgr.getServer(), "ActivityScheduleInfo modify fail, already processing planId:{}", getPlanId());
            return CommErr.OP_DISABLE;
        }

        //判断是否已处理
        if (_m_bo.getTriggerMs() > 0)
        {
            USLog.error(_m_mgr.getServer(), "ActivityScheduleInfo modify fail, already processed planId:{}", getPlanId());
            return CommErr.OP_DISABLE;
        }

        //修改数据
        _m_bo.setStartMs(_m_mgr.getServer().getBM(), _startTime);
        _m_bo.setEndMs(_m_mgr.getServer().getBM(), _endTime);
        _m_bo.setCloseMs(_m_mgr.getServer().getBM(), _closeTime);
        _m_bo.saveAllMarked(_m_mgr.getServer().getBM());

        USLog.info(_m_mgr.getServer(), "ActivityScheduleInfo modify {}, startMs:{} endMs:{} closeMs:{}", toString(),
                CommonFunc.getTimeStringMs(_startTime), CommonFunc.getTimeStringMs(_endTime), CommonFunc.getTimeStringMs(_closeTime));

        return null;
    }

    /**
     * 销毁对象
     */
    public void discard()
    {
        _m_bo.del(_m_mgr.getServer().getBM());
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("ActivityPlanInfo: ");
        sb.append("planId: ").append(_m_bo.getId()).append(", ");
        sb.append("disable: ").append(_m_bo.getDisable()).append(", ");
        sb.append("planRefId: ").append(_m_bo.getPlanRefId()).append(", ");
        sb.append("activityId: ").append(_m_bo.getActivityId()).append(", ");
        sb.append("triggerMs: ").append(_m_bo.getTriggerMs() == 0 ? "not_deal" : CommonFunc.getTimeStringMs(_m_bo.getTriggerMs())).append(", ");
        sb.append("isProcessing: ").append(_m_isProcessing).append(", ");
        sb.append("startMs: ").append(CommonFunc.getTimeStringMs(_m_bo.getStartMs())).append(", ");
        sb.append("endMs: ").append(CommonFunc.getTimeStringMs(_m_bo.getEndMs())).append(", ");
        sb.append("closeMs: ").append(CommonFunc.getTimeStringMs(_m_bo.getCloseMs()));
        return sb.toString();
    }
}
