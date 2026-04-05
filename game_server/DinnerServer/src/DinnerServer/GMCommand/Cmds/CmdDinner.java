package DinnerServer.GMCommand.Cmds;

import DinnerServer.DinnerPool.DinnerCrossGroup;
import DinnerServer.DinnerPool.DinnerCrossGroupMgr;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;

/**
 *
 */
@ACommander(comment = "宴会相关命令", name = "dinner")
public class CmdDinner extends CmdClassBase
{
	@ACommand(comment = "修改分组[分组ID 0-本地分组]")
    public String lookupGroup(long _groupId)
    {
		DinnerCrossGroup pool = DinnerCrossGroupMgr.getInstance().lookup(_groupId);
		if(null == pool)
			return "fail, not find group";
		
    	return pool.toString();
    }
}
