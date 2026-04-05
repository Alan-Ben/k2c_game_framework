package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import NPEnum.ENPPlayerEffectType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_START_QUEST;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPGameRes.Refs.Quest.RefQuest;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

public class NPPlayerEffectDealer_S_START_QUEST extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_START_QUEST;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_START_QUEST effect = (NPPlayerEffect_S_START_QUEST) _effectInfo;

        RefQuest ref = RefQuest.getMgr().get(effect.questId());
        if (null == ref)
        {
            USLog.error(_userData.getUSServer(), "Effect-S_START_QUEST fail, not find ref, {}", effect.questId());
            return;
        }

        _userData.getQuestComponent().startQuestByServer(ref, _context);
    }
}
