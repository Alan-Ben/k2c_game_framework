package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_BUF_LAYER;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;

public class NPPlayerConditionDealer_CS_BUF_LAYER extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_BUF_LAYER;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_BUF_LAYER cond = (NPPlayerCondition_CS_BUF_LAYER) _cond;

        //默认值0，用于buff不存在情况
        int layer = 0;

        PlayerBuffInfo info = _userData.getBuffComponent().lookupBuff(cond.bufId());
        if (null != info)
            layer = info.getBo().getLayer();

        return NPPlayerConditionDealerMgr.isRange(layer, cond.minValue(), cond.maxValue());
    }
}
