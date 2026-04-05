package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_GAIN_LAZY_CD;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;
import NPUSServer.USLog;

public class NPPlayerEffectDealer_S_GAIN_LAZY_CD extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_LAZY_CD;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_GAIN_LAZY_CD effect = (NPPlayerEffect_S_GAIN_LAZY_CD) _effectInfo;

        PlayerLazyCD lazyCd = _userData.getLazyCDComponent().ensureLazyCD(effect.getCdId());
        if (lazyCd == null)
        {
            USLog.error(_userData.getUSServer(), "NPPlayerEffectDealer_S_GAIN_LAZY_CD ensureLazyCd fail cid:{} cdId:{} num:{}", _userData.getCid(), effect.getCdId(), effect.getNum());
            return;
        }

        lazyCd.addCountExt(effect.getNum());
    }
}
