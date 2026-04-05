package NPScheduleServer.CrossServerGroup.WorkGroup.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess._ITALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2CS_R.np_p006_RankOp.NP2CS_R_006_001_ReqCrossInstance;
import NP2CS_RB.np_p006_RankOp.NP2CS_RB_006_001_RetCrossInstance;
import NPScheduleServer.CrossServerGroup.CrossServerGroupItem;
import NPScheduleServer.NPScheduleServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

/***
 * 请求跨服分组集群实例id的任务
 * 用于向跨服分组负载管理器请求集群实例id
 */
public class ApplyCrossRankInstanceIdTask implements _IALSynTask
{
    //跨服分组对象(用于在申请时传递负载信息)
    private CrossServerGroupItem _m_groupItem;
    //process对象(用于保证任务顺序完成)
    private _ITALProcessAction<Boolean> _m_action;
    //失败重试次数
    private int _m_failCount;

    public ApplyCrossRankInstanceIdTask(CrossServerGroupItem _groupItem, _ITALProcessAction<Boolean> _action)
    {
        _m_groupItem = _groupItem;
        _m_action = _action;
    }

    @Override
    public void run()
    {
        if (_m_groupItem.getCrsGroupInstanceId() != 0)
        {
            _m_action.dealAction(true);
            return;
        }

        if (_m_failCount > 5)
        {
            NPScheduleServer.getInstance().getDDAlert().err("ApplyCrossRankInstanceIdTask", "ApplyCrossRankInstanceIdTask tryHandle fail groupId:" + _m_groupItem.getGroupId());
            _m_action.dealAction(true);
            return;
        }

        //请求跨服排行榜分组实例id
        NPScheduleServer.getInstance().sendRequestToBSServer(NPEnum.EServerType.SINGLE.ordinal(), NPEnum.ENPSingleServerType.COMMON.ordinal(),
                new NP2CS_R_006_001_ReqCrossInstance(), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_006_001_RetCrossInstance();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _msg)
                    {
                        NP2CS_RB_006_001_RetCrossInstance msg = (NP2CS_RB_006_001_RetCrossInstance) _msg;
                        _m_groupItem.saveCrsGroupInstanceId(msg.getCrossInstanceId());
                        _m_action.dealAction(true);
                    }

                    @Override
                    public void dealFail(int _error)
                    {
                        _m_failCount++;
                        ALSynTaskManager.getInstance().regTask(ApplyCrossRankInstanceIdTask.this, 5000);
                    }
                });
    }
}
