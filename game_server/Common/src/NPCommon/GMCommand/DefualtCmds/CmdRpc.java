package NPCommon.GMCommand.DefualtCmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import RPC.RPCDataFactoryMgr;
import RPC.RpcDispatcher;
import RPC._ARPCData;
import RPC._ARpcLogger;

import java.util.Comparator;
import java.util.List;

@ACommander(comment = "rpc相关命令", name = "rpc")
public class CmdRpc extends CmdClassBase
{
    @ACommand(comment = "记录Rpc日志(RpcClassId)")
    public String addLog(int _classId)
    {
        RpcDispatcher dispatcher = RpcDispatcher.getGlobalRpcDispatcher();
        if (dispatcher == null)
        {
            return "no rpc dispatcher!";
        }
        if (null == dispatcher.getLogger())
        {
            return "no logger!";
        }
        dispatcher.getLogger().addClassId(_classId);
        return "ok";
    }

    @ACommand(comment = "记录Rpc日志(className)")
    public String addLogName(String _className)
    {
        RpcDispatcher dispatcher = RpcDispatcher.getGlobalRpcDispatcher();
        if (dispatcher == null)
        {
            return "no rpc dispatcher!";
        }
        if (null == dispatcher.getLogger())
        {
            return "no logger!";
        }

        int classId = RPCDataFactoryMgr.getInstance().getClassId(_className);
        if (classId <= 0)
            return "not found class id for name:" + _className;
        dispatcher.getLogger().addClassId(classId);
        return "ok";
    }

    @ACommand(comment = "移除Rpc日志(RpcClassId)")
    public String removeLog(int _classId)
    {
        RpcDispatcher dispatcher = RpcDispatcher.getGlobalRpcDispatcher();
        if (dispatcher == null)
        {
            return "no rpc dispatcher!";
        }
        if (null == dispatcher.getLogger())
        {
            return "no logger!";
        }
        dispatcher.getLogger().removeClassId(_classId);
        return "ok";
    }

    @ACommand(comment = "记录Rpc日志(className)")
    public String removeLogName(String _className)
    {
        RpcDispatcher dispatcher = RpcDispatcher.getGlobalRpcDispatcher();
        if (dispatcher == null)
        {
            return "no rpc dispatcher!";
        }
        if (null == dispatcher.getLogger())
        {
            return "no logger!";
        }

        int classId = RPCDataFactoryMgr.getInstance().getClassId(_className);
        if (classId <= 0)
        {
            return "invalid class name:" + _className;
        }
        dispatcher.getLogger().removeClassId(classId);
        return "ok";
    }

    @ACommand(comment = "显示日志Id列表")
    public String listLog()
    {
        RpcDispatcher dispatcher = RpcDispatcher.getGlobalRpcDispatcher();
        if (dispatcher == null)
        {
            return "no rpc dispatcher!";
        }
        if (null == dispatcher.getLogger())
        {
            return "no logger!";
        }

        _ARpcLogger logger = dispatcher.getLogger();
        StringBuilder sb = new StringBuilder();
        for (Integer classId : logger.getIdList())
        {
            Class<? extends _ARPCData> rpcClass = RPCDataFactoryMgr.getInstance().getRpcClass(classId);
            sb.append(String.format("%s\t(%d)\n", rpcClass.getSimpleName(), classId));
        }
        return sb.toString();
    }

    @ACommand(comment = "显示全部Id列表")
    public String listId()
    {
        StringBuilder sb = new StringBuilder();
        List<Integer> rpcIdList = RPCDataFactoryMgr.getInstance().getRpcIdList();
        rpcIdList.sort(Comparator.comparingInt(o -> o));
        for (Integer classId : rpcIdList)
        {
            Class<? extends _ARPCData> rpcClass = RPCDataFactoryMgr.getInstance().getRpcClass(classId);
            sb.append(String.format("%s\t(%d)\n", rpcClass.getSimpleName(), classId));
        }
        return sb.toString();
    }
}
