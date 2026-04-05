package MGClient.Cmd.Cmds;

import GC2GS.p008_TravelOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "游历", name = "Travel")
public class CmdTravel extends CmdBase {
	@Command(comment = "单次游历()")
	public void StartTravel()
	{
		GC2GS_008_001_ReqStartTravel msg = new GC2GS_008_001_ReqStartTravel();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "一键游历()")
	public void AkeyTravel()
	{
		GC2GS_008_002_ReqAkeyTravel msg = new GC2GS_008_002_ReqAkeyTravel();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理奖励游历事件(游历事件ID)")
	public void DealRewardTravel(long _param0)
	{
		GC2GS_008_003_ReqDealRewardTravel msg = new GC2GS_008_003_ReqDealRewardTravel();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理妃子酒馆游历事件(游历事件ID,妃子ID,消耗配置ID)")
	public void DealConsortBarTravel(long _param0,long _param1,long _param2)
	{
		GC2GS_008_004_ReqDealConsortBarTravel msg = new GC2GS_008_004_ReqDealConsortBarTravel();
		msg.setInstanceId(_param0);
		msg.setConsortId(_param1);
		msg.setCostId(_param2);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理兑换游历事件(游历事件ID,是否兑换 true-兑换)")
	public void DealChangeTravel(long _param0,boolean _param1)
	{
		GC2GS_008_005_ReqDealChangeTravel msg = new GC2GS_008_005_ReqDealChangeTravel();
		msg.setInstanceId(_param0);
		msg.setIsChange(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理妃子邀约游历事件(游历事件ID,妃子ID)")
	public void DealInvitationTravel(long _param0,long _param1)
	{
		GC2GS_008_006_ReqDealInvitationTravel msg = new GC2GS_008_006_ReqDealInvitationTravel();
		msg.setInstanceId(_param0);
		msg.setConsortId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理大臣加国力游历事件(游历事件ID,大臣ID)")
	public void DealAddPowerTravel(long _param0,long _param1)
	{
		GC2GS_008_007_ReqDealAddPowerTravel msg = new GC2GS_008_007_ReqDealAddPowerTravel();
		msg.setInstanceId(_param0);
		msg.setHeroId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理增加妃子好感度游历事件(游历事件ID)")
	public void DealConsortLikeTravel(long _param0)
	{
		GC2GS_008_008_ReqDealConsortLikeTravel msg = new GC2GS_008_008_ReqDealConsortLikeTravel();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理增加妃子亲密度游历事件(游历事件ID)")
	public void DealConsortIntimacyTravel(long _param0)
	{
		GC2GS_008_009_ReqDealConsortIntimacyTravel msg = new GC2GS_008_009_ReqDealConsortIntimacyTravel();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "处理增加必生卷王游历事件(游历事件ID)")
	public void DealGiftedTravel(long _param0)
	{
		GC2GS_008_010_ReqDealGiftedTravel msg = new GC2GS_008_010_ReqDealGiftedTravel();
		msg.setInstanceId(_param0);
		getOwner().sendGameMsg(msg);

	}
}
