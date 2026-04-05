package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_DEL_BUFF;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerEffectDealer_S_DEL_BUFF extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_DEL_BUFF;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_DEL_BUFF effect = (NPPlayerEffect_S_DEL_BUFF) _effectInfo;
        
        _userData.getBuffComponent().removeBuff(effect.buffId(), _context);
    }
}
