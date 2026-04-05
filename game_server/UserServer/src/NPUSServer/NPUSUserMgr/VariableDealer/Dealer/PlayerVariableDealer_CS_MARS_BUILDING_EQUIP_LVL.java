package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingEquipmentInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_MARS_BUILDING_EQUIP_LVL extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_MARS_BUILDING_EQUIP_LVL;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL obj = (NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL) _variableObj;

        MarsBuildingInfo info = _userData.getMarsBuildingComponent().lookupBuilding(obj.getId());
        if(null == info)
        	return 0;
        
        MarsBuildingEquipmentInfo equipInfo = info.getBuildingEquipmentMgr().lookup(obj.getEquipId());
        if(null == equipInfo)
        	return 0;
        
        return equipInfo.getLvl();
    }
}
