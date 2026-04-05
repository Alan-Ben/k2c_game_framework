package NPUSServer.NPUSUserMgr.EffectDealer.Dealer;

import Common.BuildingObj.Building_EffectGainWorkers;
import GS2GC.p010_BuildingOp.GS2GC_010_056_OnEffectGainBusinessWorkes;
import NPCommon.Util.Pair.WCGPair;
import NPEnum.ENPPlayerEffectType;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerEffect.EffectObj.NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS;
import NPGameRes.GameObjs.PlayerEffect._ANPPlayerEffectInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer._ANPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.util.List;

public class PlayerEffectDealer_S_RND_GAIN_X_BUSINESS_WORKERS extends _ANPPlayerEffectDealer
{
    @Override
    public ENPPlayerEffectType effectType()
    {
        return ENPPlayerEffectType.S_RND_GAIN_X_BUSINESS_WORKERS;
    }

    @Override
    public void dealEffect(_ANPPlayerEffectInfo _effectInfo, NPUSUserData _userData, NPVarInfo _varVariableInfo, NPPlayerContext _context)
    {
        NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS effect = (NPPlayerEffect_S_RND_GAIN_X_BUSINESS_WORKERS) _effectInfo;

        //计算增加值
        int num = effect.getNum();

        //计算使用次数
        long useCount = _varVariableInfo.getValue(ENPPlayerVariableVarType.USE_COUNT.ordinal());
        if (useCount == 0)
        {
            useCount = 1;
        }

        List<WCGPair<Long, Integer>> randomAddWorkersResult = _userData.getBuildingComponent().randomAddWorkers(num, (int) useCount, _context);

        //推送展示协议
        GS2GC_010_056_OnEffectGainBusinessWorkes proto = new GS2GC_010_056_OnEffectGainBusinessWorkes();
        for (WCGPair<Long, Integer> pair : randomAddWorkersResult)
        {
            proto.addList(new Building_EffectGainWorkers(pair.first, pair.second));
        }
        _userData.sendMsgToGC(proto);
    }
}
