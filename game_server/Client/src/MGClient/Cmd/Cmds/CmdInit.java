package MGClient.Cmd.Cmds;

import GC2GS.p002_InitOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "初始化", name = "init")
public class CmdInit extends CmdBase {
	@Command(comment = "宴会初始化")
	public void dinner()
	{
		GC2GS_002_060_ReqDinnerInit msg = new GC2GS_002_060_ReqDinnerInit();
		getOwner().sendGameMsg(msg);

	}

	@Command(comment = "好友初始化")
	public void friend()
	{
		GC2GS_002_049_ReqFriendInit msg = new GC2GS_002_049_ReqFriendInit();
		getOwner().sendGameMsg(msg);

	}	
	
	@Command(comment = "政务初始化")
	public void anecdote()
	{
		GC2GS_002_009_ReqAnecdoteInit msg = new GC2GS_002_009_ReqAnecdoteInit();
		getOwner().sendGameMsg(msg);

	}	
	
	@Command(comment = "征收初始化")
	public void levy()
	{
		GC2GS_002_016_ReqLevyInit msg = new GC2GS_002_016_ReqLevyInit();
		getOwner().sendGameMsg(msg);

	}
	
	@Command(comment = "联盟宝箱初始化")
	public void guildBox()
	{
		GC2GS_002_082_ReqGuildBoxInit msg = new GC2GS_002_082_ReqGuildBoxInit();
		getOwner().sendGameMsg(msg);

	}

    @Command(comment = "活动初始化")
    public void activityInit()
    {
        GC2GS_002_018_ReqActivityList msg = new GC2GS_002_018_ReqActivityList();
        getOwner().sendGameMsg(msg);

    }
}
