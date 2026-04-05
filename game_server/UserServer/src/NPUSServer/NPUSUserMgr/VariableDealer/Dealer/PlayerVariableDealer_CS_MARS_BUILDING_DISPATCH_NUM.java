package NPUSServer.NPUSUserMgr.VariableDealer.Dealer;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsBuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsPeopleBuildingFunc;
import NPUSServer.NPUSUserMgr.VariableDealer._ANPPlayerVariableDealer;

public class PlayerVariableDealer_CS_MARS_BUILDING_DISPATCH_NUM extends _ANPPlayerVariableDealer
{
    public ENPPlayerVariableType VariableType()
    {
        return ENPPlayerVariableType.CS_MARS_BUILDING_DISPATCH_NUM;
    }

    public long PlayerVariableValue(NPUSUserData _userData, _ANPBasicPlayerVariableObj _variableObj, NPVarInfo _variableInfo)
    {
        NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM obj = (NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM) _variableObj;

        if(obj.getId() > 0) //指定火星建筑居民数
        {
        	MarsBuildingInfo info = _userData.getMarsBuildingComponent().lookupBuilding(obj.getId());
            if(null == info)
            	return 0;
            
            MarsPeopleBuildingFunc func = info.getPeopleFunc();
            if(null == func)
            	return 0;
            
            return func.getDispatchedNum();
        }
        else //全部火星建筑的居民数总和
        {
        	return _userData.getMarsBuildingComponent().getDispatchedNumSum();
        }
    }
}
