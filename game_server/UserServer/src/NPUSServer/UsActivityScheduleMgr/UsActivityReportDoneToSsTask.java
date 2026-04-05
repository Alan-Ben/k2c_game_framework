package NPUSServer.UsActivityScheduleMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_010_ReportUsActivityScheduleDone;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_010_ReportUsActivityScheduleDone;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 通知SS活动已完成的异步任务
 * @author mj
 */
public class UsActivityReportDoneToSsTask implements _IALSynTask
{
    private NPUserServer _m_server;

    private long _m_lScheduleId;
    private long _m_lUsGroupId;

    public UsActivityReportDoneToSsTask(NPUserServer _server, UsActivityScheduleInfo _usSchedule)
    {
        _m_server = _server;

        _m_lScheduleId = _usSchedule.getScheduleId();
        _m_lUsGroupId = _usSchedule.getUsGroupId();
    }

    public NPUserServer getUSServer()
    {
        return _m_server;
    }


    @Override
    public void run()
    {
        //数据已被移除，不再处理
        if (null == getUSServer().getUsActivityScheduleMgr().lookupSchedule(_m_lScheduleId))
            return;

        UsActivityReportDoneToSsTask task = this;

        NP2SS_R_001_010_ReportUsActivityScheduleDone proto = new NP2SS_R_001_010_ReportUsActivityScheduleDone();
        proto.setUsId(getUSServer().getServerTypeId());
        proto.setScheduleId(_m_lScheduleId);
        proto.setUsGroupId(_m_lUsGroupId);

        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.SCHEDULE.ordinal(), proto, new _IWCGCallbackDealer()
        {

            @Override
            public void dealSuc(_IALProtocolStructure param_IALProtocolStructure)
            {
                getUSServer().getUsActivityScheduleMgr().removeActivitySchedule(_m_lScheduleId);
            }

            @Override
            public void dealFail(int _errorCode)
            {
                USLog.error(getUSServer(), "UsActivityReportDoneToSsTask dealFail, scheduleId:{} errorCode:{}", _m_lScheduleId, _errorCode);

                //1秒后重试
                ALSynTaskManager.getInstance().regTask(task, 3000);
            }

            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2SS_RB_001_010_ReportUsActivityScheduleDone();
            }
        });
    }
}
