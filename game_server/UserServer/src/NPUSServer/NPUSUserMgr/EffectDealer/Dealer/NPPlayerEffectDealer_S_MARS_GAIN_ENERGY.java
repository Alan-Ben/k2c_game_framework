package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import CommonEnum.ECurrency;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_MARS_GAIN_ENERGY;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerEffectDealer_S_MARS_GAIN_ENERGY extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_MARS_GAIN_ENERGY;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_MARS_GAIN_ENERGY effect = (NPPlayerEffect_S_MARS_GAIN_ENERGY) _effectInfo;

        long outputSpeed = _userData.getMarsBuildingComponent().getAllBuildingValue().getOutputValuePerMin();
        long outputCount = outputSpeed * effect.getMins();
        
        _userData.gainItem(ENPItemType.CURRENCY, ECurrency.MARS_ENERGY.ordinal(), outputCount, _context);
    }
}
