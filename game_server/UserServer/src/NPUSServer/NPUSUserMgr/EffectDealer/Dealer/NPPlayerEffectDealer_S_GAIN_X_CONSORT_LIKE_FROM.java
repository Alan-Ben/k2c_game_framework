package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;

public class NPPlayerEffectDealer_S_GAIN_X_CONSORT_LIKE_FROM extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_X_CONSORT_LIKE_FROM;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM effect = (NPPlayerEffect_S_GAIN_X_CONSORT_LIKE_FROM) _effectInfo;

        //已获得妃子不能获得好感度
        if (_userData.getConsortComponent().hasConsort(effect.getConsortId()))
            return;

        //计算增加值
        int count = (int) NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, effect.getValueObj(), _varVariableInfo);
        _userData.getTravelComponent().consortAddLike(effect.getConsortId(), count, _context);
    }
}
