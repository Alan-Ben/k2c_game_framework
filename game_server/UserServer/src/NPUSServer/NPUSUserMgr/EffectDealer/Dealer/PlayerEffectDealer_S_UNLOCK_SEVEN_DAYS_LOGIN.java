package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPPlayerParam;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class PlayerEffectDealer_S_UNLOCK_SEVEN_DAYS_LOGIN extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_UNLOCK_SEVEN_DAYS_LOGIN;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        _userData.getPlayerComponent().getBo().saveSevenDaysLoginCalOffset(
                _userData.getUSServer().getBM(), (int) Math.max(_userData.getParam(ENPPlayerParam.LOGIN_DAY_COUNT), 1));

        _userData.setParam(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT, -1);
    }
}
