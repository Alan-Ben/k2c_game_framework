package NPScheduleServer.ActivityScheduleMgr.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess._ITALProcessAction;
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
public class ActivitySchedulePushTask implements _IALSynTask
{
    //排期信息
    private Schedule_UsPushData _m_scheduleInfo;
    //推送目标服务器
    private ActivityScheduleUsInfo _m_usInfo;
    //失败重试次数(用于预警)
    private int _m_failCount;
    //process回调
    private _ITALProcessAction<Boolean> _m_action;

    public ActivitySchedulePushTask(Schedule_UsPushData _scheduleInfo,
                                    ActivityScheduleUsInfo _usInfo, _ITALProcessAction<Boolean> _action)
    {
        _m_scheduleInfo = _scheduleInfo;
        _m_usInfo = _usInfo;
        _m_action = _action;
    }

    @Override
    public void run()
    {
        if (_m_usInfo.hadPush())
        {
            _m_action.dealAction(true);
            return;
        }

        NP2US_R_008_010_PushSchedule proto = new NP2US_R_008_010_PushSchedule();
        proto.setUsSchedule(_m_scheduleInfo);

        NPScheduleServer.getInstance().sendRequestToBSServer(
                EServerType.USER.ordinal(), _m_usInfo.getUsId(), proto, new _IWCGCallbackDealer()
        {
            @Override
            public void dealSuc(_IALProtocolStructure _proto)
            {
                _m_usInfo.markHadPush();

                _m_action.dealAction(true);
            }

            @Override
            public void dealFail(int _errCode)
            {
                _m_failCount++;
                if (_m_failCount > 5)
                {
                    CommLog.error("scheduleId:{} push to US:{} fail.", _m_scheduleInfo.getScheduleId(), _m_usInfo.getUsId());
                    _m_usInfo.markHadPush();
                    _m_action.dealAction(true);
                    return;
                }

                // 检查排期信息是否变更 如果变更则直接设置成功
                if (_m_scheduleInfo.getSubmitCount() != _m_usInfo.getUsGroup().getScheduleData().getSubmitCount())
                {
                    CommLog.info("scheduleId:{} push to US:{} submitCount changed, get new schedule info.",
                            _m_scheduleInfo.getScheduleId(), _m_usInfo.getUsId());
                    _m_usInfo.markHadPush();
                    _m_action.dealAction(true);
                    return;
                }

                ALSynTaskManager.getInstance().regTask(ActivitySchedulePushTask.this, 5000);
            }

            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2US_RB_008_010_PushSchedule();
            }
        });
    }
}
