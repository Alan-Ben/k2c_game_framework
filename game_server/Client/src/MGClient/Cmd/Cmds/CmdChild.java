package MGClient.Cmd.Cmds;

import GC2GS.p014_ChildOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "子嗣", name = "Child")
public class CmdChild extends CmdBase {
	@Command(comment = "设置子嗣（未成年）名称(子嗣（未成年）实例ID,子嗣名称)")
	public void SetChildName(long _param0,String _param1)
	{
		GC2GS_014_001_ReqSetChildName msg = new GC2GS_014_001_ReqSetChildName();
		msg.setId(_param0);
		msg.setName(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "训练子嗣（未成年）(子嗣（未成年）实例ID)")
	public void TrainChild(long _param0)
	{
		GC2GS_014_002_ReqTrainChild msg = new GC2GS_014_002_ReqTrainChild();
		msg.setId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "增加训练位脑力值(训练位ID,增加数量)")
	public void AddSeatEnergy(long _param0,int _param1)
	{
		GC2GS_014_003_ReqAddSeatEnergy msg = new GC2GS_014_003_ReqAddSeatEnergy();
		msg.setSeatId(_param0);
		msg.setAddCount(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "设置子嗣（未成年）毕业(子嗣（未成年）实例ID)")
	public void SetChildGraduate(long _param0)
	{
		GC2GS_014_004_ReqSetChildGraduate msg = new GC2GS_014_004_ReqSetChildGraduate();
		msg.setId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "获取子嗣（成年未婚）数据(子嗣实例ID)")
	public void GetUnMarriedAdult(long _param0)
	{
		GC2GS_014_006_ReqGetUnMarriedAdult msg = new GC2GS_014_006_ReqGetUnMarriedAdult();
		msg.setAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "获取子嗣（成年已婚）数据(子嗣实例ID)")
	public void GetMarriedAdult(long _param0)
	{
		GC2GS_014_007_ReqGetMarriedAdult msg = new GC2GS_014_007_ReqGetMarriedAdult();
		msg.setAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "获取对玩家的指定联姻请求(发起请求的玩家子嗣实例ID)")
	public void GetToMeApply(long _param0)
	{
		GC2GS_014_008_ReqGetToMeApply msg = new GC2GS_014_008_ReqGetToMeApply();
		msg.setApplyAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "拒绝对玩家的指定联姻请求(发起请求的玩家子嗣实例ID)")
	public void RefuseToMeApply(long _param0)
	{
		GC2GS_014_009_ReqRefuseToMeApply msg = new GC2GS_014_009_ReqRefuseToMeApply();
		msg.setApplyAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "一键拒绝对玩家的指定联姻请求()")
	public void AkeyRefuseToMeApply()
	{
		GC2GS_014_010_ReqAkeyRefuseToMeApply msg = new GC2GS_014_010_ReqAkeyRefuseToMeApply();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "同意对玩家的指定联姻请求(匹配的子嗣实例ID,发起请求的子嗣实例ID)")
	public void AgreeToMeApply(long _param0,long _param1)
	{
		GC2GS_014_011_ReqAgreeToMeApply msg = new GC2GS_014_011_ReqAgreeToMeApply();
		msg.setAdultId(_param0);
		msg.setApplyAdultId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "联姻请求-获取推荐玩家列表(匹配的子嗣实例ID)")
	public void GetRecommendPlayerList(long _param0)
	{
		GC2GS_014_012_ReqGetRecommendPlayerList msg = new GC2GS_014_012_ReqGetRecommendPlayerList();
		msg.setAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "联姻请求-对指定玩家发起请求(匹配的子嗣实例ID,目标玩家CID)")
	public void ApplyToPlayer(long _param0,long _param1)
	{
		GC2GS_014_013_ReqApplyToPlayer msg = new GC2GS_014_013_ReqApplyToPlayer();
		msg.setAdultId(_param0);
		msg.setTargetCid(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "联姻请求-对指定群体发起请求(子嗣实例ID)")
	public void ApplyToGroup(long _param0)
	{
		GC2GS_014_014_ReqApplyToGroup msg = new GC2GS_014_014_ReqApplyToGroup();
		msg.setAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "联姻请求-对指定群体发起请求(匹配的子嗣实例ID,发起请求的玩家CID,发起请求的子嗣实例ID)")
	public void AgreeApplyGroup(long _param0,long _param1,long _param2)
	{
		GC2GS_014_015_ReqAgreeApplyGroup msg = new GC2GS_014_015_ReqAgreeApplyGroup();
		msg.setAdultId(_param0);
		msg.setApplyCid(_param1);
		msg.setApplyAdultId(_param2);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "联姻请求-取消指定联姻申请(子嗣实例ID)")
	public void CancelApplyToPlayer(long _param0)
	{
		GC2GS_014_016_ReqCancelApplyToPlayer msg = new GC2GS_014_016_ReqCancelApplyToPlayer();
		msg.setAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "联姻请求-取消联姻池申请(子嗣实例ID)")
	public void CancelApplyToGroup(long _param0)
	{
		GC2GS_014_017_ReqCancelApplyToGroup msg = new GC2GS_014_017_ReqCancelApplyToGroup();
		msg.setAdultId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "获取联姻池待匹配子嗣数据(玩家CID,子嗣实例ID)")
	public void GetPoolAdult(long _param0,long _param1)
	{
		GC2GS_014_018_ReqGetPoolAdult msg = new GC2GS_014_018_ReqGetPoolAdult();
		msg.setCid(_param0);
		msg.setAdultId(_param1);
		getOwner().sendGameMsg(msg);

	}
}
