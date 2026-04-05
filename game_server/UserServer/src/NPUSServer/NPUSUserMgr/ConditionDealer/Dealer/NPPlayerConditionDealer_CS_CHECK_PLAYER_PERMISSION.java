package NPUSServer.NPUSUserMgr.ConditionDealer.Dealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPUSServer.NPUSUserMgr.ConditionDealer._ANPPlayerConditionDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.PlayerPermissionsComp.PlayerPermissionsInfo;

public class NPPlayerConditionDealer_CS_CHECK_PLAYER_PERMISSION extends _ANPPlayerConditionDealer
{
    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_CHECK_PLAYER_PERMISSION;
    }

    @Override
    public boolean isEnable(_ANPBasicPlayerCondition _cond, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION cond = (NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION) _cond;
        
        PlayerPermissionsInfo info = _userData.getPlayerPermissionsComponent().lookup(cond.id());
        if(null == info)
        	return false;
        
        return info.isEffect();
    }
}
