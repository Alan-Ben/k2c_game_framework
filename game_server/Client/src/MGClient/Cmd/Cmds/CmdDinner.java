package MGClient.Cmd.Cmds;

import GC2GS.p019_DinnerOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "宴会", name = "Dinner")
public class CmdDinner extends CmdBase {
	@Command(comment = "开启宴会（普通模式）(宴会配置ID)")
	public void StartDinner(long _param0)
	{
		GC2GS_019_001_ReqStartDinner msg = new GC2GS_019_001_ReqStartDinner();
		msg.setDinnerId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "开启宴会（许可证模式）(许可证实例ID)")
	public void StartDinnerByPermit(long _param0)
	{
		GC2GS_019_002_ReqStartDinnerByPermit msg = new GC2GS_019_002_ReqStartDinnerByPermit();
		msg.setPermitInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "宴会交互记录()")
	public void JoinedPlayerList()
	{
		GC2GS_019_003_ReqJoinedPlayerList msg = new GC2GS_019_003_ReqJoinedPlayerList();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "开宴索引记录列表()")
	public void GetStartLogIdxList()
	{
		GC2GS_019_004_ReqGetStartLogIdxList msg = new GC2GS_019_004_ReqGetStartLogIdxList();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "开宴记录(宴会实例ID)")
	public void GetStartLogInfo(long _param0)
	{
		GC2GS_019_005_ReqGetStartLogInfo msg = new GC2GS_019_005_ReqGetStartLogInfo();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "宴会索引列表(当前页数,展示数量，上限不超过100)")
	public void GetDinnerIdxList(int _param0,int _param1)
	{
		GC2GS_019_006_ReqGetDinnerIdxList msg = new GC2GS_019_006_ReqGetDinnerIdxList();
		msg.setPage(_param0);
		msg.setNum(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "宴会信息(宴会实例ID)")
	public void GetDinnerInfo(long _param0)
	{
		GC2GS_019_007_ReqGetDinnerInfo msg = new GC2GS_019_007_ReqGetDinnerInfo();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "加入宴会(宴会实例ID,消耗配置ID)")
	public void JoinDinner(long _param0,long _param1)
	{
		GC2GS_019_008_ReqJoinDinner msg = new GC2GS_019_008_ReqJoinDinner();
		msg.setInstanceId(_param0);
		msg.setCostId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "领取开宴结算奖励()")
	public void TakeOpenReward()
	{
		GC2GS_019_009_ReqTakeOpenReward msg = new GC2GS_019_009_ReqTakeOpenReward();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "取指定宴会的上一条宴会数据(宴会实例ID,当前排序)")
	public void GetPreDinnerInfo(long _param0,int _param1)
	{
		GC2GS_019_011_ReqGetPreDinnerInfo msg = new GC2GS_019_011_ReqGetPreDinnerInfo();
		msg.setInstanceId(_param0);
		msg.setIdx(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "取指定宴会的下一条宴会数据(宴会实例ID,当前排序)")
	public void GetNextDinnerInfo(long _param0,int _param1)
	{
		GC2GS_019_012_ReqGetNextDinnerInfo msg = new GC2GS_019_012_ReqGetNextDinnerInfo();
		msg.setInstanceId(_param0);
		msg.setIdx(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "对指定玩家发起邀请(目标玩家CID)")
	public void SendInviteToPlayer(long _param0)
	{
		GC2GS_019_013_ReqSendInviteToPlayer msg = new GC2GS_019_013_ReqSendInviteToPlayer();
		msg.setTargetCid(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "检查宴会邀请信息(宴会实例ID)")
	public void CheckDinnerInvite(long _param0)
	{
		GC2GS_019_014_ReqCheckDinnerInvite msg = new GC2GS_019_014_ReqCheckDinnerInvite();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
}
