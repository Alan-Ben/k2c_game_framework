package MGClient.Cmd.Cmds;

import GC2GS.p015_ConsortOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "家人", name = "Consort")
public class CmdConsort extends CmdBase {
	@Command(comment = "家人-升级羁绊等级(家人ID)")
	public void UpgradeFettersLvl(long _param0)
	{
		GC2GS_015_001_ReqUpgradeFettersLvl msg = new GC2GS_015_001_ReqUpgradeFettersLvl();
		msg.setConsortId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-领悟经营技能等级(家人ID,经营技能ID,是否高级领悟)")
	public void UnderstandBusinessSkill(long _param0,long _param1,boolean _param2)
	{
		GC2GS_015_002_ReqUnderstandBusinessSkill msg = new GC2GS_015_002_ReqUnderstandBusinessSkill();
		msg.setConsortId(_param0);
		msg.setSkillId(_param1);
		msg.setIsAdvanced(_param2);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-升级加护技能等级(家人ID,经营技能ID)")
	public void UpgradeBlessSkill(long _param0,long _param1)
	{
		GC2GS_015_003_ReqUpgradeBlessSkill msg = new GC2GS_015_003_ReqUpgradeBlessSkill();
		msg.setConsortId(_param0);
		msg.setSkillId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-随机邀约()")
	public void CallRand()
	{
		GC2GS_015_004_ReqCallRand msg = new GC2GS_015_004_ReqCallRand();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-一键邀约()")
	public void CallAkey()
	{
		GC2GS_015_005_ReqCallAkey msg = new GC2GS_015_005_ReqCallAkey();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-指定邀约(家人ID,家人邀约类型ID)")
	public void CallAppoint(long _param0,long _param1)
	{
		GC2GS_015_006_ReqCallAppoint msg = new GC2GS_015_006_ReqCallAppoint();
		msg.setConsortId(_param0);
		msg.setConsortTravelId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-设置当前皮肤(家人ID,皮肤ID)")
	public void SetCurSkin(long _param0,long _param1)
	{
		GC2GS_015_007_ReqSetCurSkin msg = new GC2GS_015_007_ReqSetCurSkin();
		msg.setConsortId(_param0);
		msg.setSkinId(_param1);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-获取周期出游次数(家人邀约类型ID)")
	public void GetRoundTravelCount(long _param0)
	{
		GC2GS_015_008_ReqGetRoundTravelCount msg = new GC2GS_015_008_ReqGetRoundTravelCount();
		msg.setConsortTravelId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-获取所有经营技能数据(家人ID)")
	public void GetAllBusinessSkill(long _param0)
	{
		GC2GS_015_009_ReqGetAllBusinessSkill msg = new GC2GS_015_009_ReqGetAllBusinessSkill();
		msg.setConsortId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-升级星辉等级(家人ID)")
	public void UpgradeHaloLvl(long _param0)
	{
		GC2GS_015_010_ReqUpgradeHaloLvl msg = new GC2GS_015_010_ReqUpgradeHaloLvl();
		msg.setConsortId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-领取CG解锁奖励(家人Cg配置ID)")
	public void GetCgUnlockReward(long _param0)
	{
		GC2GS_015_011_ReqGetCgUnlockReward msg = new GC2GS_015_011_ReqGetCgUnlockReward();
		msg.setCgId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-解锁皮肤(皮肤ID)")
	public void UnlockSkin(long _param0)
	{
		GC2GS_015_012_ReqUnlockSkin msg = new GC2GS_015_012_ReqUnlockSkin();
		msg.setSkinId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "家人-解锁星辉(家人ID)")
	public void UnlockHalo(long _param0)
	{
		GC2GS_015_013_ReqUnlockHalo msg = new GC2GS_015_013_ReqUnlockHalo();
		msg.setConsortId(_param0);
		getOwner().sendGameMsg(msg);

	}
}
