package NPRecordServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPRecordServer.NPRecordMgr.RecordInfoList;
import NPRecordServer.NPRecordMgr.RecordInfoListMgr;

@ACommander(comment = "server命令", name = "server")
public class CmdServer extends CmdClassBase
{
    @ACommand(comment = "显示服务器信息")
    public String info()
    {
        return "info";
    }

    @ACommand(comment = "获取某用户登录记录")
    public String get(String _accountId)
    {
        RecordInfoList recordInfoList = RecordInfoListMgr.getInstance().lookup(_accountId);
        if (recordInfoList == null)
        {
            return "";
        }
        return recordInfoList.toString();
    }
}
