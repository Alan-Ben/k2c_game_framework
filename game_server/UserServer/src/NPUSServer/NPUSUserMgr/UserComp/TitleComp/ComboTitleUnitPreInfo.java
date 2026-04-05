package NPUSServer.NPUSUserMgr.UserComp.TitleComp;

import Common.NpPlayerInfoObj.PlayerInfo_ComboTitlePre;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_018_PlayerSkinOp;
import USDB.Bo.PlayerComboTitleBO;

public class ComboTitleUnitPreInfo extends _AComboTitleUnit <PlayerInfo_ComboTitlePre>
{
	public ComboTitleUnitPreInfo(NPUSUserData _userData, PlayerComboTitleBO _bo) 
	{
		super(_userData, _bo);
	}

	@Override
	protected void _onGainUnit() 
	{
		getUserData().sendMsgToGC(US2GCWriter_018_PlayerSkinOp.make_051_OnComboTitlePreChg(this));
	}

	@Override
	public PlayerInfo_ComboTitlePre toProto() 
	{
		PlayerInfo_ComboTitlePre proto = new PlayerInfo_ComboTitlePre();
		proto.setPreId(getUnitId());
		proto.setViewed(isViewed());
		
		return proto;
	}
}
