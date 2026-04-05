package NPHttpServer.HsClientListener.p002_HsClientOp;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import Common.NpServerObj.NpServerObj_ServerInfo;
import NP2HS_R.p002_HsClientOp.NP2HS_R_002_001_ReqExecGmSuper;
import NP2HS_RB.p002_HsClientOp.ExeGmSuperResult;
import NP2HS_RB.p002_HsClientOp.NP2HS_RB_002_001_RetExecGmSuper;
import NP2PS_R.p001_BasicOp.NP2PS_R_001_011_ReqOnlineServerList;
import NP2PS_R.p001_BasicOp.NP2PS_R_001_012_ReqOnlineRefServerList;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_011_RetOnlineServerList;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_012_RetOnlineRefServerList;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPCommon.Log.CommLog;
import NPCommon.Promise.Promise;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.ProtoUtil;
import NPCommon.Util.ServerGMUtil;
import NPHttpServer.HsClientListener.HsClientListener;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;
import java.util.List;

public class NP2HS_R_002_001_ReqExecGmSuper_Handler extends NPCustomMsgDealer<NP2HS_R_002_001_ReqExecGmSuper>
{
    @Override
    protected void _dealMessage(_IALProtocolReceiver _receiver, NP2HS_R_002_001_ReqExecGmSuper _msg)
    {
        CommLog.info("received local client request:\n{}", ProtoUtil.getString(_msg));

        HsClientListener listener = (HsClientListener) _receiver;
        asyncLoadTaskList(_msg, new HandlerOne<List<GmCmdTask>>()
        {
            @Override
            public void handle(List<GmCmdTask> _taskList)
            {
                if (_taskList.isEmpty())
                {
                    NP2HS_RB_002_001_RetExecGmSuper proto = new NP2HS_RB_002_001_RetExecGmSuper();
                    ExeGmSuperResult _result = new ExeGmSuperResult();
                    _result.setIsSucc(false);
                    _result.setResult("can not get server list for type:" + _msg.getServerType());
                    proto.addResultList(_result);
                    listener.send(proto);
                    return;
                }

                _processTaskList(listener, _taskList);
            }
        });
    }

    protected void _processTaskList(HsClientListener listener, List<GmCmdTask> _taskList)
    {
        NP2HS_RB_002_001_RetExecGmSuper proto = new NP2HS_RB_002_001_RetExecGmSuper();
        Promise promise = new Promise();
        for (int i = 0; i < _taskList.size(); i++)
        {
            GmCmdTask task = _taskList.get(i);
            final int theIndex = i;
            promise.then(theIndex, (p) ->
            {
                ServerGMUtil.routeGMCommandDistinct(NPHttpServer.getInstance(), EServerType.EServerType_FromInt(task.serverType), task.serverTypeId, task.cmd, new HandlerTwo<Boolean, String>()
                {
                    @Override
                    public void handle(Boolean _isSucc, String _msg)
                    {
                        ExeGmSuperResult resultItem = new ExeGmSuperResult();
                        resultItem.setServerType(task.serverType);
                        resultItem.setServerTypeId(task.serverTypeId);
                        resultItem.setIsSucc(_isSucc);
                        resultItem.setResult(_msg);
                        proto.addResultList(resultItem);
                        p.commit(theIndex);
                    }
                });
            });
        }
        promise.over((p) ->
        {
            listener.send(proto);
        });

    }

    private static class GmCmdTask
    {
        public int serverType;
        public int serverTypeId;
        public String cmd;
    }

    /*********
     * 异步加载执行GM命令的任务，从PS服务器上获取服务器id列表。
     * @param _msg
     * @param _handler
     */
    private void asyncLoadTaskList(NP2HS_R_002_001_ReqExecGmSuper _msg, HandlerOne<List<GmCmdTask>> _handler)
    {
        List<GmCmdTask> taskList = new ArrayList<GmCmdTask>();

        if (_msg.getIsAllRef()) //针对需要执行 ref reload 的所有服务器列表
        {
            NP2PS_R_001_012_ReqOnlineRefServerList rpc = new NP2PS_R_001_012_ReqOnlineRefServerList();

            NPHttpServer.getInstance().sendRequestToPlat(rpc, new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2PS_RB_001_012_RetOnlineRefServerList();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retMsg)
                {
                    NP2PS_RB_001_012_RetOnlineRefServerList retMsg = (NP2PS_RB_001_012_RetOnlineRefServerList) _retMsg;
                    for (NpServerObj_ServerInfo info : retMsg.getRefServerList())
                    {
                        GmCmdTask task = new GmCmdTask();
                        task.serverType = info.getServerType();
                        task.serverTypeId = info.getTypeId();
                        task.cmd = _msg.getGmComamnd();
                        taskList.add(task);
                    }

                    _handler.handle(taskList);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    _handler.handle(taskList);
                }
            });

            return;
        }

        if (_msg.getServerType() == EServerType.SINGLE.ordinal())
        {
            GmCmdTask task = new GmCmdTask();
            task.serverType = EServerType.SINGLE.ordinal();
            task.serverTypeId = _msg.getServerIdList().get(0);
            task.cmd = _msg.getGmComamnd();
            taskList.add(task);
            _handler.handle(taskList);
            return;
        }
        if (!_msg.getIsAll())
        {
            for (int serverTypeId : _msg.getServerIdList())
            {
                GmCmdTask task = new GmCmdTask();
                task.serverType = _msg.getServerType();
                task.serverTypeId = serverTypeId;
                task.cmd = _msg.getGmComamnd();
                taskList.add(task);
            }
            _handler.handle(taskList);
        } else
        {
            NP2PS_R_001_011_ReqOnlineServerList rpc = new NP2PS_R_001_011_ReqOnlineServerList();
            rpc.setServerType(_msg.getServerType());

            NPHttpServer.getInstance().sendRequestToPlat(rpc, new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2PS_RB_001_011_RetOnlineServerList();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _retMsg)
                {
                    NP2PS_RB_001_011_RetOnlineServerList retMsg = (NP2PS_RB_001_011_RetOnlineServerList) _retMsg;
                    for (int typeId : retMsg.getServerTypeIdList())
                    {
                        GmCmdTask task = new GmCmdTask();
                        task.serverType = _msg.getServerType();
                        task.serverTypeId = typeId;
                        task.cmd = _msg.getGmComamnd();
                        taskList.add(task);
                    }
                    _handler.handle(taskList);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    _handler.handle(taskList);
                }
            });
        }

    }
}
