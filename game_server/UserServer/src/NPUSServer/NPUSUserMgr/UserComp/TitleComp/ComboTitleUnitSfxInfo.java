package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_ComboTitleSfx;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import USDB.Bo.PlayerComboTitleBO;

public class ComboTitleUnitSfxInfo extends _AComboTitleUnit <PlayerInfo_ComboTitleSfx>
{
	public ComboTitleUnitSfxInfo(NPUSUserData _userData, PlayerComboTitleBO _bo) 
	{
		super(_userData, _bo);
	}

	@Override
	protected void _onGainUnit() 
	{
		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_052_OnComboTitleSfxChg(this));
	}

	@Override
	public PlayerInfo_ComboTitleSfx toProto() 
	{
		PlayerInfo_ComboTitleSfx proto = new PlayerInfo_ComboTitleSfx();
		proto.setSfxId(getUnitId());
		proto.setViewed(isViewed());
		
		return proto;
	}
}
