package MGClient.Cmd.Cmds;

import GC2GS.p018_PlayerSkinOp.*;
import MGClient.Cmd.Annotation.Command;
import MGClient.Cmd.CmdBase;

@MGClient.Cmd.Annotation.Commander(comment = "玩家形象", name = "PlayerSkin")
public class CmdPlayerSkin extends CmdBase {
	@Command(comment = "穿戴普通称号(空)")
	public void SetCommTitle(long _param0)
	{
		GC2GS_018_001_ReqSetCommTitle msg = new GC2GS_018_001_ReqSetCommTitle();
		msg.setTitle(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "穿戴组合称号(空,空,空)")
	public void SetComboTitle(long _param0,long _param1,long _param2)
	{
		GC2GS_018_002_ReqSetComboTitle msg = new GC2GS_018_002_ReqSetComboTitle();
		msg.setPreId(_param0);
		msg.setSfxId(_param1);
		msg.setBgId(_param2);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "设置称号是否可展示(空)")
	public void SetTitleShow(boolean _param0)
	{
		GC2GS_018_003_ReqSetTitleShow msg = new GC2GS_018_003_ReqSetTitleShow();
		msg.setIsShow(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "解锁组合称号前缀(空)")
	public void UnlockComboTitlePre(long _param0)
	{
		GC2GS_018_004_ReqUnlockComboTitlePre msg = new GC2GS_018_004_ReqUnlockComboTitlePre();
		msg.setPreId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "解锁组合称号后缀(空)")
	public void UnlockComboTitleSfx(long _param0)
	{
		GC2GS_018_005_ReqUnlockComboTitleSfx msg = new GC2GS_018_005_ReqUnlockComboTitleSfx();
		msg.setSfxId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "解锁组合称号底色(空)")
	public void UnlockComboTitleBg(long _param0)
	{
		GC2GS_018_006_ReqUnlockComboTitleBg msg = new GC2GS_018_006_ReqUnlockComboTitleBg();
		msg.setBgId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "查看组合称号前缀(空)")
	public void viewComboTitlePre(long _param0)
	{
		GC2GS_018_007_ReqviewComboTitlePre msg = new GC2GS_018_007_ReqviewComboTitlePre();
		msg.setPreId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "查看组合称号后缀(空)")
	public void ViewComboTitleSfx(long _param0)
	{
		GC2GS_018_008_ReqViewComboTitleSfx msg = new GC2GS_018_008_ReqViewComboTitleSfx();
		msg.setSfxId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "查看组合称号底色(空)")
	public void ViewComboTitleBg(long _param0)
	{
		GC2GS_018_009_ReqViewComboTitleBg msg = new GC2GS_018_009_ReqViewComboTitleBg();
		msg.setBgId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "解锁玩家皮肤(空)")
	public void UnlockPlayerSkin(long _param0)
	{
		GC2GS_018_010_ReqUnlockPlayerSkin msg = new GC2GS_018_010_ReqUnlockPlayerSkin();
		msg.setSkinId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "穿戴皮肤(空)")
	public void SetCurPlayerSkin(long _param0)
	{
		GC2GS_018_011_ReqSetCurPlayerSkin msg = new GC2GS_018_011_ReqSetCurPlayerSkin();
		msg.setSkinId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "脱下皮肤()")
	public void UnsetCurPlayerSkin()
	{
		GC2GS_018_012_ReqUnsetCurPlayerSkin msg = new GC2GS_018_012_ReqUnsetCurPlayerSkin();
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "升级皮肤等级(空)")
	public void UpgradePlayerSkin(long _param0)
	{
		GC2GS_018_013_ReqUpgradePlayerSkin msg = new GC2GS_018_013_ReqUpgradePlayerSkin();
		msg.setSkinId(_param0);
		getOwner().sendGameMsg(msg);

	}
	@Command(comment = "查看普通称号(空)")
	public void viewTitle(long _param0)
	{
		GC2GS_018_015_ReqviewTitle msg = new GC2GS_018_015_ReqviewTitle();
		msg.setTitle(_param0);
		getOwner().sendGameMsg(msg);

	}
}
