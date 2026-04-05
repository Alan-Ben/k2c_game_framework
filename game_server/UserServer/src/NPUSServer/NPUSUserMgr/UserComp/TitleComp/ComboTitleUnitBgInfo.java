package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_ComboTitleBg;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import USDB.Bo.PlayerComboTitleBO;

public class ComboTitleUnitBgInfo extends _AComboTitleUnit <PlayerInfo_ComboTitleBg>
{
	public ComboTitleUnitBgInfo(NPUSUserData _userData, PlayerComboTitleBO _bo) 
	{
		super(_userData, _bo);
	}

	@Override
	protected void _onGainUnit() 
	{
		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_053_OnComboTitleBgChg(this));
	}

	@Override
	public PlayerInfo_ComboTitleBg toProto() 
	{
		PlayerInfo_ComboTitleBg proto = new PlayerInfo_ComboTitleBg();
		proto.setBgId(getUnitId());
		proto.setViewed(isViewed());
		
		return proto;
	}
}
