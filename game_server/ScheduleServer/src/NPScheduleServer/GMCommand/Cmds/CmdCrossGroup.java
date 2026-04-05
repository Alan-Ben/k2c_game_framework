package NPScheduleServer.GMCommand.Cmds;

import Common.NpServerObj.NPServerObj_CrossServerGroupInfo;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPScheduleServer.CrossServerGroup.CrossServerGroupMgr;

/**
 *
 */
@ACommander(comment = "跨服分组", name = "crossGroup")
public class CmdCrossGroup extends CmdClassBase
{
    @ACommand(comment = "新增分组")
    public String add(long _groupId)
    {
        NPServerObj_CrossServerGroupInfo groupInfo = new NPServerObj_CrossServerGroupInfo();
        groupInfo.setGroupId(_groupId);
        groupInfo.getUsIdList().add(1);
        groupInfo.getUsIdList().add(2);

        return CrossServerGroupMgr.getInstance().getPrepareGroup().addGroup(groupInfo).toString();
    }

    @ACommand(comment = "激活分组")
    public String active()
    {
        return CrossServerGroupMgr.getInstance().activePrepareGroup().toString();
    }
}
