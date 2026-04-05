package MGClient.Cmd.Cmds;

import GC2GS.p021_PlayerInfo.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "好友", name = "Friend")
public class CmdFriend extends CmdBase {
	@Command(comment = "发起好友邀请(目标玩家CID)")
	public void SendFriendApply(long _param0)
	{
		GC2GS_021_030_ReqSendFriendApply msg = new GC2GS_021_030_ReqSendFriendApply();
		msg.setTargetCid(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理好友邀请(是否同意，发起请求玩家CID)")
	public void DealFriendApply(boolean _param0,int _param1)
	{
		GC2GS_021_031_ReqDealFriendApply msg = new GC2GS_021_031_ReqDealFriendApply();
		msg.setIsAgree(_param0);
		msg.setApplyCid(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "移除好友(目标玩家CID)")
	public void JoinDinner(long _param0)
	{
		GC2GS_021_032_ReqRemoveFriend msg = new GC2GS_021_032_ReqRemoveFriend();
		msg.setTargetCid(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "移除好友申请(申请玩家CID)")
	public void StartDinner(int _param0)
	{
		GC2GS_021_033_ReqFriendApplyExpired msg = new GC2GS_021_033_ReqFriendApplyExpired();
		msg.setApplyCid(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "好友推荐")
	public void friendTip()
	{
		GC2GS_021_034_ReqFriendRecommend msg = new GC2GS_021_034_ReqFriendRecommend();
		getOwner().sendGameMsg(msg);

	}
}
