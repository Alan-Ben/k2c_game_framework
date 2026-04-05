package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_MARS_CHG_MOOD;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerEffectDealer_S_MARS_CHG_MOOD extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_CHG_MOOD;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
    	NPPlayerEffect_S_MARS_CHG_MOOD effect = (NPPlayerEffect_S_MARS_CHG_MOOD) _effectInfo;
    	
    	_userData.getMarsBuildingComponent().getCompInfo().chgMoodIndex(effect.getCount(), _context);
    }
}
