package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPPlayerParam;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class NPPlayerEffectDealer_S_GAIN_AND_SET_CUTE_ACTOR extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_GAIN_AND_SET_CUTE_ACTOR;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
    	NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR effect = (NPPlayerEffect_S_GAIN_AND_SET_CUTE_ACTOR) _effectInfo;

        //获取用户的Q版形象道具
        _userData.getCuteActorComponent().gainItem(effect.getCuteActorId(), effect.getEffectTimeS(), false, _context);

        //设置玩家Q版形象道具
        _userData.getPlayerComponent().setParam(ENPPlayerParam.CUTE_ACTOR, effect.getCuteActorId());
    }
}
