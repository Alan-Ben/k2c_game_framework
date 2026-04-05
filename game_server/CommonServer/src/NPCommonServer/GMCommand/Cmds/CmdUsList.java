package NPCommonServer.GMCommand.Cmds;

import Common.NpServerObj.NpServerObj_SYS_ServerItem;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommonServer.GMCommand.CSCmdBase;
import NPCommonServer.USServerHandleUserMgr.USServerHandleUserMgr;
import NPCommonServer.USServerListMgr.USServerListMgrCSInstance;
import NPEnum.EServerOnlineState;

@ACommander(comment = "us列表相关命令", name = "usList")
public class CmdUsList extends CSCmdBase
{
    @ACommand(comment = "US服务器列表")
    public String USList()
    {
        return USServerListMgrCSInstance.getInstance().toString();
    }

    @ACommand(comment = "US服务器负载列表")
    public String USLoadList()
    {
        return USServerHandleUserMgr.getInstance().toString();
    }

    @ACommand(comment = "改变us状态[usId][状态OPEN;CLOSED;TEMP_CLOSED]")
    public String chgState(int _typeId, EServerOnlineState _state)
    {
        NpServerObj_SYS_ServerItem serverItem = USServerListMgrCSInstance.getInstance().lookupServerInfoByTypeId(_typeId);
        if (serverItem == null)
            return "server item not found";

        NpServerObj_SYS_ServerItem newItem = new NpServerObj_SYS_ServerItem();
        newItem.setAreaTag(serverItem.getAreaTag());
        newItem.setGroupId(serverItem.getGroupId());
        newItem.setServerTypeId(serverItem.getServerTypeId());
        newItem.setServerName(serverItem.getServerName());
        newItem.setOnlineStateTypeId(_state.ordinal());
        newItem.setShowStateTypeId(serverItem.getShowStateTypeId());
        newItem.setIsNew(serverItem.getIsNew());
        newItem.setStartDate(serverItem.getStartDate());
        newItem.setExt(serverItem.getExt());

        USServerListMgrCSInstance.getInstance().updateServerItem(newItem);

        return "ok";
    }
}
