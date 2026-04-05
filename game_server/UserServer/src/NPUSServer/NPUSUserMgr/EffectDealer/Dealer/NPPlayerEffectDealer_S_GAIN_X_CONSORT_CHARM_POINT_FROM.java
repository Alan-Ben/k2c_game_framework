package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;

public class NPPlayerEffectDealer_S_GAIN_X_CONSORT_CHARM_POINT_FROM extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_X_CONSORT_CHARM_POINT_FROM;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM effect = (NPPlayerEffect_S_GAIN_X_CONSORT_CHARM_POINT_FROM) _effectInfo;

        //计算大臣id
        long consortId = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, effect.getConsortVariableObj(), _varVariableInfo);
        ConsortInfo consortInfo = _userData.getConsortComponent().lookup(consortId);
        if (consortInfo == null)
            return;

        //计算增加值
        long count = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, effect.getCountVariableObj(), _varVariableInfo);
        
        //计算倍数
        int multiple = effect.getMultiple();
        
        consortInfo.incrCharmPoint(count * multiple, _context);
    }
}
