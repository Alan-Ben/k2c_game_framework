package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENCounterDealType;
import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPPlayerParam;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 修改卷王子嗣次数效果处理器
 */
public class NPPlayerEffectDealer_S_CHG_BIRTH_GIFTDE_COUM extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_CHG_BIRTH_GIFTDE_COUM;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM effect = (NPPlayerEffect_S_CHG_BIRTH_GIFTDE_COUM) _effectInfo;

        if (effect.dealType() == ENCounterDealType.SET)
        {
            _userData.setParam(ENPPlayerParam.BIRTH_GIFTDE_COUM, effect.count());
        }
        else
        {
            // 默认ADD，REDUCE时count传负数
            _userData.incParam(ENPPlayerParam.BIRTH_GIFTDE_COUM, effect.count());
        }
    }
}
