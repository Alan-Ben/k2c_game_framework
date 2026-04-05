package NPScheduleServer.ActivityScheduleMgr.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2US_R.p008_ScheduleOp.NP2US_R_008_002_NoticeScheduleCanSendReward;
import NP2US_RB.p008_ScheduleOp.NP2US_RB_008_002_NoticeScheduleCanSendReward;
import NPScheduleServer.NPScheduleServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 通知活动排行榜可以发放奖励
 */
public class NoticeCanSendRewardTask implements _IALSynTask
{
    private long _m_scheduleId;
    private int _m_usId;
    private int _m_failCount;

    public NoticeCanSendRewardTask(long _scheduleId, int _usId)
    {
        _m_scheduleId = _scheduleId;
        _m_usId = _usId;
    }

    @Override
    public void run()
    {
        NPScheduleServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_usId,
                new NP2US_R_008_002_NoticeScheduleCanSendReward(_m_scheduleId), new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2US_RB_008_002_NoticeScheduleCanSendReward();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _retMsg)
            {

            }

            @Override
            public void dealFail(int _errorCode)
            {
                _m_failCount++;
                if (_m_failCount > 5)
                {
                    NPScheduleServer.getInstance().getDDAlert().err("NoticeCanSendRewardTask push fail",
                            "NoticeCanSendRewardTask push fail scheduleId:{} targetUsId:{}", _m_scheduleId, _m_usId);
                    return;
                }

                ALSynTaskManager.getInstance().regTask(NoticeCanSendRewardTask.this, 5000);
            }
        });
    }
}
