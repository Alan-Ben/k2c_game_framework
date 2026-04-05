package NPScheduleServer.ActivityScheduleMgr.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import Common.ScheduleObj.Schedule_UsPushData;
import NP2US_R.p008_ScheduleOp.NP2US_R_008_010_PushSchedule;
import NP2US_RB.p008_ScheduleOp.NP2US_RB_008_010_PushSchedule;
import NPCommon.Log.CommLog;
import NPScheduleServer.ActivityScheduleMgr.ActivityScheduleUsInfo;
import NPScheduleServer.NPScheduleServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 排期信息推送任务
 * 用于推送活动排期信息到US, 若失败则重试5次, 若还不成功, 则输出报警, 等待US自己拉取
 */
public class ActivityScheduleResUpdatePushTask implements _IALSynTask
{
    //排期信息
    private Schedule_UsPushData _m_scheduleInfo;
    //推送目标服务器
    private ActivityScheduleUsInfo _m_usInfo;
    //失败重试次数(用于预警)
    private int _m_failCount;

    public ActivityScheduleResUpdatePushTask(Schedule_UsPushData _scheduleInfo, ActivityScheduleUsInfo _usInfo)
    {
        _m_scheduleInfo = _scheduleInfo;
        _m_usInfo = _usInfo;
    }

    @Override
    public void run()
    {
        NP2US_R_008_010_PushSchedule proto = new NP2US_R_008_010_PushSchedule();
        proto.setUsSchedule(_m_scheduleInfo);

        NPScheduleServer.getInstance().sendRequestToBSServer(
                EServerType.USER.ordinal(), _m_usInfo.getUsId(), proto, new _IWCGCallbackDealer()
        {
            @Override
            public void dealSuc(_IALProtocolStructure _proto)
            {
            }

            @Override
            public void dealFail(int _errCode)
            {
                _m_failCount++;
                if (_m_failCount > 5)
                {
                    CommLog.error("scheduleId:{} res update to US:{} fail.", _m_scheduleInfo.getScheduleId(), _m_usInfo.getUsId());
                    return;
                }

                // 检查排期信息是否变更 如果变更则直接设置成功
                if (_m_scheduleInfo.getSubmitCount() != _m_usInfo.getUsGroup().getScheduleData().getSubmitCount())
                {
                    CommLog.info("scheduleId:{} res update push to US:{} submitCount changed, get new schedule info.",
                            _m_scheduleInfo.getScheduleId(), _m_usInfo.getUsId());
                    return;
                }

                ALSynTaskManager.getInstance().regTask(ActivityScheduleResUpdatePushTask.this, 5000);
            }

            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2US_RB_008_010_PushSchedule();
            }
        });
    }
}
