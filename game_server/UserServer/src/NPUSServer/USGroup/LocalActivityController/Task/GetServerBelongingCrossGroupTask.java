package NPUSServer.USGroup.LocalActivityController.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2SS_R.p001_ScheduleOp.NP2SS_R_001_001_GetServerBelongingCrossGroup;
import NP2SS_RB.p001_ScheduleOp.NP2SS_RB_001_001_GetServerBelongingCrossGroup;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 查询服务器所在分组的异步任务
 * -----------------------
 * 请求失败：进行5s重试
 * 请求成功：若服务器有分组，则请求对应的排期信息
 */
public class GetServerBelongingCrossGroupTask implements _IALSynTask
{
    private NPUserServer _m_server;

    public GetServerBelongingCrossGroupTask(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        int usId = getUSServer().getServerTypeId();
        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.SCHEDULE.ordinal(), new NP2SS_R_001_001_GetServerBelongingCrossGroup(usId), new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2SS_RB_001_001_GetServerBelongingCrossGroup();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _retMsg)
            {
                NP2SS_RB_001_001_GetServerBelongingCrossGroup retMsg = (NP2SS_RB_001_001_GetServerBelongingCrossGroup) _retMsg;
                //如果服务器当前在分组内，才去做预备变更分组的操作
                if (retMsg.getInGroup())
                {
                    getUSServer().getLocalCrossServerGroupMgr().setPrepareChangeGroupInfo(retMsg.getGroupInfo());
                }
            }

            @Override
            public void dealFail(int _errorCode)
            {
                ALSynTaskManager.getInstance().regTask(GetServerBelongingCrossGroupTask.this, 5000);
            }
        });
    }
}
