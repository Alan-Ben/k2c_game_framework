package NPUSServer.NPUSUserMgr.EffectDealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public abstract class _ANPPlayerEffectDealer
{
    public abstract ENPPlayerEffectType effectType();

    public abstract void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context);
}
