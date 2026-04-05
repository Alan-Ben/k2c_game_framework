package NPScheduleServer.CrossServerGroup.WorkGroup.Task;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess._ITALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2US_R.p008_ScheduleOp.NP2US_R_008_001_ChgCrossServerGroup;
import NP2US_RB.p008_ScheduleOp.NP2US_RB_008_001_ChgCrossServerGroup;
import NPCommon.Log.CommLog;
import NPScheduleServer.CrossServerGroup.CrossServerGroupItem;
import NPScheduleServer.NPScheduleServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 跨服分组信息推送任务
 * 用于推送跨服分组信息给分组下的对应US，如果失败下有重试机制，如果超过重试次数则钉钉预警
 */
public class CrossServerGroupInfoPushTask implements _IALSynTask
{
    //跨服分组信息对象
    private CrossServerGroupItem _m_groupItem;
    //推送目标服务器
    private int _m_usId;
    //失败重试次数(用于预警)
    private int _m_failCount;
    //process回调
    private _ITALProcessAction<Boolean> _m_action;

    public CrossServerGroupInfoPushTask(CrossServerGroupItem _groupItem, Integer _usId, _ITALProcessAction<Boolean> _action)
    {
        _m_groupItem = _groupItem;
        _m_usId = _usId;
        _m_action = _action;
    }

    @Override
    public void run()
    {
        NPScheduleServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), _m_usId, new NP2US_R_008_001_ChgCrossServerGroup(_m_groupItem.toProto()), new _IWCGCallbackDealer()
        {
            @Override
            public _IALProtocolStructure createProtocolObj()
            {
                return new NP2US_RB_008_001_ChgCrossServerGroup();
            }

            @Override
            public void dealSuc(_IALProtocolStructure _retMsg)
            {
                _m_action.dealAction(true);
            }

            @Override
            public void dealFail(int _errorCode)
            {
                _m_failCount++;
                if (_m_failCount > 5)
                {
                    CommLog.error("CrossServerSchedulePushTask push fail groupId:{} targetUsId:{}", _m_groupItem.getGroupId(), _m_usId);
                    _m_action.dealAction(true);
                    return;
                }

                ALSynTaskManager.getInstance().regTask(CrossServerGroupInfoPushTask.this, 5000);
            }
        });
    }
}
