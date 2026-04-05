package NPUSServer.NPUSUserMgr.UserComp.TravelComp.Dealer;

import Common.TravelEnum.ETravelEventType;
import Common.TravelObj.Travel_EventResult;
import GC2GS.p008_TravelOp.GC2GS_008_007_ReqDealAddPowerTravel;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.HeroErr;
import NPGameRes.Refs.Travel.RefTravelEvent;
import NPGameRes.Refs.Travel.RefTravelEventAddPower;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;

public class TravelEventDealer_AddPower extends _ATravelEventDealer <GC2GS_008_007_ReqDealAddPowerTravel>
{
	@Override
	public ETravelEventType getType() 
	{
		return ETravelEventType.ADD_POWER;
	}

	//大臣增加国力
	@Override
	protected void _dealExt(TravelEventDealerResult _result, TravelCanDealEventInfo _info, GC2GS_008_007_ReqDealAddPowerTravel _msgParam, NPPlayerContext _context) 
	{
		RefTravelEventAddPower ref = _info.getRef().addPowerEventRef;
		if(null == ref)
		{
			_result.setErrCode(CommErr.REF_NOT_FOUND.getCode());
			return;
		}
		
		if(null == _msgParam)
		{
			_result.setErrCode(CommErr.PARAM_ERROR.getCode());
			return;
		}
		
		HeroInfo hero = _info.getUserData().getHeroComponent().lookupHero(_msgParam.getHeroId());
		if(null == hero)
		{
			_result.setErrCode(HeroErr.HERO_NOT_FOUND.getCode());
			return;
		}
		
		hero.addTravelAddPower(ref.add_power, _context);
	}

	@Override
	public boolean canAkeySpecDeal() 
	{
		return false;
	}
	
	@Override
	public Travel_EventResult dealAkeySpec(NPUSUserData _userData, RefTravelEvent _ref, NPPlayerContext _context) 
	{
		return null;
	}
}
