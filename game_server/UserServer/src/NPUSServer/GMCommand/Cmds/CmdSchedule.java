package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "schedule相关命令", name = "schedule")
public class CmdSchedule extends UsCmdBase
{
	@ACommand(comment = "获取US全部排期数据")
    public String usSchedule()
    {
		return getUserServer().getUsActivityScheduleMgr().toString();
    }
}
