package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_GAIN_ITEM_FORM;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;

public class NPPlayerEffectDealer_S_GAIN_ITEM_FORM extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_ITEM_FORM;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_GAIN_ITEM_FORM effect = (NPPlayerEffect_S_GAIN_ITEM_FORM) _effectInfo;
        
        int count = (int) NPPlayerVariableDeal.getInstance().CalculateVariableResult(_userData, effect.getCountVariableGroupObj(), _varVariableInfo);
        int multi = effect.getMulti();

        _userData.gainItem(effect.getItem(), count * multi, _context);
    }
}
