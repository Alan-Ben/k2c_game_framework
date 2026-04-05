package NPUSServer.GMCommand.Cmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "政务相关命令", name = "anecdote")
public class CmdAnecdote extends UsCmdBase
{
    @ACommand(comment = "刷新政务事件[位置组id]")
    public String refreshPos(int _groupId)
    {
		getOwner().getAnecdoteComponent().refreshPosEvent(_groupId, getContext());
		return "done";
    }
}
