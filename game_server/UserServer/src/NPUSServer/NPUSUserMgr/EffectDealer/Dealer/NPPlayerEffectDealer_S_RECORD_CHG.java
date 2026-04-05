package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_RECORD_CHG;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerEffectDealer_S_RECORD_CHG extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_RECORD_CHG;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_RECORD_CHG effect = (NPPlayerEffect_S_RECORD_CHG) _effectInfo;

        _userData.getRecordComponent().ensureRecord(effect.record(), effect.count(), effect.dealType(), _context);
    }
}
