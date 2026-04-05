package MGClient.Cmd.Cmds;

import Common.GuildEnum.EGuildBoxType;
import GC2GS.p042_GuildRelatedOp.GC2GS_042_007_ReqGainGuildRewardBox;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "联盟宝箱", name = "GuildBox")
public class CmdGuildBox extends CmdBase 
{
	@Command(comment = "领取联盟宝箱奖励")
	public void GainGuildRewardBox(EGuildBoxType _boxType, long _id)
	{
		GC2GS_042_007_ReqGainGuildRewardBox msg = new GC2GS_042_007_ReqGainGuildRewardBox();
		msg.setBoxType(_boxType);
		msg.setId(_id);
		
		getOwner().sendGameMsg(msg);
	}
}
