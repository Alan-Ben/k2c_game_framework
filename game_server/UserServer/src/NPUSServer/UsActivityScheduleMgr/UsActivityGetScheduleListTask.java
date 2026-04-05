package NPUSServer.UsActivityScheduleMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import Common.ScheduleObj.Schedule_UsPushData;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_011_GetUsActivityScheduleList;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_011_GetUsActivityScheduleList;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 从SS服务器获取排期列表数据，获取失败不做重试
 * @author mj
 */
public class UsActivityGetScheduleListTask implements _IALSynTask
{
    private NPUserServer _m_usServer;

    public UsActivityGetScheduleListTask(NPUserServer usServer)
    {
        _m_usServer = usServer;
    }

    @Override
    public void run()
    {
        NP2SS_R_001_011_GetUsActivityScheduleList proto = new NP2SS_R_001_011_GetUsActivityScheduleList();
        proto.setUsId(_m_usServer.getServerTypeId());

        _m_usServer.sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.SCHEDULE.ordinal(), proto, new _IWCGCallbackDealer()
        {
            @Override
            public void dealSuc(_IALProtocolStructure _proto)
            {
                NP2SS_RB_001_011_GetUsActivityScheduleList ret = (NP2SS_RB_001_011_GetUsActivityScheduleList) _proto;

                for (Schedule_UsPushData scheduleUsPush : ret.getDataList())
                {
                    _m_usServer.getUsActivityScheduleMgr().onScheduleInfoPush(scheduleUsPush);
                }
            }

            @Override
            public void dealFail(int _errorCode)
            {
                USLog.error(_m_usServer, "UsActivityGetScheduleListTask dealFail errorCode:{}", _errorCode);

                //失败后3秒重试
                ALSynTaskManager.getInstance().regTask(UsActivityGetScheduleListTask.this, 3000);
            }

            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2SS_RB_001_011_GetUsActivityScheduleList();
            }
        });
    }
}
